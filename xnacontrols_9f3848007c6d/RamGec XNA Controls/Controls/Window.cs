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
/// <summary>Window Class</summary>
#endregion

using System;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RamGecXNAControlsExtensions;

// TODO: Add a close box

namespace RamGecXNAControls
{
    /// <summary>
    /// Window Control
    /// </summary>
    public class Window : GUIControl
    {
        #region Public Properties
        /// <summary>
        /// Window Title
        /// </summary>
        public string Title = String.Empty;

        /// <summary>
        /// If set, the Window will be drown last (that is - on top of other windows).
        /// If more than one Window is TopMost - the first added window will be on top
        /// </summary>
        public bool TopMost = false;

        /// <summary>
        /// Allows user to move the window around
        /// </summary>
        public bool Movable = true;

        /// <summary>
        /// Default Font
        /// </summary>
        public SpriteFont Font
        {
            get { return _font ?? Theme.WindowFont; }
            set { _font = value; }
        }
        private SpriteFont _font = null;

        /// <summary>
        /// Window Title color
        /// </summary>
        public Color TitleColor
        {
            get { return _titleColor ?? Theme.WindowTitleColor; }
            set { _titleColor = value; }
        }
        private Color? _titleColor = null;
        #endregion

        #region Private Properties
        /// <summary>
        /// Tracks mouse movement (for moving around the window)
        /// </summary>
        private Point movePoint;

        /// <summary>
        /// Cached height of the title bar
        /// </summary>
        private int titleHeight = 24;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates Window control
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control</param>
        /// <param name="title">Window title</param>
        /// <param name="name">Control name</param>
        public Window(Rectangle bounds, string title, string name)
            : base(bounds)
        {
            Title = title;
            Name = name;
            Init();
        }
        
        /// <summary>
        /// Creates Window control
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control</param>
        /// <param name="title">Window title</param>
        public Window(Rectangle bounds, string title)
            : this(bounds, title, String.Empty)
        {
        }

        /// <summary>
        /// Creates Window control
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control</param>
        public Window(Rectangle bounds)
            : this(bounds, String.Empty, String.Empty)
        {
        }

        /// <summary>
        /// Creates Window control and loads its data from XmlNode
        /// </summary>
        /// <param name="xmlNode">XmlNode containing control data</param>
        public Window(XmlNode xmlNode)
            : base(xmlNode)
        {
            Init();
        }

        private void Init()
        {
            OnMousePressed += new MousePressedEventHandler(Window_OnMousePressed);
            OnMouseMove += new MouseMoveEventHandler(Window_OnMouseMove);
        }
        #endregion

        #region Save and Load
        public override void LoadControl(XmlNode xmlNode)
        {
            base.LoadControl(xmlNode);

            if (xmlNode.Attributes["Title"] != null)
                Title = xmlNode.Attributes["Title"].Value;
            if (xmlNode.Attributes["Movable"] != null)
                Movable = bool.Parse(xmlNode.Attributes["Movable"].Value);
            if (xmlNode.Attributes["TitleColor"] != null)
                TitleColor = xmlNode.Attributes["TitleColor"].Value.ToXNAColor();
            if (xmlNode.Attributes["TopMost"] != null)
                TopMost = bool.Parse(xmlNode.Attributes["TopMost"].Value);
        }

        public override XmlElement SaveControl(XmlDocument xmlDocument)
        {
            XmlElement xmlElement = base.SaveControl(xmlDocument);

            xmlElement.SetAttribute("Title", Title);
            xmlElement.SetAttribute("Movable", Movable.ToString());
            xmlElement.SetAttribute("TitleColor", TitleColor.ToXNAString());
            xmlElement.SetAttribute("TopMost", TopMost.ToString());
            
            return xmlElement;
        }
        #endregion

        #region Private Methods
        private void Window_OnMouseMove(GUIControl sender, MouseState mouseState)
        {
            // move window only if clicked on title bar
            if (mouseState.Y < AbsoluteBounds.Y + 24)
                if (Movable && mouseState.LeftButton == ButtonState.Pressed)
                {
                    Bounds.X += mouseState.X - movePoint.X;
                    Bounds.Y += mouseState.Y - movePoint.Y;

                    movePoint.X = mouseState.X;
                    movePoint.Y = mouseState.Y;
                }
        }

        private void Window_OnMousePressed(GUIControl sender, MouseState mouseState)
        {
            // store the location where the mouse was pressed
            if (Movable)
            {
                movePoint.X = mouseState.X;
                movePoint.Y = mouseState.Y;
            }
        }
        #endregion

        #region Draw and Update
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
                return;

            // cache
            Rectangle bounds = AbsoluteBounds;
            Color tint = Theme.WindowTintColor[(int)state] * Transparency;

            titleHeight = Theme.SkinWindowTitleRight.Height;

            // title bar
            // left
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(Bounds.X, Bounds.Y, Theme.SkinWindowTitleLeft.Width, Theme.SkinWindowTitleLeft.Height),
                Theme.SkinWindowTitleLeft, tint);

            // right
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(Bounds.X + Bounds.Width - Theme.SkinWindowTitleRight.Width, Bounds.Y, Theme.SkinWindowTitleRight.Width, Theme.SkinWindowTitleRight.Height),
                Theme.SkinWindowTitleRight, tint);

            // top
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(Bounds.X + Theme.SkinWindowTitleLeft.Width, Bounds.Y, Bounds.Width - Theme.SkinWindowTitleLeft.Width - Theme.SkinWindowTitleRight.Width, Theme.SkinWindowTitle.Height), 
                Theme.SkinWindowTitle, tint);

            // body
            // left border
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(Bounds.X, Bounds.Y + Theme.SkinWindowTitleLeft.Height, Theme.SkinWindowLeft.Width, Bounds.Height - Theme.SkinWindowTitleLeft.Height - Theme.SkinWindowBottomLeft.Height),
                Theme.SkinWindowLeft, tint);

            // bottom left
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(Bounds.X, Bounds.Y + Bounds.Height - Theme.SkinWindowBottomLeft.Height, Theme.SkinWindowBottomLeft.Width, Theme.SkinWindowBottomLeft.Height),
                Theme.SkinWindowBottomLeft, tint);

            // right border
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(Bounds.X + Bounds.Width - Theme.SkinWindowRight.Width, Bounds.Y + Theme.SkinWindowTitleRight.Height, Theme.SkinWindowRight.Width, Bounds.Height - Theme.SkinWindowTitleRight.Height - Theme.SkinWindowBottomRight.Height),
                Theme.SkinWindowRight, tint);

            // bottom right
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(Bounds.X + Bounds.Width - Theme.SkinWindowBottomRight.Width, Bounds.Y + Bounds.Height - Theme.SkinWindowBottomRight.Height, Theme.SkinWindowBottomRight.Width, Theme.SkinWindowBottomRight.Height),
                Theme.SkinWindowBottomRight, tint);

            // bottom border
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(Bounds.X + Theme.SkinWindowBottomLeft.Width, Bounds.Y + Bounds.Height - Theme.SkinWindowBottom.Height, Bounds.Width - Theme.SkinWindowBottomLeft.Width - Theme.SkinWindowBottomRight.Width, Theme.SkinWindowBottom.Height), 
                Theme.SkinWindowBottom, tint);

            // body
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(Bounds.X + Theme.SkinWindowLeft.Width, Bounds.Y + Theme.SkinWindowTitle.Height, Bounds.Width - Theme.SkinWindowLeft.Width - Theme.SkinWindowRight.Width, Bounds.Height - Theme.SkinWindowTitle.Height - Theme.SkinWindowBottom.Height),
                Theme.SkinWindowBody, tint);

            // draw title
            spriteBatch.DrawString(Font, Title, 
                new Vector2(Bounds.X + 20, Bounds.Y + 2), // margins
                TitleColor * Transparency);

            base.Draw(spriteBatch);
        }
        #endregion
    }
}
