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
/// <summary>TextArea Class</summary>
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RamGecXNAControlsExtensions;

// TODO: fix /r/n issue for 2+ line

namespace RamGecXNAControls
{
    /// <summary>
    /// TextArea Control
    /// </summary>
    public class TextArea : GUIControl
    {
        #region Public Properties
        /// <summary>
        /// Warp words. If set to false, strings that do not fit, won't be displayed
        /// </summary>
        public bool WordWrap
        {
            get
            {
                return _wordWrap;
            }
            set
            {
                _wordWrap = value;
                BreakTextToLines();
            }
        }
        private bool _wordWrap = true;

        /// <summary>
        /// TextArea text value
        /// </summary>
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                BreakTextToLines();
            }
        }
        private string _text = String.Empty;

        /// <summary>
        /// Default Font
        /// </summary>
        public SpriteFont Font
        {
            get { return _font ?? Theme.TextAreaFont; }
            set { _font = value; }
        }
        private SpriteFont _font = null;

        /// <summary>
        /// Text color
        /// </summary>
        public Color TextColor
        {
            get { return _textColor ?? Theme.TextAreaColor; }
            set { _textColor = value; }
        }
        private Color? _textColor = null;

        /// <summary>
        /// If set, applies colors to Text as defined in ColorTable.
        /// Invoke colorization with $ character followed by key character.
        /// Example: "$rRed $bBlue text here"
        /// </summary>
        public bool Colorize = false;

        /// <summary>
        /// Stores a char-Color pair. Each char (key) represents which Color (value) should be used
        /// </summary>
        public Dictionary<char, Color> ColorTable = new Dictionary<char, Color>();
        #endregion

        #region Private Properties
        /// <summary>
        /// Stores the drawable representation of Text
        /// </summary>
        private List<string> Lines = new List<string>();

        /// <summary>
        /// Index of the first displayed element
        /// </summary>
        private int scrollIndex
        {
            set { _scrollIndex = value; }
            get { if (_scrollIndex < 0) return 0; return _scrollIndex; }
        }
        private int _scrollIndex = 0;

        /// <summary>
        /// Relative scroller possition
        /// </summary>
        private int scrollerPosition = 0;

        /// <summary>
        /// Cache of Items (rows) height
        /// </summary>
        private int itemHeight = 16;

        /// <summary>
        /// Returns the relative size of a scroller (1 - takes full allowed height (minus margins))
        /// </summary>
        private float scrollerSize
        {
            set { }
            get
            {
                if ((float)displayableItemsCount >= Lines.Count)
                    return 1f;
                return (float)displayableItemsCount / Lines.Count;
            }
        }

        /// <summary>
        /// Checks if ScrollBar is visible
        /// </summary>
        private bool showScrollBar
        {
            set { }
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Calculates how many items can be displayed in the control
        /// </summary>
        private int displayableItemsCount
        {
            set { }
            get
            {
                return (Bounds.Height - 4) / itemHeight;
            }
        }

        private char colorizationChar = '$';
        #endregion

        #region Constructors
        /// <summary>
        /// Creates TextArea control
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control</param>
        /// <param name="name">Control name</param>
        public TextArea(Rectangle bounds, string name)
            : base(bounds, name)
        {
            Init();
        }


        /// <summary>
        /// Creates TextArea control
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control</param>
        public TextArea(Rectangle bounds)
            : this(bounds, String.Empty)
        {
        }

        /// <summary>
        /// Creates TextArea control and loads its data from XmlNode
        /// </summary>
        /// <param name="xmlNode">XmlNode containing control data</param>
        public TextArea(XmlNode xmlNode)
            : base(xmlNode)
        {
            Init();
        }

        private void Init()
        {
            OnMouseMove += new MouseMoveEventHandler(TextArea_OnMouseMove);
        }
        #endregion

        #region Save and Load
        public override void LoadControl(XmlNode xmlNode)
        {
            base.LoadControl(xmlNode);

            if (xmlNode.Attributes["Text"] != null)
                Text = xmlNode.Attributes["Text"].Value;
            if (xmlNode.Attributes["WordWrap"] != null)
                WordWrap = bool.Parse(xmlNode.Attributes["WordWrap"].Value);
            if (xmlNode.Attributes["TextColor"] != null)
                TextColor = xmlNode.Attributes["TextColor"].Value.ToXNAColor();
            if (xmlNode.Attributes["Colorize"] != null)
                Colorize = bool.Parse(xmlNode.Attributes["Colorize"].Value);
            if (xmlNode.Attributes["ColorTable"] != null)
            {
                string[] split = xmlNode.Attributes["ColorTable"].Value.Split(new char[] { ';' });

                for (int i = 0; i < split.Length - 1; i++)
                {
                    char ch = split[i][0];
                    split[i] = split[i].Substring(2);
                    Color cl = split[i].ToXNAColor();
                    ColorTable.Add(ch, cl);
                }
            }

        }

        public override XmlElement SaveControl(XmlDocument xmlDocument)
        {
            XmlElement xmlElement = base.SaveControl(xmlDocument);

            xmlElement.SetAttribute("Text", Text);
            xmlElement.SetAttribute("WordWrap", WordWrap.ToString());
            xmlElement.SetAttribute("TextColor", TextColor.ToXNAString());
            xmlElement.SetAttribute("Colorize", Colorize.ToString());

            if (ColorTable.Count > 0)
            {
                StringBuilder colorTableString = new StringBuilder();
                foreach (KeyValuePair<char, Color> val in ColorTable)
                {
                    colorTableString.Append(val.Key);
                    colorTableString.Append("=");
                    colorTableString.Append(val.Value.ToXNAString());
                    colorTableString.Append(";");
                }
                xmlElement.SetAttribute("ColorTable", colorTableString.ToString());
            }

            return xmlElement;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Structure to store colorization table
        /// </summary>
        struct ColorStruct
        {
            /// <summary>
            /// Token position within a line
            /// </summary>
            public int pos;

            /// <summary>
            /// Color that should be used from now on
            /// </summary>
            public Color color;
        }

        /// <summary>
        /// Creates Colorization table and returns a string with removed tokens
        /// </summary>
        /// <param name="line">Input string containing tokens</param>
        /// <param name="colorizationTable">out. Colorization table</param>
        /// <returns>String without tokens</returns>
        private string CalculateLineColorizationTable(string line, out List<ColorStruct> colorizationTable)
        {
            List<ColorStruct> table = new List<ColorStruct>();

            for (int i = 0; i < line.Length; i++)
            {
                // TODO: ?? fix if part of a token breaks to another line

                // we found a token
                if (line[i] == colorizationChar && i + 1 < line.Length)
                {
                    if (ColorTable.ContainsKey(line[i + 1]))
                    {
                        // add our new token
                        table.Add(new ColorStruct() { pos = i, color = ColorTable[line[i + 1]] });
                        // remove the token
                        line = line.Remove(i, 2);
                    }
                }
            }

            colorizationTable = table;
            return line;
        }

        /// <summary>
        /// Breaks Text to Lines (simplified, no WordWrap/Colorization)
        /// </summary>
        private void BreakTextToLines()
        {
            // update Lines list
            Lines.Clear();
            Lines.AddRange(Text.Split(new string[] { Environment.NewLine, "\n", "\r" }, StringSplitOptions.None));
        }

        /// <summary>
        /// Corrects Lines based on WordWrap/Colorization
        /// </summary>
        /// <param name="bounds"></param>
        private void LineCorrections(Rectangle bounds)
        {
            BreakTextToLines();

            for (int i = 0; i < Lines.Count; i++)
            {
                string trimmedText = Lines[i];
                if (WordWrap)
                {
                    int trimmedIndex = 0;
                    while (Font.MeasureString(trimmedText).X >= bounds.Width - (showScrollBar ? Theme.SkinTextAreaScrollbarMiddle.Width : 0) - 8 &&  // 4 is (left+right) margin
                        trimmedText.Length > 0)
                    {
                        trimmedIndex++;
                        trimmedText = trimmedText.Remove(trimmedText.Length - 1, 1);
                    }

                    string splitLeft = trimmedText;
                    string splitDrop = String.Empty;
                    if (Lines[i].Length - trimmedText.Length != 0)
                        splitDrop = Lines[i].Substring(trimmedText.Length, Lines[i].Length - trimmedText.Length);
                    Lines[i] = trimmedText;

                    if (splitDrop != String.Empty)
                    {
                        if (i == Lines.Count - 1)
                            Lines.Add(splitDrop);
                        else
                        {
                            Lines[i + 1] = splitDrop + Lines[i + 1];
                        }
                    }

                    trimmedText = splitLeft;
                }
                else // no word wrap - just cut the ends
                {
                    // trim the text
                    while (Theme.TextBoxFont.MeasureString(trimmedText).X >= bounds.Width - (showScrollBar ? Theme.SkinTextAreaScrollbarMiddle.Width : 0) - 4 &&  // 4 is (left+right) margin
                        trimmedText.Length > 0)
                    {
                        trimmedText = trimmedText.Remove(trimmedText.Length - 1, 1);
                    }
                    Lines[i] = trimmedText;
                }
            }
        }

        private void TextArea_OnMouseMove(GUIControl sender, MouseState mouseState)
        {
            // Handles scrollbar

            Rectangle bounds = AbsoluteBounds;
            // scrollbar is drawn and mouse is moving over it
            if (showScrollBar && (mouseState.X > bounds.X + bounds.Width - Theme.SkinTextAreaScrollbarMiddle.Width))
            {
                // scrollbar clicked
                if (IsMouseLeftDown)
                {
                    // how much % one item "consume"
                    float steps = 1f / (float)(Lines.Count - displayableItemsCount);

                    // relative mouse position within a scroller
                    int relYPos = mouseState.Y - bounds.Y;

                    float realScrollerSize = (bounds.Height - Theme.SkinTextAreaScrollerTop.Height - Theme.SkinTextAreaScrollerBottom.Height) * scrollerSize;
                    float lowerLimit = Theme.SkinTextAreaScrollerTop.Height + (realScrollerSize / 2);
                    float upperLimit = bounds.Height - Theme.SkinTextAreaScrollerBottom.Height - (realScrollerSize / 2);

                    scrollerPosition = (int)MathHelper.Clamp(relYPos, lowerLimit, upperLimit);

                    float pos = (mouseState.Y - (bounds.Y + lowerLimit)) / (bounds.Height - lowerLimit - (bounds.Height - upperLimit));

                    if (pos < 0)
                        pos = 0;
                    if (pos > 1)
                        pos = 1;

                    // relative normalized mouse click position
                    float pos2 = (float)(Math.Abs(mouseState.Y - bounds.Y) + 0.0001f) / bounds.Height;

                    scrollIndex = (int)Math.Round(pos / steps);
                }
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
            Color tint = Theme.TextAreaTintColor[(int)state] * Transparency;
            itemHeight = (int)Font.MeasureString("j|@").Y;

            #region Skin
            // top-left
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X, bounds.Y, Theme.SkinTextAreaTopLeft.Width, Theme.SkinTextAreaTopLeft.Height),
                Theme.SkinTextAreaTopLeft, tint);

            // top-right
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + bounds.Width - Theme.SkinTextAreaTopRight.Width, bounds.Y, Theme.SkinTextAreaTopRight.Width, Theme.SkinTextAreaTopRight.Height),
                Theme.SkinTextAreaTopRight, tint);

            // top
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + Theme.SkinTextAreaTopLeft.Width, bounds.Y, bounds.Width - Theme.SkinTextAreaTopLeft.Width - Theme.SkinTextAreaTopRight.Width, Theme.SkinTextAreaTop.Height),
                Theme.SkinTextAreaTop, tint);

            // left
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X, bounds.Y + Theme.SkinTextAreaTopLeft.Width, Theme.SkinTextAreaLeft.Width, bounds.Height - Theme.SkinTextAreaTopLeft.Height - Theme.SkinTextAreaBottomLeft.Height),
                Theme.SkinTextAreaLeft, tint);

            // right
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + bounds.Width - Theme.SkinTextAreaRight.Width, bounds.Y + Theme.SkinTextAreaTopRight.Height, Theme.SkinTextAreaRight.Width, bounds.Height - Theme.SkinTextAreaTopRight.Height - Theme.SkinTextAreaBottomRight.Height),
                Theme.SkinTextAreaRight, tint);

            // bottom-left
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X, bounds.Y + bounds.Height - Theme.SkinTextAreaBottomLeft.Height, Theme.SkinTextAreaBottomLeft.Width, Theme.SkinTextAreaBottomLeft.Height),
                Theme.SkinTextAreaBottomLeft, tint);

            // bottom-right
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + bounds.Width - Theme.SkinTextAreaBottomRight.Width, bounds.Y + bounds.Height - Theme.SkinTextAreaBottomRight.Height, Theme.SkinTextAreaBottomRight.Width, Theme.SkinTextAreaBottomRight.Height),
                Theme.SkinTextAreaBottomRight, tint);

            // bottom
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + Theme.SkinTextAreaBottomLeft.Width, bounds.Y + bounds.Height - Theme.SkinTextAreaBottom.Height, bounds.Width - Theme.SkinTextAreaBottomLeft.Width - Theme.SkinTextAreaBottomRight.Width, Theme.SkinTextAreaBottom.Height),
                Theme.SkinTextAreaBottom, tint);

            // middle
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + Theme.SkinTextAreaLeft.Width, bounds.Y + Theme.SkinTextAreaTop.Height, bounds.Width - Theme.SkinTextAreaLeft.Width - Theme.SkinTextAreaRight.Width, bounds.Height - Theme.SkinTextAreaTop.Height - Theme.SkinTextAreaBottom.Height),
                Theme.SkinTextAreaMiddle, tint);


            // scrollbar
            if (showScrollBar)
            {
                // top
                spriteBatch.Draw(Theme.Skin,
                    new Rectangle(bounds.X + bounds.Width - Theme.SkinTextAreaScrollbarTop.Width, bounds.Y, Theme.SkinTextAreaScrollbarTop.Width, Theme.SkinTextAreaScrollbarTop.Height),
                    Theme.SkinTextAreaScrollbarTop, tint);

                // bottom
                spriteBatch.Draw(Theme.Skin,
                    new Rectangle(bounds.X + bounds.Width - Theme.SkinTextAreaScrollbarBottom.Width, bounds.Y + bounds.Height - Theme.SkinTextAreaScrollbarBottom.Height, Theme.SkinTextAreaScrollbarBottom.Width, Theme.SkinTextAreaScrollbarBottom.Height),
                    Theme.SkinTextAreaScrollbarBottom, tint);

                // middle
                spriteBatch.Draw(Theme.Skin,
                    new Rectangle(bounds.X + bounds.Width - Theme.SkinTextAreaScrollbarMiddle.Width, bounds.Y + Theme.SkinTextAreaScrollbarTop.Height, Theme.SkinTextAreaScrollbarMiddle.Width, bounds.Height - Theme.SkinTextAreaScrollbarTop.Height - Theme.SkinTextAreaScrollbarBottom.Height),
                    Theme.SkinTextAreaScrollbarMiddle, tint);

                int scrollerHeight = (int)((float)(bounds.Height - Theme.SkinTextAreaScrollerTop.Height - Theme.SkinTextAreaScrollerBottom.Height) * scrollerSize);
                Rectangle scrollerBounds = new Rectangle(
                    bounds.X + bounds.Width - Theme.SkinTextAreaScrollerMiddle.Width,
                    bounds.Y + scrollerPosition - (scrollerHeight / 2),
                    Theme.SkinTextAreaScrollerMiddle.Width,
                    scrollerHeight);

                // make sure we're within the allowed bounds (this is used once after construction, before user scrolled anything)
                if (scrollerBounds.Y < bounds.Y + Theme.SkinTextAreaScrollerTop.Height)
                    scrollerBounds.Y = bounds.Y + Theme.SkinTextAreaScrollerTop.Height;

                // scroller - top
                spriteBatch.Draw(Theme.Skin,
                    new Rectangle(scrollerBounds.X, scrollerBounds.Y - Theme.SkinTextAreaScrollerTop.Height, Theme.SkinTextAreaScrollerTop.Width, Theme.SkinTextAreaScrollerTop.Height),
                    Theme.SkinTextAreaScrollerTop, tint);

                // scroller - bottom
                spriteBatch.Draw(Theme.Skin,
                    new Rectangle(scrollerBounds.X, scrollerBounds.Y + scrollerBounds.Height, Theme.SkinTextAreaScrollerBottom.Width, Theme.SkinTextAreaScrollerBottom.Height),
                    Theme.SkinTextAreaScrollerBottom, tint);

                // scroller - middle
                spriteBatch.Draw(Theme.Skin,
                    scrollerBounds,
                    Theme.SkinTextAreaScrollerMiddle, tint);
            }

            #endregion

            LineCorrections(bounds);

            // color we going to use (only if Colorization is set)
            Color currentColor = TextColor;

            // rows
            for (int i = scrollIndex; i < displayableItemsCount + scrollIndex && i < Lines.Count; i++)
            {   
                string trimmedText = Lines[i];

                if (Colorize)
                {
                    List<ColorStruct> colorizationTable = null;
                    string line = CalculateLineColorizationTable(trimmedText, out colorizationTable);
                    string textToDraw = String.Empty;

                    int startPos = 0;
                    // X offset where the text should be drawned
                    float xOffset = 0;
                    for (int ti = 0; ti < colorizationTable.Count + 1; ti++)
                    {
                        // cut the left hand side of the string to the token
                        if (ti < colorizationTable.Count)
                            textToDraw = line.Substring(startPos, colorizationTable[ti].pos - startPos);
                        else // it's the last token
                            textToDraw = line.Substring(startPos);

                        spriteBatch.DrawString(Font, textToDraw,
                                new Vector2(bounds.X + 4 + xOffset, bounds.Y + ((i - scrollIndex) * itemHeight)),
                                currentColor * Transparency);

                        // if it's not the last token
                        if (ti < colorizationTable.Count)
                        {
                            xOffset += Font.MeasureString(textToDraw).X;
                            currentColor = colorizationTable[ti].color;
                            startPos = colorizationTable[ti].pos;
                        }
                    }
                }
                else // no colorization, just draw the Lines
                {
                    if (i < displayableItemsCount + scrollIndex)
                        spriteBatch.DrawString(Font, trimmedText,
                                new Vector2(bounds.X + 4, bounds.Y + ((i - scrollIndex) * itemHeight)),
                                TextColor * Transparency);
                }
            }

            base.Draw(spriteBatch);
        }


        #endregion
    }
}
