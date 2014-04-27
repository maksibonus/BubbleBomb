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
/// <summary>RadioButton Class</summary>
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
    /// RadioButton Control
    /// </summary>
    public class RadioButton : GUIControl
    {
        #region Public Properties
        /// <summary>
        /// RadioButton Text
        /// </summary>
        public string Text = String.Empty;

        /// <summary>
        /// Sets of gets if RadioButton is checked
        /// </summary>
        public bool Checked
        {
            get { return _checked; }
            set
            {
                if (value)
                {
                    ResetRadioButtons();
                }
                _checked = value;
            }
        }
        private bool _checked = false;

        /// <summary>
        /// Group of RadioButton Controls (only single one can be checked at the time)
        /// </summary>
        public int Group = 0;

        /// <summary>
        /// Automatically update Checked state on mouse click
        /// </summary>
        public bool AutoCheck = true;

        /// <summary>
        /// Default Font
        /// </summary>
        public SpriteFont Font
        {
            get { return _font ?? Theme.RadioButtonFont; }
            set { _font = value; }
        }
        private SpriteFont _font = null;

        /// <summary>
        /// Text color
        /// </summary>
        public Color TextColor
        {
            get { return _textColor ?? Theme.RadioButtonColor; }
            set { _textColor = value; }
        }
        private Color? _textColor = null;

        /// <summary>
        /// If set, Width and Bound is updated automatically
        /// </summary>
        public bool AutoSize = false;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a RadioButton control
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control</param>
        /// <param name="text">RadioButton text</param>
        /// <param name="name">Control name</param>
        public RadioButton(Rectangle bounds, string text, string name)
            : base(bounds)
        {
            // if bounds not set - treat it as AutoSize
            if (bounds.Width <= 0 || bounds.Height <= 0)
                AutoSize = true;

            Text = text;
            Name = name;
            Init();
        }

        /// <summary>
        /// Creates a RadioButton control
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control</param>
        /// <param name="text">RadioButton text</param>
        public RadioButton(Rectangle bounds, string text)
            : this(bounds, text, String.Empty)
        {   
        }

        /// <summary>
        /// Creates RadioButton control
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control</param>
        public RadioButton(Rectangle bounds)
            : this(bounds, String.Empty)
        {
        }

        /// <summary>
        /// Creates RadioButton control and loads its data from XmlNode
        /// </summary>
        /// <param name="xmlNode">XmlNode containing control data</param>
        public RadioButton(XmlNode xmlNode)
            : base(xmlNode)
        {
            Init();
        }

        private void Init()
        {
            // handle checking of boxes
            OnMousePressed += new MousePressedEventHandler(RadioBox_OnMousePressed);
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
            if (xmlNode.Attributes["Group"] != null)
                Group = Int32.Parse(xmlNode.Attributes["Group"].Value);
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
            xmlElement.SetAttribute("Group", Group.ToString());
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

            Bounds.Width = (int)textSize.X + (Checked ? Theme.SkinRadioButtonChecked.Width : Theme.SkinRadioButton.Width) + Theme.RadioButtonSpacing;
            Bounds.Height = (int)textSize.Y;
        }

        private void RadioBox_OnMousePressed(GUIControl sender, MouseState mouseState)
        {
            if (AutoCheck && IsMouseLeftDown)
            {
                ResetRadioButtons();
                Checked = true;
            }
        }

        /// <summary>
        /// Removes all checks from other RadioButton controls (from the same group) and checks the active one
        /// </summary>
        private void ResetRadioButtons()
        {
            // reset checked statuses on same group radio buttons (from the same Parent)
            if (Parent != null)
                foreach (GUIControl control in Parent.Controls)
                {
                    if (control is RadioButton)
                    {
                        if ((control as RadioButton).Group == Group)
                            (control as RadioButton).Checked = false;
                    }
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
            Color tint = Theme.RadioButtonTintColor[(int)state] * Transparency;

            // draw box = allways square
            if (!Checked)
            {
                spriteBatch.Draw(Theme.Skin,
                    new Rectangle(bounds.X, bounds.Y, bounds.Height, bounds.Height),
                    Theme.SkinRadioButtonChecked, tint);
            }
            else
            {
                spriteBatch.Draw(Theme.Skin,
                    new Rectangle(bounds.X, bounds.Y, bounds.Height, bounds.Height),
                    Theme.SkinRadioButton, tint);
            }

            // draw text
            spriteBatch.DrawString(Font, Text,
                new Vector2(AbsoluteBounds.X + bounds.Height + Theme.RadioButtonSpacing, AbsoluteBounds.Y),
                TextColor * Transparency);

            base.Draw(spriteBatch);
        }
        #endregion
    }
}
