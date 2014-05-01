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
/// <summary>Button Class</summary>
#endregion

using System;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RamGecXNAControlsExtensions;

namespace RamGecXNAControls
{
    /// <summary>
    /// Button Control
    /// </summary>
    public class Button : GUIControl
    {
        #region Public Properties
        /// <summary>
        /// Button text
        /// </summary>
        public String Text = String.Empty;

        /// <summary>
        /// Default Font
        /// </summary>
        public SpriteFont Font
        {
            get { return _font ?? Theme.ButtonFont; }
            set { _font = value; }
        }
        private SpriteFont _font = null;

        /// <summary>
        /// Text color
        /// </summary>
        public Color TextColor
        {
            get { return _textColor ?? Theme.ButtonColor; }
            set { _textColor = value; }
        }
        private Color? _textColor = null;

        /// <summary>
        /// If set, Width and Bound is updated automatically
        /// </summary>
        public bool AutoSize = false;

        /// <summary>
        /// Button Image
        /// </summary>
        public Texture2D Icon = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates button control
        /// </summary>
        /// <param name="bounds">Relative location within parent control. If Width/Height is 0, treat it as AutoSize</param>
        /// <param name="text">Button text</param>
        /// <param name="name">Control name</param>
        public Button(Rectangle bounds, string text, string name)
            : base(bounds)
        {
            // if bounds not set - treat it as AutoSize
            if (bounds.Width <= 0 || bounds.Height <= 0)
                AutoSize = true;

            Text = text;
            Name = name;
        }

        /// <summary>
        /// Creates Button control
        /// </summary>
        /// <param name="bounds">Relative location within parent control. If Width/Height is 0, treat it as AutoSize</param>
        /// <param name="text">Button text</param>
        public Button(Rectangle bounds, string text)
            : this(bounds, text, String.Empty)
        {   
        }

        /// <summary>
        /// Creates Button control
        /// </summary>
        /// <param name="bounds">Relative location within parent control. If Width/Height is 0, treat it as AutoSize</param>
        public Button(Rectangle bounds)
            : this(bounds, String.Empty)
        {
        }

        /// <summary>
        /// Creates Button control and loads its data from XmlNode
        /// </summary>
        /// <param name="xmlNode">XmlNode containing control data</param>
        public Button(XmlNode xmlNode)
            : base(xmlNode)
        {
        }
        #endregion

        #region Save and Load
        public override void LoadControl(XmlNode xmlNode)
        {
            base.LoadControl(xmlNode);

            if (xmlNode.Attributes["Text"] != null)
                Text = xmlNode.Attributes["Text"].Value;
            if (xmlNode.Attributes["AutoSize"] != null)
                AutoSize = bool.Parse(xmlNode.Attributes["AutoSize"].Value);
            if (xmlNode.Attributes["TextColor"] != null)
                TextColor = xmlNode.Attributes["TextColor"].Value.ToXNAColor();
        }

        public override XmlElement SaveControl(XmlDocument xmlDocument)
        {
            XmlElement xmlElement = base.SaveControl(xmlDocument);

            xmlElement.SetAttribute("Text", Text);
            xmlElement.SetAttribute("AutoSize", AutoSize.ToString());
            xmlElement.SetAttribute("TextColor", TextColor.ToXNAString());

            return xmlElement;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Perform AutoSize
        /// </summary>
        private void DoAutoSize()
        {
            Vector2 textSize = Font.MeasureString(Text);

            Bounds.Width = (int)textSize.X + Theme.SkinButtonLeft.Width + Theme.SkinButtonRight.Width + 12;
            Bounds.Height = (int)textSize.Y + 6;

            if (Icon != null)
            {
                if (String.IsNullOrEmpty(Text))
                    Bounds.Height = 24;
                Bounds.Width += (int)(Bounds.Height * Theme.ButtomIconScale) + Theme.ButtonIconSpacing;
            }
        }
        #endregion

        #region Draw and Update
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
                return;

            if (AutoSize)
                DoAutoSize();

            // cache
            Rectangle bounds = AbsoluteBounds;
            Color tint = Theme.ButtonTintColor[(int)state] * Transparency;

            // left
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X, bounds.Y, Theme.SkinButtonLeft.Width, bounds.Height),
                Theme.SkinButtonLeft, tint);

            // right
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + bounds.Width - Theme.SkinButtonRight.Width, bounds.Y, Theme.SkinButtonRight.Width, bounds.Height),
                Theme.SkinButtonRight, tint);

            // middle
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + Theme.SkinButtonLeft.Width, bounds.Y, bounds.Width - Theme.SkinButtonLeft.Width - Theme.SkinButtonRight.Width, bounds.Height),
                Theme.SkinButtonMiddle, tint);

            Vector2 textSize = Theme.ButtonFont.MeasureString(Text);
            int actualX = bounds.X + (bounds.Width / 2);
            int actualY = bounds.Y + (bounds.Height / 2);
            int textureSize = (int)(bounds.Height * Theme.ButtomIconScale);

            // draw image
            if (Icon != null)
            {
                actualX = actualX - (int)((textureSize + (int)textSize.X) / 2);
                spriteBatch.Draw(Icon, new Rectangle(actualX, actualY - (textureSize / 2), textureSize, textureSize), tint);

                actualX = actualX + textureSize + Theme.ButtonIconSpacing + (int)(textSize.X / 2);
            }

            // text
            if (!String.IsNullOrEmpty(Text))
            {   
                spriteBatch.DrawString(Font,
                    Text, new Vector2(actualX - (textSize.X / 2), actualY - (textSize.Y / 2)), // display in the middle
                    TextColor * Transparency);
            }

            base.Draw(spriteBatch);
        }
        #endregion
    }
}
