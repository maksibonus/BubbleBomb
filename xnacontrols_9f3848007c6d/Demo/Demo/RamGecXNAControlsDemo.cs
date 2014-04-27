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
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.ComponentModel;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Design;
using RamGecXNAControls.ExtendedControls;


namespace Demo
{
    public class RamGecXNAControlsDemo : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GUIManager guiManager;

        Texture2D userTexture;

        public RamGecXNAControlsDemo()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();


            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            userTexture = Content.Load<Texture2D>("user");

            guiManager = new GUIManager(this, "Themes", "Default");

            Window topWindow = new Window(new Rectangle(10, 10, 350, 160), "Demo");
            guiManager.Controls.Add(topWindow);

            GroupBox groupBox1 = new GroupBox(new Rectangle(10, 30, 150, 120), "Windows");
            topWindow.Controls.Add(groupBox1);

            Button demoWindow1Button = new Button(new Rectangle(10, 20, 130, 24), "Demo Window 1");
            groupBox1.Controls.Add(demoWindow1Button);

            //Button demoWindow2Button = new Button(new Rectangle(10, 50, 130, 24), "Demo Window 2");
            //groupBox1.Controls.Add(demoWindow2Button);

            Button testWindowButton = new Button(new Rectangle(10, 50, 130, 24), "Test Window");
            groupBox1.Controls.Add(testWindowButton);

            GroupBox groupBox2 = new GroupBox(new Rectangle(170, 30, 170, 120), "Available Themes");
            topWindow.Controls.Add(groupBox2);


            ListBox themesList = new ListBox(new Rectangle(10, 20, 150, 90));
            groupBox2.Controls.Add(themesList);
            string[] themes = System.IO.Directory.GetDirectories(Environment.CurrentDirectory + "\\Themes");

            foreach (string themeName in themes)
                themesList.Items.Add(System.IO.Path.GetFileName(themeName));

            themesList.OnSelectItem += (s, i) =>
            {
                guiManager.LoadTheme("Themes", (s as ListBox).SelectedString);
            };


            demoWindow1Button.OnClick += (s) =>
                {
                    if (guiManager.GetControl("Demo Window") != null)
                        guiManager.Controls.Remove(guiManager.GetControl("Demo Window"));

                    guiManager.LoadControls("Content\\demo_window.xml");
                    (guiManager.GetControl("UserImage") as Image).Texture = userTexture;
                    (guiManager.GetControl("OKButton") as Button).Icon = guiManager.Theme.IconYes;
                    (guiManager.GetControl("CloseButton") as Button).Icon = guiManager.Theme.IconNo;
                    (guiManager.GetControl("OKButton") as Button).OnClick += (sender) => { guiManager.Controls.Remove(guiManager.GetControl("Demo Window")); };
                    (guiManager.GetControl("CloseButton") as Button).OnClick += (sender) => { guiManager.Controls.Remove(guiManager.GetControl("Demo Window")); };
                };

            testWindowButton.OnClick += (s) =>
                {
                    if (guiManager.GetControl("Test Window") != null)
                        guiManager.Controls.Remove(guiManager.GetControl("Test Window"));

                    guiManager.LoadControls("Content\\test_window.xml");
                    (guiManager.GetControl("Button1") as Button).Icon = guiManager.Theme.IconYes;
                    (guiManager.GetControl("Button2") as Button).Icon = guiManager.Theme.IconNo;
                    (guiManager.GetControl("Image1") as Image).Texture = guiManager.Theme.IconWarning;
                    (guiManager.GetControl("Image2") as Image).Texture = guiManager.Theme.IconQuestion;

                    (guiManager.GetControl("ImageList") as ListBox).ShowIcons = true;
                    (guiManager.GetControl("ImageList") as ListBox).Icons.AddRange(
                        new Texture2D[] { guiManager.Theme.IconFile, guiManager.Theme.IconFolder, guiManager.Theme.IconYes, guiManager.Theme.IconNo, guiManager.Theme.IconUp,
                        guiManager.Theme.IconSave, guiManager.Theme.IconStar, guiManager.Theme.IconWarning, guiManager.Theme.IconQuestion, guiManager.Theme.IconHome }
                        );

                    (guiManager.GetControl("Ex1") as Button).OnClick += (button) =>
                        {
                            FileDialog fd = new FileDialog(Point.Zero, guiManager, new FileDialog.SelectClickEventHandler((res) => {
                                (guiManager.GetControl("ExResult") as TextArea).Text += "> " + res + "\r\n";
                            }));
                            fd.Show();
                        };

                    (guiManager.GetControl("Ex2") as Button).OnClick += (button) =>
                    {
                        ColorDialog cd = new ColorDialog(Point.Zero, guiManager, new ColorDialog.OKClickEventHandler((res) =>
                        {
                            (guiManager.GetControl("ExResult") as TextArea).Text += "> " + res.ToString() + "\r\n";
                        }));
                        cd.Show();
                    };

                    (guiManager.GetControl("Ex3") as Button).OnClick += (button) =>
                    {
                        MessageBox mb = new MessageBox(Point.Zero, guiManager, "Message Text", "Message", new MessageBox.OKClickEventHandler(() =>
                        {
                            (guiManager.GetControl("ExResult") as TextArea).Text += "> OK Clicked\r\n";
                        }));
                        mb.Show();
                    };

                    (guiManager.GetControl("Ex4") as Button).OnClick += (button) =>
                    {
                        FileDialog fd = new FileDialog(Point.Zero, guiManager, new FileDialog.SelectClickEventHandler((res) =>
                        {
                            (guiManager.GetControl("ExResult") as TextArea).Text += "> " + res + "\r\n";
                        }));
                        fd.Filename = Environment.CurrentDirectory + "\\newfile.txt";
                        fd.FileFilter = "*.txt";
                        fd.Show();
                    };

                    (guiManager.GetControl("Ex5") as Button).OnClick += (button) =>
                    {
                        ColorDialog cd = new ColorDialog(Point.Zero, guiManager, new ColorDialog.OKClickEventHandler((res) =>
                        {
                            (guiManager.GetControl("ExResult") as TextArea).Text += "> " + res.ToString() + "\r\n";
                        }));
                        cd.ColorsList[1] = Color.OrangeRed;
                        cd.ColorsList[2] = Color.IndianRed;
                        cd.ColorsList[3] = Color.MediumVioletRed;
                        cd.ColorsList[4] = Color.PaleVioletRed;
                        cd.SelectedColor = Color.PaleTurquoise;
                        cd.Show();
                    };

                    (guiManager.GetControl("Ex6") as Button).OnClick += (button) =>
                    {
                        MessageBox mb = new MessageBox(Point.Zero, guiManager, "Message Text", "Message", new MessageBox.OKClickEventHandler(() =>
                        {   
                            (guiManager.GetControl("ExResult") as TextArea).Text += "> OK Clicked\r\n";
                        }));
                        mb.ButtonIcon = guiManager.Theme.IconStar;
                        mb.Title = "Message Title";
                        mb.Text = "This is a first line of the MessageBox dialog.\r\nThis is the second line.";
                        mb.Show();
                    };
                };
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            guiManager.Update(gameTime);
            
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {   
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            guiManager.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
