#region Copyright
/// <copyright>
/// Copyright (c) 2012 Ramunas Geciauskas, http://geciauskas.com
///
/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:
///
/// The above copyright notice and this permission notice shall be included in
/// all copies or substantial portions of the Software.
///
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
/// THE SOFTWARE.
/// </copyright>
/// <author>Ramunas Geciauskas</author>
/// <summary>ColorDialog Class</summary>
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using RamGecXNAControlsExtensions;
using System.IO;
using Microsoft.Xna.Framework.Input;

namespace RamGecXNAControls.ExtendedControls
{
    /// <summary>
    /// ColorDialog
    /// </summary>
    public class ColorDialog
    {
        #region Public Properties
        /// <summary>
        /// Sets or gets currently selected color
        /// </summary>
        public Color SelectedColor
        {
            set
            {
                currentColor = value;
                RGBtoHSV(currentColor.R, currentColor.G, currentColor.B, out colorHue, out colorSaturation, out colorValue);
                Update();
            }
            get
            {
                return currentColor;
            }
        }

        /// <summary>
        /// List of colors displayed on ColorDialog window. Max: 16 items
        /// </summary>
        public Color[] ColorsList = new Color[] {
            Color.White, Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.Cyan,
            Color.Black, Color.Gray, Color.DarkGray, Color.LightGray, Color.CornflowerBlue, Color.Magenta
        };
        #endregion

        #region Private Properties
        private GUIManager guiManager = null;
        private Window boxWindow = null;
        
        private Image colorImage = null;
        private Image hueImage = null;
        private Image colorCursor = null;
        private Image hueCursor = null;
        private Image resultColor = null;

        private double colorHue = 0;
        private double colorSaturation = 0;
        private double colorValue = 0;

        private TextBox hueBox = null;
        private TextBox saturationBox = null;
        private TextBox valueBox = null;
        private TextBox rBox = null;
        private TextBox gBox = null;
        private TextBox bBox = null;

        private Color currentColor;
        #endregion

        #region Events and Delegates
        /// <summary>
        /// Delegate for OnOKClick events
        /// </summary>
        /// <param name="selectedColor">Selected Color</param>
        public delegate void OKClickEventHandler(Color selectedColor);
        #endregion

        #region Constructors
        /// <summary>
        /// Creates and shows ColorDialog window
        /// </summary>
        /// <param name="location">Location of top-left corner of ColorDialog window. If Point.Zero - display in the middle of the screen</param>
        /// <param name="guiManager">GUIManager where this control should be drawed</param>
        /// <param name="selectedColor">Currently selected color</param>
        /// <param name="onOKClickCallback">>Callback method that is called when OK button is clicked</param>
        public ColorDialog(Point location, GUIManager guiManager, Color selectedColor, OKClickEventHandler onOKClickCallback)
        {
            this.guiManager = guiManager;

            boxWindow = new Window(Rectangle.Empty, "Color Dialog", "FileDialog Window");
            boxWindow.TopMost = true;

            // set window size
            boxWindow.Bounds.X = location.X;
            boxWindow.Bounds.Y = location.Y;
            boxWindow.Bounds.Width = 405;
            boxWindow.Bounds.Height = 285;

            // make it in the center of the screen
            if (location == Point.Zero)
            {
                boxWindow.Bounds.X = (guiManager.Game.GraphicsDevice.Viewport.Width / 2) - (boxWindow.Bounds.Width / 2);
                boxWindow.Bounds.Y = (guiManager.Game.GraphicsDevice.Viewport.Height / 2) - (boxWindow.Bounds.Height / 2);
            }

            // create Image controls
            colorImage = new Image(new Rectangle(10, 30, 200, 200));
            colorImage.Texture = new Texture2D(guiManager.Game.GraphicsDevice, colorImage.Bounds.Width, colorImage.Bounds.Height, false, SurfaceFormat.Color);
            boxWindow.Controls.Add(colorImage);
            colorImage.OnMouseMove += new GUIControl.MouseMoveEventHandler(colorSelect_OnMouseMove);

            hueImage = new Image(new Rectangle(220, 30, 20, 200));
            hueImage.Texture = new Texture2D(guiManager.Game.GraphicsDevice, hueImage.Bounds.Width, hueImage.Bounds.Height, false, SurfaceFormat.Color);
            boxWindow.Controls.Add(hueImage);
            hueImage.OnMouseMove += new GUIControl.MouseMoveEventHandler(hueBox_OnMouseMove);

            hueCursor = new Image(new Rectangle(218, 100, 24, 8));
            hueCursor.Texture = new Texture2D(guiManager.Game.GraphicsDevice, hueCursor.Bounds.Width, hueCursor.Bounds.Height, false, SurfaceFormat.Color);
            boxWindow.Controls.Add(hueCursor);

            colorCursor = new Image(new Rectangle(100, 100, 8, 8));
            colorCursor.Texture = new Texture2D(guiManager.Game.GraphicsDevice, colorCursor.Bounds.Width, colorCursor.Bounds.Height, false, SurfaceFormat.Color);
            boxWindow.Controls.Add(colorCursor);

            // create textures
            CreateCursors();

            // hsv boxes
            boxWindow.Controls.Add(new Label(new Rectangle(250, 150, 0, 0), "H: "));
            hueBox = new TextBox(new Rectangle(270, 148, 40, 24), "0");
            hueBox.NumbersOnly = true;
            hueBox.OnSubmit += (s) =>
                {
                    colorHue = (s as TextBox).Text.Length < 0 ? 0 : MathHelper.Clamp(float.Parse((s as TextBox).Text), 0f, 360f);
                    Update();
                };
            boxWindow.Controls.Add(hueBox);

            boxWindow.Controls.Add(new Label(new Rectangle(250, 180, 0, 0), "S: "));
            saturationBox = new TextBox(new Rectangle(270, 178, 40, 24), "0");
            saturationBox.OnSubmit += (s) =>
            {
                colorSaturation = (s as TextBox).Text.Length < 0 ? 0 : MathHelper.Clamp(float.Parse((s as TextBox).Text), 0f, 100f) / 100f;
                Update();
            };
            boxWindow.Controls.Add(saturationBox);

            boxWindow.Controls.Add(new Label(new Rectangle(250, 210, 0, 0), "V: "));
            valueBox = new TextBox(new Rectangle(270, 208, 40, 24), "0");
            valueBox.OnSubmit += (s) =>
            {
                colorValue = (s as TextBox).Text.Length < 0 ? 0 : MathHelper.Clamp(float.Parse((s as TextBox).Text), 0f, 100f) / 100f;
                Update();
            };
            boxWindow.Controls.Add(valueBox);

            // rgb boxes
            boxWindow.Controls.Add(new Label(new Rectangle(330, 150, 0, 0), "R: "));
            rBox = new TextBox(new Rectangle(350, 148, 40, 24), "0");
            rBox.OnSubmit += (s) =>
            {
                currentColor.R = (s as TextBox).Text.Length < 0 ? (byte)0 : (byte)MathHelper.Clamp(float.Parse((s as TextBox).Text), 0f, 255f);
                RGBtoHSV(currentColor.R, currentColor.G, currentColor.B, out colorHue, out colorSaturation, out colorValue);
                Update();
            };
            boxWindow.Controls.Add(rBox);

            boxWindow.Controls.Add(new Label(new Rectangle(330, 180, 0, 0), "G: "));
            gBox = new TextBox(new Rectangle(350, 178, 40, 24), "0");
            gBox.OnSubmit += (s) =>
            {
                currentColor.G = (s as TextBox).Text.Length <= 0 ? (byte)0 : (byte)MathHelper.Clamp(float.Parse((s as TextBox).Text), 0f, 255f);
                RGBtoHSV(currentColor.R, currentColor.G, currentColor.B, out colorHue, out colorSaturation, out colorValue);
                Update();
            };
            boxWindow.Controls.Add(gBox);

            boxWindow.Controls.Add(new Label(new Rectangle(330, 210, 0, 0), "B: "));
            bBox = new TextBox(new Rectangle(350, 208, 40, 24), "0");
            bBox.OnSubmit += (s) =>
            {
                currentColor.B = (s as TextBox).Text.Length <= 0 ? (byte)0 : (byte)MathHelper.Clamp(float.Parse((s as TextBox).Text), 0f, 255f);
                RGBtoHSV(currentColor.R, currentColor.G, currentColor.B, out colorHue, out colorSaturation, out colorValue);
                Update();
            };
            boxWindow.Controls.Add(bBox);

            // selected color
            boxWindow.Controls.Add(new Label(new Rectangle(250, 30, 0, 0), "Selected Color: "));
            resultColor = new Image(new Rectangle(350, 30, 40, 40));
            resultColor.Texture = new Texture2D(guiManager.Game.GraphicsDevice, resultColor.Bounds.Width, resultColor.Bounds.Height, false, SurfaceFormat.Color);
            boxWindow.Controls.Add(resultColor);

            CreateColorList();
            
            // buttons
            Button closeButton = new Button(new Rectangle(310, 250, 80, 24), "Cancel");
            closeButton.OnClick += (s) =>
            {
                Close();
            };
            boxWindow.Controls.Add(closeButton);

            Button okButton = new Button(new Rectangle(220, 250, 80, 24), "OK");
            okButton.OnClick += (s) =>
            {
                guiManager.Controls.Remove(boxWindow);
                if (onOKClickCallback != null)
                    onOKClickCallback(SelectedColor);
            };
            boxWindow.Controls.Add(okButton);

            
            SelectedColor = selectedColor;
        }

        /// <summary>
        /// Creates and shows ColorDialog window in the center of the screen
        /// </summary>
        /// <param name="guiManager">GUIManager where this control should be drawed</param>
        /// <param name="selectedColor">Currently selected color</param>
        /// <param name="onOKClickCallback">>Callback method that is called when OK button is clicked</param>
        public ColorDialog(GUIManager guiManager, Color selectedColor, OKClickEventHandler onOKClickCallback)
            : this(Point.Zero, guiManager, selectedColor, onOKClickCallback)
        {
        }

        /// <summary>
        /// Creates and shows ColorDialog window
        /// </summary>
        /// <param name="location">Location of top-left corner of ColorDialog window. If Point.Zero - display in the middle of the screen</param>
        /// <param name="guiManager">GUIManager where this control should be drawed</param>
        /// <param name="onOKClickCallback">>Callback method that is called when OK button is clicked</param>
        public ColorDialog(Point location, GUIManager guiManager, OKClickEventHandler onOKClickCallback)
            : this(Point.Zero, guiManager, Color.Black, onOKClickCallback)
        {
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Show Window (add control to GUIManager)
        /// </summary>
        public void Show()
        {
            CreateColorList();
            guiManager.Controls.Add(boxWindow);
        }

        /// <summary>
        /// Close Window (remove control from GUIManager)
        /// </summary>
        public void Close()
        {
            guiManager.Controls.Remove(boxWindow);
        }
        #endregion

        #region Private Methods
        void colorSelect_OnMouseMove(GUIControl sender, MouseState mouseState)
        {
            if (sender.IsMouseLeftDown)
            {
                colorSaturation = (mouseState.X - sender.AbsoluteBounds.X) / (double)(sender.Bounds.Width - 1);
                colorValue = 1f - ((mouseState.Y - sender.AbsoluteBounds.Y) / (double)(sender.Bounds.Height - 1));

                Update();
            }
        }

        void hueBox_OnMouseMove(GUIControl sender, MouseState mouseState)
        {
            if (sender.IsMouseLeftDown)
            {
                colorHue = (1f - ((mouseState.Y - sender.AbsoluteBounds.Y) / (double)(sender.Bounds.Height - 1))) * 360f;
                GenerateColorWheel();


                Update();
            }
        }

        /// <summary>
        /// Updates all controls to represent correct colors and values
        /// </summary>
        void Update()
        {
            currentColor = HsvToRgb(colorHue, colorSaturation, colorValue);

            GenerateColorWheel();
            GenerateHueWheel();

            UpdateCursors();

            GenerateSelectedColor();
            UpdateValues();
        }

        private void CreateColorList()
        {
            // colors list
            for (int colorIndex = 0; colorIndex < ColorsList.Length; colorIndex++)
            {
                if (boxWindow.GetControl("Color Image " + colorIndex.ToString()) != null)
                    boxWindow.Controls.Remove(boxWindow.GetControl("Color Image " + colorIndex.ToString()));

                Image img = new Image(new Rectangle(250 + ((colorIndex - (colorIndex > 5 ? 6 : 0)) * 24), 90 + (colorIndex > 5 ? 24 : 0), 20, 20));
                img.Name = "Color Image" + colorIndex.ToString();
                img.Texture = new Texture2D(guiManager.Game.GraphicsDevice, img.Bounds.Width, img.Bounds.Height, false, SurfaceFormat.Color);
                boxWindow.Controls.Add(img);

                Color[] data = new Color[img.Bounds.Width * img.Bounds.Height];
                for (int y = 0; y < img.Bounds.Height; y++)
                    for (int x = 0; x < img.Bounds.Width; x++)
                        if (x == 0 || x == img.Texture.Width - 1 || y == 0 || y == img.Texture.Height - 1)
                            data[y * img.Texture.Width + x] = new Color(0, 0, 0, 255);
                        else if (x == 1 || x == img.Texture.Width - 2 || y == 1 || y == img.Texture.Height - 2)
                            data[y * img.Texture.Width + x] = new Color(255, 255, 255, 255);
                        else
                            data[y * img.Bounds.Width + x] = ColorsList[colorIndex];

                img.Name = colorIndex.ToString();
                img.OnClick += (s) =>
                {
                    int index = Int32.Parse(img.Name);
                    RGBtoHSV(ColorsList[index].R, ColorsList[index].G, ColorsList[index].B,
                        out colorHue, out colorSaturation, out colorValue);
                    Update();
                };
                img.Texture.SetData<Color>(data);
            }
        }

        /// <summary>
        /// Displays cursors in Hue and Color images
        /// </summary>
        void UpdateCursors()
        {
            hueCursor.Bounds.Y = (int)(((1f - (colorHue / 360f)) * hueImage.Bounds.Height) + hueImage.Bounds.Y - (hueCursor.Bounds.Height / 2));

            colorCursor.Bounds.X = (int)((colorSaturation * colorImage.Bounds.Width) + colorImage.Bounds.X - (colorCursor.Bounds.Width / 2));
            colorCursor.Bounds.Y = (int)(((1f - colorValue) * colorImage.Bounds.Height) + colorImage.Bounds.Y - (colorCursor.Bounds.Height / 2));
        }

        /// <summary>
        /// Creates selected color
        /// </summary>
        private void GenerateSelectedColor()
        {
            Color color = HsvToRgb(colorHue, colorSaturation, colorValue);

            Color[] data = new Color[resultColor.Texture.Width * resultColor.Texture.Height];
            for (int y = 0; y < resultColor.Texture.Height; y++)
                for (int x = 0; x < resultColor.Texture.Width; x++)
                    if (x == 0 || x == resultColor.Texture.Width - 1 || y == 0 || y == resultColor.Texture.Height - 1)
                        data[y * resultColor.Texture.Width + x] = new Color(0, 0, 0, 255);
                    else if (x == 1 || x == resultColor.Texture.Width - 2 || y == 1 || y == resultColor.Texture.Height - 2)
                        data[y * resultColor.Texture.Width + x] = new Color(255, 255, 255, 255);
                    else
                        data[y * resultColor.Texture.Width + x] = color;
                    
            resultColor.Texture.SetData<Color>(data);
        }

        /// <summary>
        /// Creates Cursors' textures
        /// </summary>
        private void CreateCursors()
        {
            // hue
            Color[] data = new Color[hueCursor.Texture.Width * hueCursor.Texture.Height];
            for (int y = 0; y < hueCursor.Texture.Height; y++)
            {
                for (int x = 0; x < hueCursor.Texture.Width; x++)
                {
                    data[y * hueCursor.Texture.Width + x] = new Color(0, 0, 0, 0);

                    if (x <= 1 || x >= hueCursor.Texture.Width - 2 || y <= 1 || y >= hueCursor.Texture.Height - 2)
                    {
                        data[y * hueCursor.Texture.Width + x] = new Color(0, 0, 0, 255);
                    }

                }
            }
            hueCursor.Texture.SetData<Color>(data);

            // color
            data = new Color[colorCursor.Texture.Width * colorCursor.Texture.Height];
            for (int y = 0; y < colorCursor.Texture.Height; y++)
            {
                for (int x = 0; x < colorCursor.Texture.Width; x++)
                {
                    data[y * colorCursor.Texture.Width + x] = new Color(0, 0, 0, 0);

                    if (x == 0 || x == colorCursor.Texture.Width - 1 || y == 0 || y == colorCursor.Texture.Height - 1)
                        data[y * colorCursor.Texture.Width + x] = new Color(0, 0, 0, 255);
                    else if (x == 1 || x == colorCursor.Texture.Width - 2 || y == 1 || y == colorCursor.Texture.Height - 2)
                        data[y * colorCursor.Texture.Width + x] = new Color(255, 255, 255, 255);

                }
            }
            colorCursor.Texture.SetData<Color>(data);
        }

        /// <summary>
        /// Updates values of TextBox'es
        /// </summary>
        private void UpdateValues()
        {
            hueBox.Text = ((int)colorHue).ToString();
            saturationBox.Text = ((int)(colorSaturation * 100f)).ToString();
            valueBox.Text = ((int)(colorValue * 100f)).ToString();

            rBox.Text = currentColor.R.ToString();
            gBox.Text = currentColor.G.ToString();
            bBox.Text = currentColor.B.ToString();
        }

        /// <summary>
        /// Create Hue texture
        /// </summary>
        private void GenerateHueWheel()
        {
            double h = 0f;

            Color[] data = new Color[hueImage.Texture.Width * hueImage.Texture.Height];
            for (int y = 0; y < hueImage.Texture.Height; y++)
            {
                h = (1f - (y / (double)(hueImage.Texture.Height - 1))) * 360f;
                for (int x = 0; x < hueImage.Texture.Width; x++)
                {
                    data[y * hueImage.Texture.Width + x] = HsvToRgb(h, 1f, 1f);
                }
            }

            hueImage.Texture.SetData<Color>(data);
        }

        /// <summary>
        /// Create Color texture
        /// </summary>
        private void GenerateColorWheel()
        {
            int size = colorImage.Texture.Width;

            Color[] data = new Color[size * size];
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    data[y * size + x] = HsvToRgb(colorHue, x / (double)(size - 1), (1f - y / (double)(size - 1)));
                }
            }

            colorImage.Texture.SetData<Color>(data);
        }

        /// <summary>
        /// Converts HSV to RGB
        /// </summary>
        /// <param name="h">Hue</param>
        /// <param name="s">Saturation</param>
        /// <param name="v">Value</param>
        /// <returns>Converted Color</returns>
        private Color HsvToRgb(double h, double s, double v)
        {
            double r;
            double g;
            double b;
            int i;
            double f, p, q, t;

            if (s == 0)
            {
                // achromatic (grey)
                r = g = b = v;
                return new Color((float)r, (float)g, (float)b);
            }

            h /= 60;			// sector 0 to 5
            i = (int)Math.Floor(h);
            f = h - i;			// factorial part of h
            p = v * (1 - s);
            q = v * (1 - s * f);
            t = v * (1 - s * (1 - f));

            switch (i)
            {
                case 0:
                    r = v;
                    g = t;
                    b = p;
                    break;
                case 1:
                    r = q;
                    g = v;
                    b = p;
                    break;
                case 2:
                    r = p;
                    g = v;
                    b = t;
                    break;
                case 3:
                    r = p;
                    g = q;
                    b = v;
                    break;
                case 4:
                    r = t;
                    g = p;
                    b = v;
                    break;
                default:		// case 5:
                    r = v;
                    g = p;
                    b = q;
                    break;
            }

            return new Color((float)r, (float)g, (float)b);
        }

        /// <summary>
        /// Converts RGB to HSV
        /// </summary>
        private void RGBtoHSV(float r, float g, float b, out double h, out double s, out double v)
        {
            double min, max, delta;

            r = r / 255;
            g = g / 255;
            b = b / 255;

            min = MathHelper.Min(MathHelper.Min(r, g), b);
            max = MathHelper.Max(MathHelper.Max(r, g), b);
            v = max;				// v

            delta = max - min;

            if (max != 0)
                s = delta / max;		// s
            else
            {
                // r = g = b = 0		// s = 0, v is undefined
                s = 0;
                h = 0;
                return;
            }

            if (delta == 0)
            {
                h = 0f;
            }
            else
            {
                if (r == max)
                    h = (g - b) / delta;		// between yellow & magenta
                else if (g == max)
                    h = 2 + (b - r) / delta;	// between cyan & yellow
                else
                    h = 4 + (r - g) / delta;	// between magenta & cyan
            }


            h *= 60;				// degrees
            if (h < 0)
                h += 360;
        }

        #endregion
    }
}
