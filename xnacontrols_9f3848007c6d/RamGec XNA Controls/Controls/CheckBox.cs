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
/// <summary>CheckBox Class</summary>
#endregion

using System;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RamGecXNAControlsExtensions;

namespace RamGecXNAControls
{   
    /// <summary>
    /// CheckBox Control
    /// </summary>
    public class CheckBox : GUIControl
    {
        #region Public Properties
        /// <summary>
        /// CheckBox Text
        /// </summary>
        public string Text = String.Empty;

        /// <summary>
        /// Sets or gets whether this element was checked
        /// </summary>
        public bool Checked
        {
            get
            {
                return _checked;
            }
            set
            {
                if (value != _checked && OnChanged != null)
                {
                    _checked = value;
                    OnChanged(this);
                    return;
                }
                _checked = value;
            }
        }
        private bool _checked = false;

        /// <summary>
        /// Automatically change Checked state on mouse clicks
        /// </summary>
        public bool AutoCheck = true;

        /// <summary>
        /// Default Font
        /// </summary>
        public SpriteFont Font
        {
            get { return _font ?? Theme.CheckBoxFont; }
            set { _font = value; }
        }
        private SpriteFont _font = null;

        /// <summary>
        /// Text color
        /// </summary>
        public Color TextColor
        {
            get { return _textColor ?? Theme.CheckBoxColor; }
            set { _textColor = value; }
        }
        private Color? _textColor = null;

        /// <summary>
        /// If set, Width and Bound is updated automatically
        /// </summary>
        public bool AutoSize = false;
        #endregion

        #region Events and Delegates
        /// <summary>
        /// Delegate for handling OnChanged events
        /// </summary>
        /// <param name="sender">Control that triggered the event</param>
        public delegate void ChangedEventHandler(GUIControl sender);
        /// <summary>
        /// Submit key was pressed
        /// </summary>
        public event ChangedEventHandler OnChanged;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a CheckBox control
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control. If Width/Height is 0, treat it as AutoSize</param>
        /// <param name="text">CheckBox text</param>
        /// <param name="name">Control name</param>
        public CheckBox(Rectangle bounds, string text, string name)
            : base(bounds)
        {
            // if bounds not set - treat it as AutoSize
            if (bounds.Width <= 0 || bounds.Height <= 0)
                AutoSize = true;

            Text = text;
            Name = name;

            // handle checking of boxes
            OnMousePressed += new MousePressedEventHandler(CheckBox_OnMousePressed);
        }

        /// <summary>
        /// Creates a CheckBox control
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control. If Width/Height is 0, treat it as AutoSize</param>
        /// <param name="text">CheckBox text</param>
        public CheckBox(Rectangle bounds, string text)
            : this(bounds, text, String.Empty)
        {   
        }

        /// <summary>
        /// Creates a CheckBox control
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control. If Width/Height is 0, treat it as AutoSize</param>
        public CheckBox(Rectangle bounds)
            : this(bounds, String.Empty)
        {
        }

        /// <summary>
        /// Creates CheckBox control and loads its data from XmlNode
        /// </summary>
        /// <param name="xmlNode">XmlNode containing control data</param>
        public CheckBox(XmlNode xmlNode)
            : base(xmlNode)
        {
            // handle checking of boxes
            OnMousePressed += new MousePressedEventHandler(CheckBox_OnMousePressed);
        }
        #endregion

        #region Save and Load
        public override void LoadControl(XmlNode xmlNode)
        {
            base.LoadControl(xmlNode);

            if (xmlNode.Attributes["Text"] != null)
                Text = xmlNode.Attributes["Text"].Value;
            if (xmlNode.Attributes["Checked"] != null)
                Checked = bool.Parse(xmlNode.Attributes["Checked"].Value);
            if (xmlNode.Attributes["AutoCheck"] != null)
                AutoCheck = bool.Parse(xmlNode.Attributes["AutoCheck"].Value);
            if (xmlNode.Attributes["AutoSize"] != null)
                AutoSize = bool.Parse(xmlNode.Attributes["AutoSize"].Value);
            if (xmlNode.Attributes["TextColor"] != null)
                TextColor = xmlNode.Attributes["TextColor"].Value.ToXNAColor();
        }

        public override XmlElement SaveControl(XmlDocument xmlDocument)
        {
            XmlElement xmlElement = base.SaveControl(xmlDocument);

            xmlElement.SetAttribute("Text", Text);
            xmlElement.SetAttribute("Checked", Checked.ToString());
            xmlElement.SetAttribute("AutoCheck", AutoCheck.ToString());
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

            Bounds.Width = (int)textSize.X + (Checked ? Theme.SkinCheckBoxChecked.Width : Theme.SkinCheckBox.Width) + Theme.CheckBoxSpacing;
            Bounds.Height = (int)textSize.Y;
        }

        private void CheckBox_OnMousePressed(GUIControl sender, MouseState mouseState)
        {
            if (AutoCheck && IsMouseLeftDown)
                Checked = !Checked;
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
            Color tint = Theme.CheckBoxTintColor[(int)state] * Transparency;

            // draw box
            if (Checked)
            {
                spriteBatch.Draw(Theme.Skin,
                    new Rectangle(bounds.X, bounds.Y, bounds.Height, bounds.Height), // rectangular scale
                    Theme.SkinCheckBoxChecked, tint);
            }
            else
            {
                spriteBatch.Draw(Theme.Skin,
                    new Rectangle(bounds.X, bounds.Y, bounds.Height, bounds.Height), // rectangular scale
                    Theme.SkinCheckBox, tint);
            }

            // draw text
            spriteBatch.DrawString(Font, Text,
                new Vector2(bounds.X + bounds.Height + Theme.CheckBoxSpacing, bounds.Y), // draw text with a set spacing from the box
                TextColor * Transparency);

            base.Draw(spriteBatch);
        }
        #endregion
    }
}
