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
/// <summary>Label Class</summary>
#endregion

using System;
using System.Xml;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RamGecXNAControlsExtensions;

namespace RamGecXNAControls
{
    /// <summary>
    /// Label Control
    /// </summary>
    public class Label : GUIControl
    {
        #region Public Properties
        /// <summary>
        /// Label Text
        /// </summary>
        private string text = String.Empty;

        /// <summary>
        /// List of lines
        /// </summary>
        public List<string> Lines;

        /// <summary>
        /// Default Font
        /// </summary>
        public SpriteFont Font
        {
            get { return _font ?? Theme.LabelFont; }
            set { _font = value; }
        }
        private SpriteFont _font = null;

        /// <summary>
        /// Text color
        /// </summary>
        public Color TextColor
        {
            get { return _textColor ?? Theme.LabelTintColor[0]; }
            set { _textColor = value; }
        }
        private Color? _textColor = null;

        /// <summary>
        /// If set, Width and Bound is updated automatically
        /// </summary>
        public bool AutoSize = false;

        /// <summary>
        /// Formatted text
        /// </summary>
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                Lines.Clear();
                string curLine = "";
                for (int i = 0; i < text.Length && Lines.Count < 3; i++)
                {
                    curLine += text[i];
                    if (Font.MeasureString(curLine).X >= Bounds.Width)
                    {
                        Lines.Add(curLine);
                        curLine = "";
                    }
                }

                if (curLine != "")
                    Lines.Add(curLine);
                //    Lines.Reverse();
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a Label control
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control. If Width/Height is 0, treat it as AutoSize</param>
        /// <param name="text">Label text</param>
        /// <param name="name">Control name</param>
        public Label(Rectangle bounds, string text, string name)
            : base(bounds)
        {
            // if bounds not set - treat it as AutoSize
            if (bounds.Width <= 0 || bounds.Height <= 0)
                AutoSize = true;
            this.Lines = new List<string>();
            this.text = text;
            Name = name;
        }

        /// <summary>
        /// Creates a Label control
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control. If Width/Height is 0, treat it as AutoSize</param>
        /// <param name="text">Label text</param>
        public Label(Rectangle bounds, string text)
            : this(bounds, text, String.Empty)
        {
        }

        /// <summary>
        /// Creates a Label control
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control. If Width/Height is 0, treat it as AutoSize</param>
        public Label(Rectangle bounds)
            : this(bounds, String.Empty, String.Empty)
        {
        }

        /// <summary>
        /// Creates Label control and loads its data from XmlNode
        /// </summary>
        /// <param name="xmlNode">XmlNode containing control data</param>
        public Label(XmlNode xmlNode)
            : base(xmlNode)
        {
            this.Lines = new List<string>();
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

            Bounds.Width = (int)textSize.X;
            Bounds.Height = (int)textSize.Y;
        }

        /// <summary>
        /// Places lines of the rectangle
        /// </summary>
        /// 
        //если что мерять размер строки, и если он превышает рахмер ректангла переносить на следующую
        //private void DoNewLine()
        //{
        //    Vector2 textSize = Font.MeasureString(Text);
        //    float widthText = textSize.X;
        //    while (widthText > 0)
        //    {
        //        if (Bounds.Width < (int)textSize.X)
        //        {
                    
        //            Text.Insert(Bounds.Width, Environment.NewLine);
        //        }
        //        widthText -= Bounds.Width;
        //    }
        //}
        #endregion

        #region Draw and Update
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
                return;

            if (AutoSize)
                DoAutoSize();

            //if (AutoNewLine)
            //    DoNewLine();
            // cache
            Rectangle bounds = AbsoluteBounds;
            Color tint = (_textColor ?? Theme.LabelTintColor[(int)state]) * Transparency;

            // draw text
            for (int index = 0; index < Lines.Count; index++)
            {
                string line = Lines[index];
                spriteBatch.DrawString(_font, line, new Vector2(bounds.X, bounds.Y + Font.MeasureString(line).Y * index), Color.Black);
            }
            //spriteBatch.DrawString(Font, Text, new Vector2(bounds.X, bounds.Y), tint);

            base.Draw(spriteBatch);
        }
        #endregion
    }
}
