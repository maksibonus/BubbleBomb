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
/// <summary>GroupBox Class</summary>
#endregion

using System;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RamGecXNAControlsExtensions;

namespace RamGecXNAControls
{
    /// <summary>
    /// GroupBox Control
    /// </summary>
    public class GroupBox : GUIControl
    {
        #region Public Properties
        /// <summary>
        /// GroupBox Title
        /// </summary>
        public string Title = String.Empty;

        /// <summary>
        /// Default Font
        /// </summary>
        public SpriteFont Font
        {
            get { return _font ?? Theme.GroupBoxFont; }
            set { _font = value; }
        }
        private SpriteFont _font = null;

        /// <summary>
        /// Text color
        /// </summary>
        public Color TitleColor
        {
            get { return _titleColor ?? Theme.GroupBoxTitleColor; }
            set { _titleColor = value; }
        }
        private Color? _titleColor = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a GroupBox control
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control</param>
        /// <param name="title">GroupBox text</param>
        /// <param name="name">Control name</param>
        public GroupBox(Rectangle bounds, string title, string name)
            : base(bounds)
        {
            Title = title;
            Name = name;
        }

        /// <summary>
        /// Creates a GroupBox control
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control</param>
        /// <param name="title">GroupBox text</param>
        public GroupBox(Rectangle bounds, string title)
            : this(bounds, title, String.Empty)
        {
        }

        /// <summary>
        /// Creates a GroupBox control
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control</param>
        public GroupBox(Rectangle bounds)
            : this(bounds, String.Empty, String.Empty)
        {
        }

        /// <summary>
        /// Creates GroupBox control and loads its data from XmlNode
        /// </summary>
        /// <param name="xmlNode">XmlNode containing control data</param>
        public GroupBox(XmlNode xmlNode)
            : base(xmlNode)
        {
        }
        #endregion

        #region Save and Load
        public override void LoadControl(XmlNode xmlNode)
        {
            base.LoadControl(xmlNode);

            if (xmlNode.Attributes["Title"] != null)
                Title = xmlNode.Attributes["Title"].Value;
            if (xmlNode.Attributes["TitleColor"] != null)
                TitleColor = xmlNode.Attributes["TitleColor"].Value.ToXNAColor();
        }

        public override XmlElement SaveControl(XmlDocument xmlDocument)
        {
            XmlElement xmlElement = base.SaveControl(xmlDocument);

            xmlElement.SetAttribute("Title", Title);
            xmlElement.SetAttribute("TitleColor", TitleColor.ToXNAString());

            return xmlElement;
        }
        #endregion

        #region Draw and Update
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
                return;

            // cache
            Rectangle bounds = AbsoluteBounds;
            Color tint = Theme.GroupBoxTintColor[(int)state] * Transparency;
            Vector2 titleSize = Font.MeasureString(Title);

            // top-left
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X, bounds.Y, Theme.SkinGroupBoxTopLeft.Width, Theme.SkinGroupBoxTopLeft.Height),
                Theme.SkinGroupBoxTopLeft, tint);

            // top-right
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + bounds.Width - Theme.SkinGroupBoxTopRight.Width, bounds.Y, Theme.SkinGroupBoxTopRight.Width, Theme.SkinGroupBoxTopRight.Height),
                Theme.SkinGroupBoxTopRight, tint);

            // top
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + Theme.SkinGroupBoxTopLeft.Width + (int)titleSize.X + 8, bounds.Y, bounds.Width - Theme.SkinGroupBoxTopLeft.Width - Theme.SkinGroupBoxTopRight.Width - (int)titleSize.X - 8, Theme.SkinGroupBoxTop.Height),
                Theme.SkinGroupBoxTop, tint);

            // left
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X, bounds.Y + Theme.SkinGroupBoxTopLeft.Width, Theme.SkinGroupBoxLeft.Width, bounds.Height - Theme.SkinGroupBoxTopLeft.Height - Theme.SkinGroupBoxBottomLeft.Height),
                Theme.SkinGroupBoxLeft, tint);

            // right
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + bounds.Width - Theme.SkinGroupBoxRight.Width, bounds.Y + Theme.SkinGroupBoxTopRight.Height, Theme.SkinGroupBoxRight.Width, bounds.Height - Theme.SkinGroupBoxTopRight.Height - Theme.SkinGroupBoxBottomRight.Height),
                Theme.SkinGroupBoxRight, tint);

            // bottom-left
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X, bounds.Y + bounds.Height - Theme.SkinGroupBoxBottomLeft.Height, Theme.SkinGroupBoxBottomLeft.Width, Theme.SkinGroupBoxBottomLeft.Height),
                Theme.SkinGroupBoxBottomLeft, tint);

            // bottom-right
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + bounds.Width - Theme.SkinGroupBoxBottomRight.Width, bounds.Y + bounds.Height - Theme.SkinGroupBoxBottomRight.Height, Theme.SkinGroupBoxBottomRight.Width, Theme.SkinGroupBoxBottomRight.Height),
                Theme.SkinGroupBoxBottomRight, tint);

            // bottom
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + Theme.SkinGroupBoxBottomLeft.Width, bounds.Y + bounds.Height - Theme.SkinGroupBoxBottom.Height, bounds.Width - Theme.SkinGroupBoxBottomLeft.Width - Theme.SkinGroupBoxBottomRight.Width, Theme.SkinGroupBoxBottom.Height),
                Theme.SkinGroupBoxBottom, tint);

            // middle
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + Theme.SkinGroupBoxLeft.Width, bounds.Y + Theme.SkinGroupBoxTop.Height, bounds.Width - Theme.SkinGroupBoxLeft.Width - Theme.SkinGroupBoxRight.Width, bounds.Height - Theme.SkinGroupBoxTop.Height - Theme.SkinGroupBoxBottom.Height),
                Theme.SkinGroupBoxMiddle, tint);


            // title
            spriteBatch.DrawString(Font, Title,
                new Vector2(bounds.X + Theme.SkinGroupBoxTopLeft.Width + 4, bounds.Y - (titleSize.Y / 2) + 2),
                TitleColor * Transparency);

            base.Draw(spriteBatch);
        }
        #endregion
    }
}
