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
/// <summary>MessageBox Class</summary>
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using RamGecXNAControlsExtensions;

namespace RamGecXNAControls.ExtendedControls
{
    /// <summary>
    /// MessageBox Dialog
    /// </summary>
    public class MessageBox
    {
        #region Public Properties
        /// <summary>
        /// Message text
        /// </summary>
        public string Text
        {
            set
            {
                message.Text = value;
            }
            get
            {
                return message.Text;
            }
        }

        /// <summary>
        /// MessageBox window title
        /// </summary>
        public string Title
        {
            set
            {
                boxWindow.Title = value;
            }
            get
            {
                return boxWindow.Title;
            }
        }

        /// <summary>
        /// OK Button Icon
        /// </summary>
        public Texture2D ButtonIcon
        {
            set
            {
                okButton.Icon = value;
            }
            get
            {
                return okButton.Icon;
            }
        }
        #endregion

        #region Private Properties
        /// <summary>
        /// MessageBox Window
        /// </summary>
        private Window boxWindow = null;

        private GUIManager guiManager = null;

        private Label message = null;
        private Button okButton = null;
        #endregion

        #region Events and Delegates
        /// <summary>
        /// Delegate for OnOKClick events
        /// </summary>
        /// <param name="sender">Control that triggered the event</param>
        public delegate void OKClickEventHandler();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates and displayes MessageBox dialog window
        /// </summary>
        /// <param name="location">Location of top-left corner of MessageBox window. If Point.Zero - display in the middle of the screen</param>
        /// <param name="guiManager">GUIManager where this control should be drawed</param>
        /// <param name="text">Message text</param>
        /// <param name="title">MessageBox window title</param>
        /// <param name="onOKClickCallback">Callback method that is called when OK button is clicked</param>
        public MessageBox(Point location, GUIManager guiManager, string text, string title, OKClickEventHandler onOKClickCallback)
        {
            this.guiManager = guiManager;

            // make sure the title won't outstretch the window
            if (title.Length > text.Length)
                title = title.Substring(0, text.Length);

            boxWindow = new Window(Rectangle.Empty, title, "MessageBox Window");
            boxWindow.TopMost = true;

            boxWindow.Bounds.X = location.X;
            boxWindow.Bounds.Y = location.Y;
            boxWindow.Bounds.Width = 100;
            boxWindow.Bounds.Height = 100;

            int textHeight = (int)guiManager.Theme.DefaultFont.MeasureString(text).Y;

            message = new Label(new Rectangle(20, 30, 0, 0), text);
            boxWindow.Controls.Add(message);

            okButton = new Button(new Rectangle(boxWindow.Bounds.Width / 2 - 40, textHeight + 30 + 10, 80, 24), "OK");
            okButton.Icon = guiManager.Theme.IconYes;
            okButton.OnClick += (s) =>
                {
                    Close();
                    if (onOKClickCallback != null)
                        onOKClickCallback();
                };

            CalculateSize();
            boxWindow.Controls.Add(okButton);
        }

        /// <summary>
        /// Creates and displayes MessageBox dialog window
        /// </summary>
        /// <param name="location">Location of top-left corner of MessageBox window. If Point.Zero - display in the middle of the screen</param>
        /// <param name="guiManager">GUIManager where this control should be drawed</param>
        /// <param name="text">Message text</param>
        /// <param name="title">MessageBox window title</param>
        public MessageBox(Point location, GUIManager guiManager, string text, string title)
            : this(location, guiManager, text, title, null)
        {
        }

        /// <summary>
        /// Creates and displayes MessageBox dialog window
        /// </summary>
        /// <param name="location">Location of top-left corner of MessageBox window. If Point.Zero - display in the middle of the screen</param>
        /// <param name="guiManager">GUIManager where this control should be drawed</param>
        /// <param name="text">Message text</param>
        public MessageBox(Point location, GUIManager guiManager, string text)
            : this(location, guiManager, text, String.Empty, null)
        {
        }

        /// <summary>
        /// Creates and displayes MessageBox dialog window in the center of the screen
        /// </summary>
        /// <param name="guiManager">GUIManager where this control should be drawed</param>
        /// <param name="text">Message text</param>
        public MessageBox(GUIManager guiManager, string text)
            : this(Point.Zero, guiManager, text, String.Empty, null)
        {
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Show Window (add control to GUIManager)
        /// </summary>
        /// <param name="text">Message text</param>
        public void Show(string text)
        {
            CalculateSize();
            Text = text;
            guiManager.Controls.Add(boxWindow);
        }

        /// <summary>
        /// Show Window (add control to GUIManager)
        /// </summary>
        public void Show()
        {
            CalculateSize();
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
        private void CalculateSize()
        {
            int textWidth = (int)guiManager.Theme.DefaultFont.MeasureString(Text).X;
            int textHeight = (int)guiManager.Theme.DefaultFont.MeasureString(Text).Y;

            // set window size
            boxWindow.Bounds.X = boxWindow.Bounds.X;
            boxWindow.Bounds.Y = boxWindow.Bounds.Y;
            boxWindow.Bounds.Width = (int)MathHelper.Clamp(textWidth + 40, 120, 10000);
            boxWindow.Bounds.Height = (int)MathHelper.Clamp(textHeight + 80, 80, 10000);

            // make it in the center of the screen
            if (boxWindow.Bounds.X == 0 && boxWindow.Bounds.Y == 0)
            {
                boxWindow.Bounds.X = (guiManager.Game.GraphicsDevice.Viewport.Width / 2) - (boxWindow.Bounds.Width / 2);
                boxWindow.Bounds.Y = (guiManager.Game.GraphicsDevice.Viewport.Height / 2) - (boxWindow.Bounds.Height / 2);
            }

            okButton.Bounds = new Rectangle(boxWindow.Bounds.Width / 2 - 40, textHeight + 30 + 10, 80, 24);
        }
        #endregion
    }
}
