using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using RamGecXNAControls;
using RamGecXNAControlsExtensions;
using System.Diagnostics;
using RamGecXNAControls.ExtendedControls;

namespace Window_Designer
{
    public class MainClass : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GUIManager guiManager;

        Texture2D imageTexture;
        Texture2D dotTexture;
        Color gridColor = Color.White;

        Window targetWindow = null;
        System.Windows.Forms.Cursor cursor = System.Windows.Forms.Cursors.Default;

        GUIControl controlToAdd = null;
        GUIControl activeControl = null;
        GUIControl movingControl = null;

        Window statisticsWindow = null;
        Point movingOffset = Point.Zero;
        bool floatingControls = true;
        bool showGrid = false;
        bool snapToGrid = true;
        int gridSize = 10;

        Stopwatch drawSW = new Stopwatch();
        Stopwatch updateSW = new Stopwatch();

        long statisticsFrequency = 0;
        long fpsFrames = 0;
        double fpsTime = 0;
        double fps = 0;

        public MainClass()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // setut the XNA window
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            imageTexture = Content.Load<Texture2D>("logo");

            // texture for drawing grid
            dotTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            dotTexture.SetData<Color>(new Color[] { gridColor });

            guiManager = new GUIManager(this, "Themes", "Default");

            #region Controls Window
            guiManager.LoadControls("Content\\controls.xml");

            (guiManager.GetControl("LogoImage") as Image).Texture = imageTexture;

            guiManager.GetControl("ResetWindowButton").OnClick += (s) => { ResetWindow(); };
            (guiManager.GetControl("ShowStatisticsBox") as CheckBox).OnChanged += (s) => { ToggleStatisticsWindow(); };
            (guiManager.GetControl("FloatingControlsBox") as CheckBox).OnChanged += (s) => { floatingControls = (s as CheckBox).Checked; };
            (guiManager.GetControl("SnapToGridBox") as CheckBox).OnChanged += (s) => { snapToGrid = (s as CheckBox).Checked; };
            (guiManager.GetControl("ShowGridBox") as CheckBox).OnChanged += (s) => { showGrid = (s as CheckBox).Checked; };
            (guiManager.GetControl("GridSizeBox") as TextBox).OnSubmit += (s) => { gridSize = Int32.Parse((s as TextBox).Text); };

            (guiManager.GetControl("AddControlButton") as Button).OnClick += (s) =>
            {
                ListBox control = (guiManager.GetControl("ControlsList") as ListBox);

                if (control.SelectedItems.Count <= 0)
                    return;

                AddControl(control.SelectedString);
                cursor = System.Windows.Forms.Cursors.Cross;
            };

            (guiManager.GetControl("RemoveControlButton") as Button).OnClick += (s) =>
            {
                if (activeControl != null)
                {
                    if (activeControl.Parent != null)
                        activeControl.Parent.Controls.Remove(activeControl);

                    activeControl = null;
                }
            };

            (guiManager.GetControl("SaveButton") as Button).OnClick += (s) => 
            {
                FileDialog fd = new FileDialog(Point.Zero, guiManager, new FileDialog.SelectClickEventHandler((filePath) =>
                    {
                        guiManager.SaveControl(filePath, targetWindow);
                    }));
                fd.Show();

                (new MessageBox(guiManager, "Window has been saved successfully")).Show();
            };

            (guiManager.GetControl("LoadButton") as Button).OnClick += (s) =>
            {
                FileDialog fd = new FileDialog(Point.Zero, guiManager, new FileDialog.SelectClickEventHandler((filePath) =>
                {
                    guiManager.Controls.Remove(targetWindow);
                    targetWindow = null;
                    List<GUIControl> loadedControls = guiManager.LoadControls(filePath);

                    // remove Properties window if open
                    if (guiManager.GetControl("PropertiesWindow") != null)
                        guiManager.Controls.Remove(guiManager.GetControl("PropertiesWindow"));

                    foreach (GUIControl control in loadedControls)
                    {
                        if (control is Window)
                            targetWindow = control as Window;
                        AssignEvents(control);
                    }

                    (new MessageBox(guiManager, "Window has been loaded successfully")).Show();
                }));
                fd.Show();
            };

            ListBox themesList = guiManager.GetControl("ThemesList") as ListBox;
            string[] themes = System.IO.Directory.GetDirectories(Environment.CurrentDirectory + "\\Themes");

            foreach (string themeName in themes)
                themesList.Items.Add(System.IO.Path.GetFileName(themeName));

            themesList.OnSelectItem += (s, i) =>
                {
                    guiManager.LoadTheme("Themes", (s as ListBox).SelectedString);
                };

            #endregion

            // show initial empty window
            ResetWindow();
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();
            System.Windows.Forms.Cursor.Current = cursor;

            // escape closes the app
            if (keyboardState.IsKeyDown(Keys.Escape))
                this.Exit();

            #region Floating Controls
            // moving controls
            if (floatingControls)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && movingControl != null)
                {
                    // do the moving

                    if (movingOffset == Point.Zero) // left click just occured
                    {
                        movingOffset.X = mouseState.X - movingControl.AbsoluteBounds.X;
                        movingOffset.Y = mouseState.Y - movingControl.AbsoluteBounds.Y;
                    }
                    else // mouse is moving
                    {
                        int xx = movingControl.Bounds.X + mouseState.X - movingControl.AbsoluteBounds.X - movingOffset.X;
                        int yy = movingControl.Bounds.Y + mouseState.Y - movingControl.AbsoluteBounds.Y - movingOffset.Y;

                        if (snapToGrid)
                        {
                            int txx = (xx / gridSize) * gridSize;
                            if (xx - txx < (gridSize / 2))
                                xx = txx;
                            else
                                xx = txx + gridSize;

                            int tyy = (yy / gridSize) * gridSize;
                            if (yy - tyy < (gridSize / 2))
                                yy = tyy;
                            else
                                yy = tyy + gridSize;
                        }

                        movingControl.Bounds.X = xx;
                        movingControl.Bounds.Y = yy;
                    }

                }

                if (mouseState.LeftButton == ButtonState.Released && movingControl != null)
                {
                    movingControl = null;
                    movingOffset = Point.Zero;
                }
            }
            #endregion

            #region Add Control
            if (mouseState.LeftButton == ButtonState.Pressed &&
                cursor == System.Windows.Forms.Cursors.Cross &&
                controlToAdd != null)
            {
                GUIControl clickedControl = guiManager.GetControl(mouseState.X, mouseState.Y);

                if (clickedControl != null)
                {
                    if (clickedControl is TabsContainer)
                        clickedControl = (clickedControl as TabsContainer).Controls[(clickedControl as TabsContainer).CurrentTab];

                    controlToAdd.Bounds.X = mouseState.X - clickedControl.AbsoluteBounds.X;
                    controlToAdd.Bounds.Y = mouseState.Y - clickedControl.AbsoluteBounds.Y;

                    clickedControl.Controls.Add(controlToAdd);
                }
                controlToAdd = null;
                cursor = System.Windows.Forms.Cursors.Default;
            }
            else if (mouseState.RightButton == ButtonState.Pressed) // action canceled
            {
                controlToAdd = null;
                cursor = System.Windows.Forms.Cursors.Default;
            }
            #endregion

            #region Delete control on CTRL+DEL
            if ((keyboardState.IsKeyDown(Keys.LeftControl) || keyboardState.IsKeyDown(Keys.RightControl)) && keyboardState.IsKeyDown(Keys.Delete))
            {
                if (activeControl != null)
                {
                    if (activeControl.Parent != null)
                        activeControl.Parent.Controls.Remove(activeControl);

                    activeControl = null;
                }
            }
            #endregion

            #region Statistics
            if (statisticsWindow != null)
            {
                fpsTime += gameTime.ElapsedGameTime.TotalMilliseconds;

                if (fpsTime > 1000f)
                {
                    fps = fpsFrames;
                    fpsFrames = 0;
                    fpsTime = 0;
                }
                if (statisticsFrequency % 20 == 0)
                {
                    (guiManager.GetControl("infoCount") as Label).Text = "GUI Controls Count: " + guiManager.GetAllControls().Count;
                    (guiManager.GetControl("infoDraw") as Label).Text = "GUI Drawing Time (ms): " + drawSW.Elapsed.TotalMilliseconds.ToString("F");
                    (guiManager.GetControl("infoUpdate") as Label).Text = "GUI Update Time (ms): " + updateSW.Elapsed.TotalMilliseconds.ToString("F");
                    (guiManager.GetControl("infoFPS") as Label).Text = "Frames Per Second: " + fps.ToString();
                }
            }
            #endregion

            updateSW.Reset();
            updateSW.Start();
            guiManager.Update(gameTime);
            updateSW.Stop();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            fpsFrames++;

            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            drawSW.Reset();
            drawSW.Start();
            guiManager.Draw(spriteBatch);
            drawSW.Stop();

            #region ShowGrid
            if (showGrid)
            {
                if (targetWindow != null && targetWindow.IsMouseOver)
                {
                    GUIControl ctrl = guiManager.GetControl(Mouse.GetState().X, Mouse.GetState().Y);
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                        ctrl = ctrl.Parent;

                    if (ctrl != null)
                    {
                        // vertical
                        int x = ctrl.AbsoluteBounds.X; //gridSize;
                        while (x < ctrl.AbsoluteBounds.X + ctrl.AbsoluteBounds.Width)
                        {
                            spriteBatch.Draw(dotTexture, new Rectangle(x, ctrl.AbsoluteBounds.Y, 1, ctrl.AbsoluteBounds.Height), Color.LightGray);
                            x += gridSize;
                        }

                        // horizontal
                        x = ctrl.AbsoluteBounds.Y; //gridSize;
                        while (x < ctrl.AbsoluteBounds.Y + ctrl.AbsoluteBounds.Height)
                        {
                            spriteBatch.Draw(dotTexture, new Rectangle(ctrl.AbsoluteBounds.X, x, ctrl.AbsoluteBounds.Width, 1), Color.LightGray);
                            x += gridSize;
                        }
                    }
                }
            }
            #endregion

            spriteBatch.End();
            base.Draw(gameTime);
        }

        void ToggleStatisticsWindow()
        {
            if (statisticsWindow == null)
            {
                statisticsWindow = new Window(new Rectangle(400, 10, 220, 160), "Statistics", "StatisticsWindow");
                guiManager.Controls.Add(statisticsWindow);

                GroupBox groupBox = new GroupBox(new Rectangle(10, 30, 200, 120), "Debug");
                statisticsWindow.Controls.Add(groupBox);
                groupBox.Controls.Add(new Label(new Rectangle(10, 20, 100, 16), "GUI Controls Count:", "infoCount"));
                groupBox.Controls.Add(new Label(new Rectangle(10, 40, 100, 16), "GUI Drawing Time (ms):", "infoDraw"));
                groupBox.Controls.Add(new Label(new Rectangle(10, 60, 100, 16), "GUI Update Time (ms):", "infoUpdate"));
                groupBox.Controls.Add(new Label(new Rectangle(10, 80, 100, 16), "Frames Per Second:", "infoFPS"));
            }
            else
            {
                guiManager.Controls.Remove(statisticsWindow);
                statisticsWindow = null;
            }
        }

        void ResetWindow()
        {
            if (targetWindow != null)
                guiManager.Controls.Remove(targetWindow);

            cursor = System.Windows.Forms.Cursors.Default;
            targetWindow = new Window(new Rectangle(10, 340, 600, 400));
            targetWindow.Title = "Window Control";
            targetWindow.Name = "Window";
            targetWindow.OnClick += (s) => { ShowGeneralPropertiesWindow(s); };
            guiManager.Controls.Add(targetWindow);
        }

        void ShowGeneralPropertiesWindow(GUIControl control)
        {
            GUIControl propertiesWindow;
            if ((propertiesWindow = guiManager.GetControl("PropertiesWindow")) != null)
            {
                guiManager.Controls.Remove(propertiesWindow);
            }

            RamGecXNAControls.Window nWindow = new Window(new Rectangle(700, 10, 260, 400));
            nWindow.Title = "Properties";
            nWindow.Name = "PropertiesWindow";
            guiManager.Controls.Add(nWindow);

            int yOffset = 32;

            GroupBox generalBox = new GroupBox(new Rectangle(10, yOffset, 240, 1), "General");
            nWindow.Controls.Add(generalBox);

            yOffset = 16;

            CheckBox enabledBox = new CheckBox(new Rectangle(10, yOffset, 60, 16), "Enabled");
            enabledBox.Checked = control.Enabled;
            enabledBox.OnClick += (s) => { control.Enabled = (s as CheckBox).Checked; };
            generalBox.Controls.Add(enabledBox);
            CheckBox visibleBox = new CheckBox(new Rectangle(120, yOffset, 60, 16), "Visible");
            visibleBox.Checked = control.Visible;
            visibleBox.OnClick += (s) => { control.Visible = (s as CheckBox).Checked; };
            generalBox.Controls.Add(visibleBox);

            yOffset += 24;

            generalBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 80, 16), "Location: "));

            yOffset += 26;
            generalBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 40, 16), "Left: "));
            TextBox l1 = new TextBox(new Rectangle(60, yOffset, 50, 24));
            l1.NumbersOnly = true;
            l1.Text = control.Bounds.X.ToString();
            l1.OnSubmit += (s) => { control.Bounds.X = Int32.Parse((s as TextBox).Text); };
            generalBox.Controls.Add(l1);
            generalBox.Controls.Add(new Label(new Rectangle(130, yOffset + 2, 40, 16), "Top: "));
            TextBox l2 = new TextBox(new Rectangle(180, yOffset, 50, 24));
            l2.NumbersOnly = true;
            l2.Text = control.Bounds.Y.ToString();
            l2.OnSubmit += (s) => { control.Bounds.Y = Int32.Parse((s as TextBox).Text); };
            generalBox.Controls.Add(l2);

            yOffset += 26;
            generalBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 40, 16), "Width: "));
            TextBox l3 = new TextBox(new Rectangle(60, yOffset, 50, 24));
            l3.NumbersOnly = true;
            l3.Text = control.Bounds.Width.ToString();
            l3.OnSubmit += (s) => { control.Bounds.Width = Int32.Parse((s as TextBox).Text); };
            generalBox.Controls.Add(l3);
            generalBox.Controls.Add(new Label(new Rectangle(130, yOffset + 2, 40, 16), "Height: "));
            TextBox l4 = new TextBox(new Rectangle(180, yOffset, 50, 24));
            l4.NumbersOnly = true;
            l4.Text = control.Bounds.Height.ToString();
            l4.OnSubmit += (s) => { control.Bounds.Height = Int32.Parse((s as TextBox).Text); };
            generalBox.Controls.Add(l4);

            yOffset += 30;
            generalBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 60, 16), "Name: "));
            TextBox tName = new TextBox(new Rectangle(80, yOffset, 150, 24));
            tName.Text = control.Name;
            tName.OnSubmit += (s) => { control.Name = (s as TextBox).Text; };
            generalBox.Controls.Add(tName);

            yOffset += 30;
            generalBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 60, 16), "Parent: "));
            TextBox tParent = new TextBox(new Rectangle(80, yOffset, 150, 24));
            tParent.Enabled = false;
            tParent.Text = (control.Parent == null) ? "" :
                (control.Parent.ToString().Contains(".") ? control.Parent.ToString().Remove(0, control.Parent.ToString().LastIndexOf(".") + 1) : control.Parent.ToString());
            generalBox.Controls.Add(tParent);

            yOffset += 30;
            generalBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 60, 16), "Hint: "));
            TextBox tHint = new TextBox(new Rectangle(80, yOffset, 150, 24));
            tHint.Text = control.Hint;
            tHint.OnSubmit += (s) => { control.Hint = (s as TextBox).Text; };
            generalBox.Controls.Add(tHint);

            yOffset += 30;
            generalBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 60, 16), "Opaque: "));
            Progress pTrans = new Progress(new Rectangle(80, yOffset + 4, 150, 16));
            pTrans.Clickable = true;
            pTrans.Value = control.Transparency;
            pTrans.OnProgressChanged += (s) => { control.Transparency = (s as Progress).Value; };
            generalBox.Controls.Add(pTrans);

            generalBox.Bounds.Height = yOffset + 40;
            yOffset += 80;

            GroupBox objBox = new GroupBox(new Rectangle(10, yOffset, 240, 300), (control == null) ? "" :
                (control.ToString().Contains(".") ? control.ToString().Remove(0, control.ToString().LastIndexOf(".") + 1) : control.ToString()));
            nWindow.Controls.Add(objBox);

            yOffset = -12;

            if (control is Button)
            {
                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 60, 16), "Text: "));
                TextBox tText = new TextBox(new Rectangle(80, yOffset, 150, 24));
                tText.Text = (control as Button).Text;
                tText.OnSubmit += (s) => { (control as Button).Text = (s as TextBox).Text; };
                objBox.Controls.Add(tText);

                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 0, 0), "Text Color:"));
                Image cl1 = new Image(new Rectangle(160, yOffset, 24, 24));
                cl1.Texture = CreateSquare(20, (control as Button).TextColor);
                cl1.OnClick += (s) =>
                { (new ColorDialog(new Point(600, 400), guiManager, (control as Button).TextColor, new ColorDialog.OKClickEventHandler((color) => {
                    (control as Button).TextColor = color; ShowGeneralPropertiesWindow(control); }))).Show(); };
                objBox.Controls.Add(cl1);
                
                yOffset += 30;
                CheckBox cpBoxA = new CheckBox(new Rectangle(10, yOffset, 200, 16), "AutoSize");
                cpBoxA.Checked = (control as Button).AutoSize;
                cpBoxA.OnClick += (s) => { (control as Button).AutoSize = (s as CheckBox).Checked; };
                objBox.Controls.Add(cpBoxA);
            }

            if (control is Chart)
            {   
                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 60, 16), "Title:"));
                TextBox tText = new TextBox(new Rectangle(80, yOffset, 150, 24));
                tText.Text = (control as Chart).Title;
                tText.OnSubmit += (s) => { (control as Chart).Title = (s as TextBox).Text; };
                objBox.Controls.Add(tText);

                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 0, 0), "Text Color:"));
                Image cl1 = new Image(new Rectangle(160, yOffset, 24, 24));
                cl1.Texture = CreateSquare(20, (control as Chart).TextColor);
                cl1.OnClick += (s) =>
                { (new ColorDialog(new Point(600, 400), guiManager, (control as Chart).TextColor, new ColorDialog.OKClickEventHandler((color) => {
                    (control as Chart).TextColor = color; ShowGeneralPropertiesWindow(control); }))).Show(); };
                objBox.Controls.Add(cl1);

                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 100, 16), "Chart Margin:"));
                TextBox tText3 = new TextBox(new Rectangle(120, yOffset, 50, 24));
                tText3.Text = (control as Chart).ChartMargin.ToString();
                tText3.NumbersOnly = true;
                tText3.OnSubmit += (s) => { (control as Chart).ChartMargin = Int32.Parse((s as TextBox).Text); };
                objBox.Controls.Add(tText3);

                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 100, 16), "Chart Line Width:"));
                TextBox tText9 = new TextBox(new Rectangle(120, yOffset, 50, 24));
                tText9.Text = (control as Chart).ChartLineWidth.ToString();
                tText9.NumbersOnly = true;
                tText9.OnSubmit += (s) => { (control as Chart).ChartLineWidth = Int32.Parse((s as TextBox).Text); };
                objBox.Controls.Add(tText9);

                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 100, 16), "Axis Line Width:"));
                TextBox tText2 = new TextBox(new Rectangle(120, yOffset, 50, 24));
                tText2.Text = (control as Chart).AxisLineWidth.ToString();
                tText2.NumbersOnly = true;
                tText2.OnSubmit += (s) => { (control as Chart).AxisLineWidth = Int32.Parse((s as TextBox).Text); };
                objBox.Controls.Add(tText2);

                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 100, 16), "Grid Spacing:"));
                TextBox tText4 = new TextBox(new Rectangle(120, yOffset, 50, 24));
                tText4.Text = (control as Chart).GridSpacing.ToString();
                tText4.NumbersOnly = true;
                tText4.OnSubmit += (s) => { (control as Chart).GridSpacing = Int32.Parse((s as TextBox).Text); };
                objBox.Controls.Add(tText4);

                yOffset += 30;
                CheckBox cpBox0 = new CheckBox(new Rectangle(10, yOffset, 200, 16), "Show X-Axis Min/Max Values");
                cpBox0.Checked = (control as Chart).ShowXAxisRange;
                cpBox0.OnClick += (s) => { (control as Chart).ShowXAxisRange = (s as CheckBox).Checked; };
                objBox.Controls.Add(cpBox0);

                yOffset += 24;
                CheckBox cpBox1 = new CheckBox(new Rectangle(10, yOffset, 200, 16), "Show Y-Axis Min/Max Values");
                cpBox1.Checked = (control as Chart).ShowYAxisRange;
                cpBox1.OnClick += (s) => { (control as Chart).ShowYAxisRange = (s as CheckBox).Checked; };
                objBox.Controls.Add(cpBox1);

                yOffset += 24;
                CheckBox cpBox2 = new CheckBox(new Rectangle(10, yOffset, 200, 16), "Show Grid");
                cpBox2.Checked = (control as Chart).ShowGrid;
                cpBox2.OnClick += (s) => { (control as Chart).ShowGrid = (s as CheckBox).Checked; };
                objBox.Controls.Add(cpBox2);

                yOffset += 24;
                CheckBox cpBox3 = new CheckBox(new Rectangle(10, yOffset, 200, 16), "Show Grid Values");
                cpBox3.Checked = (control as Chart).ShowGridValues;
                cpBox3.OnClick += (s) => { (control as Chart).ShowGridValues = (s as CheckBox).Checked; };
                objBox.Controls.Add(cpBox3);

                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 60, 16), "Add Chart:"));
                TextBox tTextChart = new TextBox(new Rectangle(80, yOffset, 150, 24));
                tTextChart.Text = String.Empty;
                tTextChart.OnSubmit += (s) =>
                {
                    string str = (s as TextBox).Text.Replace(" ", "");
                    string[] sp = str.Split(',');
                    List<float> chart = new List<float>();
                    foreach (string data in sp)
                        chart.Add(float.Parse(data));

                    (control as Chart).AddChart(chart, "Chart0", Color.CornflowerBlue);
                };
                objBox.Controls.Add(tTextChart);

                yOffset += 30;
                Button bList = new Button(new Rectangle(80, yOffset, 150, 24));
                bList.Text = "Remove The Last Chart";
                bList.OnClick += (s) =>
                {
                    (control as Chart).RemoveChart("Chart0");
                };
                objBox.Controls.Add(bList);
            }

            if (control is GroupBox)
            {
                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 60, 16), "Title: "));
                TextBox tText = new TextBox(new Rectangle(80, yOffset, 150, 24));
                tText.Text = (control as GroupBox).Title;
                tText.OnSubmit += (s) => { (control as GroupBox).Title = (s as TextBox).Text; };
                objBox.Controls.Add(tText);

                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 0, 0), "Title Color"));
                Image cl1 = new Image(new Rectangle(140, yOffset, 24, 24));
                cl1.Texture = CreateSquare(20, (control as GroupBox).TitleColor);
                cl1.OnClick += (s) =>
                { (new ColorDialog(new Point(600, 400), guiManager, (control as GroupBox).TitleColor, new ColorDialog.OKClickEventHandler((color) => {
                    (control as GroupBox).TitleColor = color; ShowGeneralPropertiesWindow(control); }))).Show(); };
                objBox.Controls.Add(cl1);
            }

            if (control is CheckBox)
            {
                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 60, 16), "Text: "));
                TextBox tText = new TextBox(new Rectangle(80, yOffset, 150, 24));
                tText.Text = (control as CheckBox).Text;
                tText.OnSubmit += (s) => { (control as CheckBox).Text = (s as TextBox).Text; };
                objBox.Controls.Add(tText);

                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 0, 0), "Text Color:"));
                Image cl1 = new Image(new Rectangle(160, yOffset, 24, 24));
                cl1.Texture = CreateSquare(20, (control as CheckBox).TextColor);
                cl1.OnClick += (s) =>
                { (new ColorDialog(new Point(600, 400), guiManager, (control as CheckBox).TextColor, new ColorDialog.OKClickEventHandler((color) => {
                    (control as CheckBox).TextColor = color; ShowGeneralPropertiesWindow(control); }))).Show(); };
                objBox.Controls.Add(cl1);

                yOffset += 30;
                CheckBox cpBox = new CheckBox(new Rectangle(10, yOffset, 120, 16), "Auto Check");
                cpBox.Checked = (control as CheckBox).AutoCheck;
                cpBox.OnClick += (s) => { (control as CheckBox).AutoCheck = (s as CheckBox).Checked; };
                objBox.Controls.Add(cpBox);

                yOffset += 24;
                CheckBox cpBoxA = new CheckBox(new Rectangle(10, yOffset, 200, 16), "AutoSize");
                cpBoxA.Checked = (control as CheckBox).AutoSize;
                cpBoxA.OnClick += (s) => { (control as CheckBox).AutoSize = (s as CheckBox).Checked; };
                objBox.Controls.Add(cpBoxA);
            }

            if (control is Image)
            {
            }

            if (control is Label)
            {
                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 60, 16), "Text: "));
                TextBox tText = new TextBox(new Rectangle(80, yOffset, 150, 24));
                tText.Text = (control as Label).Text;
                tText.OnSubmit += (s) => { (control as Label).Text = (s as TextBox).Text; };
                objBox.Controls.Add(tText);

                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 0, 0), "Text Color:"));
                Image cl1 = new Image(new Rectangle(160, yOffset, 24, 24));
                cl1.Texture = CreateSquare(20, (control as Label).TextColor);
                cl1.OnClick += (s) =>
                { (new ColorDialog(new Point(600, 400), guiManager, (control as Label).TextColor, new ColorDialog.OKClickEventHandler((color) => { 
                    (control as Label).TextColor = color; ShowGeneralPropertiesWindow(control); }))).Show(); };
                objBox.Controls.Add(cl1);
            }

            if (control is ListBox)
            {
                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 0, 0), "Item Color:"));
                Image cl1 = new Image(new Rectangle(160, yOffset, 24, 24));
                cl1.Texture = CreateSquare(20, (control as ListBox).ItemColor);
                cl1.OnClick += (s) =>
                { (new ColorDialog(new Point(600, 400), guiManager, (control as ListBox).ItemColor, new ColorDialog.OKClickEventHandler((color) => { 
                    (control as ListBox).ItemColor = color; ShowGeneralPropertiesWindow(control); }))).Show(); };
                objBox.Controls.Add(cl1);

                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 0, 0), "Selected Color:"));
                Image cl2 = new Image(new Rectangle(160, yOffset, 24, 24));
                cl2.Texture = CreateSquare(20, (control as ListBox).SelectedItemColor);
                cl2.OnClick += (s) =>
                { (new ColorDialog(new Point(600, 400), guiManager, (control as ListBox).SelectedItemColor, new ColorDialog.OKClickEventHandler((color) => { 
                    (control as ListBox).SelectedItemColor = color; ShowGeneralPropertiesWindow(control); }))).Show(); };
                objBox.Controls.Add(cl2);

                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 60, 16), "Add Item: "));
                TextBox tText = new TextBox(new Rectangle(80, yOffset, 150, 24));
                tText.OnSubmit += (s) => { (control as ListBox).Items.Add((s as TextBox).Text); (s as TextBox).Text = String.Empty; };
                objBox.Controls.Add(tText);

                yOffset += 30;
                Button bList = new Button(new Rectangle(80, yOffset, 150, 24));
                bList.Text = "Remove Selected Item";
                bList.OnClick += (s) =>
                {
                    ListBox x = (control as ListBox);
                    if (x.SelectedItems.Count <= 0)
                        return;
                    x.Items.RemoveAt(x.SelectedItems[0]);
                };
                objBox.Controls.Add(bList);

                yOffset += 30;
                CheckBox cpBox = new CheckBox(new Rectangle(10, yOffset, 120, 16), "Multiple Selection");
                cpBox.Checked = (control as ListBox).MultipleSelection;
                cpBox.OnClick += (s) => { (control as ListBox).MultipleSelection = (s as CheckBox).Checked; };
                objBox.Controls.Add(cpBox);
            }


            if (control is Progress)
            {
                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 60, 16), "Value:"));
                TextBox tText = new TextBox(new Rectangle(80, yOffset, 150, 24));
                tText.Text = (control as Progress).Value.ToString();
                tText.OnSubmit += (s) => { (control as Progress).Value = float.Parse((s as TextBox).Text); };
                objBox.Controls.Add(tText);

                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 60, 16), "Min Value:"));
                TextBox tText0 = new TextBox(new Rectangle(80, yOffset, 150, 24));
                tText0.Text = (control as Progress).MinValue.ToString();
                tText0.OnSubmit += (s) => { (control as Progress).MinValue = float.Parse((s as TextBox).Text); };
                objBox.Controls.Add(tText0);

                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 60, 16), "Min Value:"));
                TextBox tText1 = new TextBox(new Rectangle(80, yOffset, 150, 24));
                tText1.Text = (control as Progress).MaxValue.ToString();
                tText1.OnSubmit += (s) => { (control as Progress).MaxValue = float.Parse((s as TextBox).Text); };
                objBox.Controls.Add(tText1);

                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 0, 0), "Text Color:"));
                Image cl1 = new Image(new Rectangle(160, yOffset, 24, 24));
                cl1.Texture = CreateSquare(20, (control as Progress).TextColor);
                cl1.OnClick += (s) =>
                { (new ColorDialog(new Point(600, 400), guiManager, (control as Progress).TextColor, new ColorDialog.OKClickEventHandler((color) => {
                    (control as Progress).TextColor = color; ShowGeneralPropertiesWindow(control);}))).Show(); };
                objBox.Controls.Add(cl1);

                yOffset += 30;
                CheckBox cpBox = new CheckBox(new Rectangle(10, yOffset, 120, 16), "Clickable");
                cpBox.Checked = (control as Progress).Clickable;
                cpBox.OnClick += (s) => { (control as Progress).Clickable = (s as CheckBox).Checked; };
                objBox.Controls.Add(cpBox);

                yOffset += 24;
                CheckBox svBox = new CheckBox(new Rectangle(10, yOffset, 120, 16), "Display Progress");
                svBox.Checked = (control as Progress).DisplayProgress;
                svBox.OnClick += (s) => { (control as Progress).DisplayProgress = (s as CheckBox).Checked; };
                objBox.Controls.Add(svBox);

                yOffset += 24;
                CheckBox svBox0 = new CheckBox(new Rectangle(10, yOffset, 120, 16), "Show Percentage");
                svBox0.Checked = (control as Progress).ShowPercentage;
                svBox0.OnClick += (s) => { (control as Progress).ShowPercentage = (s as CheckBox).Checked; };
                objBox.Controls.Add(svBox0);
            }

            if (control is RadioButton)
            {
                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 60, 16), "Text: "));
                TextBox tText = new TextBox(new Rectangle(80, yOffset, 150, 24));
                tText.Text = (control as RadioButton).Text;
                tText.OnSubmit += (s) => { (control as RadioButton).Text = (s as TextBox).Text; };
                objBox.Controls.Add(tText);

                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 0, 0), "Text Color:"));
                Image cl1 = new Image(new Rectangle(160, yOffset, 24, 24));
                cl1.Texture = CreateSquare(20, (control as RadioButton).TextColor);
                cl1.OnClick += (s) => { (new ColorDialog(new Point(600, 400), guiManager, (control as RadioButton).TextColor, new ColorDialog.OKClickEventHandler((color) => {
                    (control as RadioButton).TextColor = color; ShowGeneralPropertiesWindow(control); }))).Show(); };
                objBox.Controls.Add(cl1);

                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 60, 16), "Group: "));
                TextBox tGroup = new TextBox(new Rectangle(80, yOffset, 150, 24));
                tGroup.NumbersOnly = true;
                tGroup.Text = (control as RadioButton).Group.ToString();
                tGroup.OnSubmit += (s) => { (control as RadioButton).Group = Int32.Parse((s as TextBox).Text); };
                objBox.Controls.Add(tGroup);

                yOffset += 30;
                CheckBox cpBox = new CheckBox(new Rectangle(10, yOffset, 120, 16), "Auto Check");
                cpBox.Checked = (control as RadioButton).AutoCheck;
                cpBox.OnClick += (s) => { (control as RadioButton).AutoCheck = (s as CheckBox).Checked; };
                objBox.Controls.Add(cpBox);

                yOffset += 24;
                CheckBox cpBoxA = new CheckBox(new Rectangle(10, yOffset, 200, 16), "AutoSize");
                cpBoxA.Checked = (control as RadioButton).AutoSize;
                cpBoxA.OnClick += (s) => { (control as RadioButton).AutoSize = (s as CheckBox).Checked; };
                objBox.Controls.Add(cpBoxA);
            }

            // todo: tabcontrol
            if (control is TabsContainer)
            {
                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 60, 16), "Tab Text: "));
                TextBox tText0 = new TextBox(new Rectangle(80, yOffset, 150, 24));
                tText0.Text = ((control as TabsContainer).Controls[(control as TabsContainer).CurrentTab] as TabControl).Text;
                tText0.OnSubmit += (s) =>
                {
                    TabControl tb = (control as TabsContainer).Controls[(control as TabsContainer).CurrentTab] as TabControl;
                    tb.Text = (s as TextBox).Text;
                };
                objBox.Controls.Add(tText0);

                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 60, 16), "Add Tab: "));
                TextBox tText = new TextBox(new Rectangle(80, yOffset, 150, 24));
                tText.OnSubmit += (s) =>
                {
                    TabControl tb = new TabControl();
                    tb.Text = (s as TextBox).Text;
                    AssignEvents(tb);
                    control.Controls.Add(tb);
                    (s as TextBox).Text = String.Empty;
                };
                objBox.Controls.Add(tText);

                yOffset += 30;
                Button bList = new Button(new Rectangle(80, yOffset, 150, 24));
                bList.Text = "Remove Selected Tab";
                bList.OnClick += (s) =>
                {
                    TabsContainer x = (control as TabsContainer);
                    if (x.Controls.Count <= 1)
                        return;

                    x.Controls.RemoveAt(x.CurrentTab);
                    x.CurrentTab = 0;
                };
                objBox.Controls.Add(bList);
            }

            if (control is TextArea)
            {
                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 60, 16), "Text: "));
                TextBox tText = new TextBox(new Rectangle(80, yOffset, 150, 24));
                tText.Text = (control as TextArea).Text;
                tText.OnSubmit += (s) => { (control as TextArea).Text = (s as TextBox).Text; };
                objBox.Controls.Add(tText);

                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 0, 0), "Text Color:"));
                Image cl1 = new Image(new Rectangle(160, yOffset, 24, 24));
                cl1.Texture = CreateSquare(20, (control as TextArea).TextColor);
                cl1.OnClick += (s) => { (new ColorDialog(new Point(600, 400), guiManager, (control as TextArea).TextColor, new ColorDialog.OKClickEventHandler((color) => {
                    (control as TextArea).TextColor = color; ShowGeneralPropertiesWindow(control); }))).Show(); };
                objBox.Controls.Add(cl1);

                yOffset += 30;
                CheckBox cpBox = new CheckBox(new Rectangle(10, yOffset, 120, 16), "Word Wrap");
                cpBox.Checked = (control as TextArea).WordWrap;
                cpBox.OnClick += (s) => { (control as TextArea).WordWrap = (s as CheckBox).Checked; };
                objBox.Controls.Add(cpBox);

                yOffset += 30;
                CheckBox cpBox0 = new CheckBox(new Rectangle(10, yOffset, 120, 16), "Colorize");
                cpBox0.Checked = (control as TextArea).Colorize;
                cpBox0.OnClick += (s) => { (control as TextArea).Colorize = (s as CheckBox).Checked; };
                objBox.Controls.Add(cpBox0);

                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 60, 16), "Add Color:"));
                TextBox tText0 = new TextBox(new Rectangle(80, yOffset, 30, 24));
                tText0.OnSubmit += (s) =>
                {
                    if (tText0.Text.Length <= 0)
                        return;

                    (new ColorDialog(new Point(600, 400), guiManager, (control as TextArea).TextColor, new ColorDialog.OKClickEventHandler((color) =>
                    {   
                        (control as TextArea).ColorTable.Add(tText0.Text[0], color);
                        tText0.Text = String.Empty;
                    }))).Show();
                };
                objBox.Controls.Add(tText0);
                yOffset += 30;
                Button bList = new Button(new Rectangle(80, yOffset, 150, 24));
                bList.Text = "Clear Color Table";
                bList.OnClick += (s) =>
                    {
                        (control as TextArea).ColorTable.Clear();
                    };
                objBox.Controls.Add(bList);
            }

            if (control is TextBox)
            {
                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 60, 16), "Text: "));
                TextBox tText = new TextBox(new Rectangle(80, yOffset, 150, 24));
                tText.Text = (control as TextBox).Text;
                tText.OnSubmit += (s) => { (control as TextBox).Text = (s as TextBox).Text; };
                objBox.Controls.Add(tText);

                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 0, 0), "Text Color:"));
                Image cl1 = new Image(new Rectangle(160, yOffset, 24, 24));
                cl1.Texture = CreateSquare(20, (control as TextBox).TextColor);
                cl1.OnClick += (s) => { (new ColorDialog(new Point(600, 400), guiManager, (control as TextBox).TextColor, new ColorDialog.OKClickEventHandler((color) => {
                        (control as TextBox).TextColor = color; ShowGeneralPropertiesWindow(control); }))).Show(); };
                objBox.Controls.Add(cl1);

                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 60, 16), "Cursor Char: "));
                TextBox tText0 = new TextBox(new Rectangle(160, yOffset, 50, 24));
                tText0.Text = (control as TextBox).CursorChar;
                tText0.OnSubmit += (s) => { (control as TextBox).CursorChar = (s as TextBox).Text; };
                objBox.Controls.Add(tText0);

                yOffset += 30;
                CheckBox cpBox = new CheckBox(new Rectangle(10, yOffset, 120, 16), "Numbers Only");
                cpBox.Checked = (control as TextBox).NumbersOnly;
                cpBox.OnClick += (s) => { (control as TextBox).NumbersOnly = (s as CheckBox).Checked; };
                objBox.Controls.Add(cpBox);

                yOffset += 24;
                CheckBox cpBox2 = new CheckBox(new Rectangle(10, yOffset, 120, 16), "Alpha-Numeric Only");
                cpBox2.Checked = (control as TextBox).NumbersOnly;
                cpBox2.OnClick += (s) => { (control as TextBox).NumbersOnly = (s as CheckBox).Checked; };
                objBox.Controls.Add(cpBox2);

                yOffset += 24;
                CheckBox cpBox3 = new CheckBox(new Rectangle(10, yOffset, 120, 16), "Password Field");
                cpBox3.Checked = (control as TextBox).PasswordField;
                cpBox3.OnClick += (s) => { (control as TextBox).PasswordField = (s as CheckBox).Checked; };
                objBox.Controls.Add(cpBox3);

                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 60, 16), "Password Mask: "));
                TextBox tText2 = new TextBox(new Rectangle(130, yOffset, 40, 24));
                tText2.Text = (control as TextBox).PasswordMask;
                tText2.OnSubmit += (s) => { (control as TextBox).PasswordMask = (s as TextBox).Text; };
                objBox.Controls.Add(tText2);
            }

            if (control is Window)
            {
                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 60, 16), "Title: "));
                TextBox tText = new TextBox(new Rectangle(80, yOffset, 150, 24));
                tText.Text = (control as Window).Title;
                tText.OnSubmit += (s) => { (control as Window).Title = (s as TextBox).Text; };
                objBox.Controls.Add(tText);

                yOffset += 30;
                objBox.Controls.Add(new Label(new Rectangle(10, yOffset + 2, 0, 0), "Title Color:"));
                Image cl1 = new Image(new Rectangle(160, yOffset, 24, 24));
                cl1.Texture = CreateSquare(20, (control as Window).TitleColor);
                cl1.OnClick += (s) => { (new ColorDialog(new Point(600, 400), guiManager, (control as Window).TitleColor, new ColorDialog.OKClickEventHandler((color) => {
                        (control as Window).TitleColor = color; ShowGeneralPropertiesWindow(control); }))).Show(); };
                objBox.Controls.Add(cl1);

                yOffset += 30;
                CheckBox cpBox = new CheckBox(new Rectangle(10, yOffset, 120, 16), "Movable");
                cpBox.Checked = (control as Window).Movable;
                cpBox.OnClick += (s) => { (control as Window).Movable = (s as CheckBox).Checked; };
                objBox.Controls.Add(cpBox);
            }

            objBox.Bounds.Height = yOffset + 40;

            nWindow.Bounds.Height = objBox.Bounds.Y + objBox.Bounds.Height + 16;
        }

        void AddControl(string name)
        {
            // if no window is open
            GUIControl parent = targetWindow;
            if (parent == null)
                return;

            GUIControl control = null;
            GUIControl subControl = null;

            switch (name)
            {
                case "Button":
                    control = new Button(Rectangle.Empty, "Button Text");
                    break;

                case "Chart":
                    control = new Chart(new Rectangle(0, 0, 360, 260));
                    break;

                case "GroupBox":
                    control = new GroupBox(new Rectangle(0, 0, 150, 200), "GroupBox Title");
                    break;

                case "CheckBox":
                    control = new CheckBox(Rectangle.Empty, "CheckBox Text");
                    break;

                case "Image":
                    control = new Image(new Rectangle(0, 0, 64, 64));
                    (control as Image).Texture = imageTexture;
                    break;

                case "Label":
                    control = new Label(Rectangle.Empty, "Label Text");
                    break;

                case "ListBox":
                    control = new ListBox(new Rectangle(0, 0, 150, 200));
                    break;

                case "Progress":
                    control = new Progress(new Rectangle(0, 0, 100, 16));
                    break;

                case "RadioButton":
                    control = new RadioButton(Rectangle.Empty, "RadioButton Text");
                    break;

                case "TabsContainer":
                    control = new TabsContainer(new Rectangle(0, 0, 300, 250));
                    TabControl tabControl = new TabControl();
                    tabControl.Text = "First Tab";
                    subControl = tabControl;
                    control.Controls.Add(tabControl);
                    break;

                case "TextArea":
                    control = new TextArea(new Rectangle(0, 0, 150, 100));
                    break;

                case "TextBox":
                    control = new TextBox(new Rectangle(0, 0, 100, 24));
                    break;
            }

            if (subControl == null)
                AssignEvents(control);
            else // used for TabsContainer
                AssignEvents(subControl);

            controlToAdd = control;
        }

        void AssignEvents(GUIControl control)
        {
            control.OnMousePressed += (s, e) =>
            {
                if (s is TabControl)
                    s = (s as TabControl).Parent;

                activeControl = movingControl = s;
            };

            control.OnClick += (s) =>
            {
                if (s is TabControl)
                    s = (s as TabControl).Parent;

                ShowGeneralPropertiesWindow(s);
            };
        }

        private Texture2D CreateSquare(int size, Color bgColor)
        {
            Color[] data = new Color[size * size];

            Texture2D tmp = new Texture2D(GraphicsDevice, size, size, false, SurfaceFormat.Color);

            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                    if (x == 0 || x == size - 1 || y == 0 || y == size - 1)
                        data[y * size + x] = new Color(36, 36, 36, 255);
                    else
                        data[y * size + x] = bgColor;
            tmp.SetData<Color>(data);
            return tmp;
        }
    }
}
