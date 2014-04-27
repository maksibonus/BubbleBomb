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
/// <summary>Progress Class</summary>
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
    /// Progress Control
    /// </summary>
    public class Progress : GUIControl
    {
        #region Public Properties
        /// <summary>
        /// Minimum allowed value. Default: 0
        /// </summary>
        public float MinValue = 0f;

        /// <summary>
        /// Maximum allowed value. Default: 1
        /// </summary>
        public float MaxValue = 1f;

        /// <summary>
        /// Value of a Progress control
        /// </summary>
        public float Value
        {
            get { return _value; }
            set
            {
                _value = MathHelper.Clamp(value, MinValue, MaxValue);

                if (OnProgressChanged != null)
                    OnProgressChanged(this);
            }
        }
        private float _value = 0f;

        /// <summary>
        /// Displays progress on the bar
        /// </summary>
        public bool DisplayProgress = false;

        /// <summary>
        /// If set and if DisplayProgress is set, Value in percentage form will be displayed
        /// </summary>
        public bool ShowPercentage = true;

        /// <summary>
        /// Indicates if user can change the progress (with left mouse button)
        /// </summary>
        public bool Clickable = false;

        /// <summary>
        /// Default Font
        /// </summary>
        public SpriteFont Font
        {
            get { return _font ?? Theme.ProgressFont; }
            set { _font = value; }
        }
        private SpriteFont _font = null;

        /// <summary>
        /// Text color
        /// </summary>
        public Color TextColor
        {
            get { return _textColor ?? Theme.ProgressColor; }
            set { _textColor = value; }
        }
        private Color? _textColor = null;
        #endregion

        #region Events and Delegates
        /// <summary>
        /// Delegate for OnProgressChanged events
        /// </summary>
        /// <param name="sender">Control that triggered the event</param>
        public delegate void ProgressChangedEventHandler(GUIControl sender);
        /// <summary>
        /// Progress Value has been changed
        /// </summary>
        public event ProgressChangedEventHandler OnProgressChanged;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates Progress Control
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control</param>
        /// <param name="name">Control name</param>
        public Progress(Rectangle bounds, string name)
            : base(bounds)
        {
            Name = name;
            Init();
        }

        /// <summary>
        /// Creates Progress Control
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control</param>
        public Progress(Rectangle bounds)
            : this(bounds, String.Empty)
        {
        }

        /// <summary>
        /// Creates Progress control and loads its data from XmlNode
        /// </summary>
        /// <param name="xmlNode">XmlNode containing control data</param>
        public Progress(XmlNode xmlNode)
            : base(xmlNode)
        {
            Init();
        }

        private void Init()
        {
            // handles mouse clicks
            OnMouseMove += new MouseMoveEventHandler(Progress_OnMouseMove);
        }
        #endregion

        #region Save and Load
        public override void LoadControl(XmlNode xmlNode)
        {
            base.LoadControl(xmlNode);

            if (xmlNode.Attributes["Value"] != null)
                Value = float.Parse(xmlNode.Attributes["Value"].Value);
            if (xmlNode.Attributes["Clickable"] != null)
                Clickable = bool.Parse(xmlNode.Attributes["Clickable"].Value);
            if (xmlNode.Attributes["DisplayProgress"] != null)
                DisplayProgress = bool.Parse(xmlNode.Attributes["DisplayProgress"].Value);
            if (xmlNode.Attributes["TextColor"] != null)
                TextColor = xmlNode.Attributes["TextColor"].Value.ToXNAColor();
            if (xmlNode.Attributes["MinValue"] != null)
                MinValue = float.Parse(xmlNode.Attributes["MinValue"].Value);
            if (xmlNode.Attributes["MaxValue"] != null)
                MaxValue = float.Parse(xmlNode.Attributes["MaxValue"].Value);
            if (xmlNode.Attributes["ShowPercentage"] != null)
                ShowPercentage = bool.Parse(xmlNode.Attributes["ShowPercentage"].Value);
        }

        public override XmlElement SaveControl(XmlDocument xmlDocument)
        {
            XmlElement xmlElement = base.SaveControl(xmlDocument);

            xmlElement.SetAttribute("Value", Value.ToString());
            xmlElement.SetAttribute("Clickable", Clickable.ToString());
            xmlElement.SetAttribute("DisplayProgress", DisplayProgress.ToString());
            xmlElement.SetAttribute("TextColor", TextColor.ToXNAString());
            xmlElement.SetAttribute("MinValue", MinValue.ToString());
            xmlElement.SetAttribute("MaxValue", MaxValue.ToString());
            xmlElement.SetAttribute("ShowPercentage", ShowPercentage.ToString());

            return xmlElement;
        }
        #endregion

        #region Private Methods
        private void Progress_OnMouseMove(GUIControl sender, MouseState mouseState)
        {
            // handles value changes
            if (Clickable && IsMouseLeftDown)
            {
                float delta = (float)(mouseState.X - AbsoluteBounds.X);
                Value = MinValue + ((delta / (float)Bounds.Width) * (MaxValue - MinValue));

                if (delta < 3)
                    Value = MinValue;
                if (delta > Bounds.Width - 4)
                    Value = MaxValue;
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
            Color tint = Theme.ProgressTintColor[(int)state] * Transparency;

            // left
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X, bounds.Y, Theme.SkinProgressLeft.Width, bounds.Height),
                Theme.SkinProgressLeft, tint);

            // right
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + bounds.Width - Theme.SkinProgressRight.Width, bounds.Y, Theme.SkinProgressRight.Width, bounds.Height),
                Theme.SkinProgressRight, tint);

            // top
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + Theme.SkinProgressLeft.Width, bounds.Y, bounds.Width - Theme.SkinProgressLeft.Width - Theme.SkinProgressRight.Width, Theme.SkinProgressTop.Height), 
                Theme.SkinProgressTop, tint);

            // bottom
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + Theme.SkinProgressLeft.Width, bounds.Y + bounds.Height - Theme.SkinProgressBottom.Height, bounds.Width - Theme.SkinProgressLeft.Width - Theme.SkinProgressRight.Width, Theme.SkinProgressBottom.Height),
                Theme.SkinProgressBottom, tint);

            // middle
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + Theme.SkinProgressLeft.Width, bounds.Y + Theme.SkinProgressTop.Height, bounds.Width - Theme.SkinProgressLeft.Width - Theme.SkinProgressRight.Width, bounds.Height - Theme.SkinProgressTop.Height - Theme.SkinProgressBottom.Height), 
                Theme.SkinProgressMiddle, tint);

            // progress indicator
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + 1, bounds.Y + 1, (int)((float)bounds.Width * ((Value - MinValue) / (MaxValue - MinValue))) - 2, bounds.Height - 2), // 1px margin
                Theme.SkinProgressIndicator, tint);

            // progress text
            if (DisplayProgress)
            {
                string progressString = ShowPercentage ? (((Value - MinValue) / (MaxValue - MinValue)) * 100).ToString("F0") + "%" : Value.ToString();
                Vector2 stringSize = Font.MeasureString(progressString);
                
                spriteBatch.DrawString(Font, progressString,
                    new Vector2(bounds.X + (bounds.Width / 2) - (stringSize.X / 2), bounds.Y + (bounds.Height / 2) - (stringSize.Y / 2)),
                    TextColor * Transparency);
            }
            
            base.Draw(spriteBatch);
        }
        #endregion
    }
}
