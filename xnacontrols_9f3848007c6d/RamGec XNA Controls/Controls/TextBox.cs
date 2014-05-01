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
/// <summary>TextBox Class</summary>
#endregion

using System;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RamGecXNAControlsExtensions;

// todo: fix moving cursor left/right

namespace RamGecXNAControls
{
    /// <summary>
    /// TextBox Control
    /// </summary>
    public class TextBox : GUIControl
    {
        #region Public Properties
        /// <summary>
        /// TextBox Text
        /// </summary>
        public string Text
        {
            get { return _text; }
            set {
                _text = value; 
                if (OnTextChanged != null) 
                    OnTextChanged(this);
            }
        }
        private string _text = String.Empty;

        /// <summary>
        /// User can only enter numbers
        /// </summary>
        public bool NumbersOnly = false;

        /// <summary>
        /// User can only enter letters, digits and space
        /// </summary>
        public bool AlphaNumericOnly = false;

        /// <summary>
        /// If set to true, all input is masked and displayed as a collection of the same character
        /// </summary>
        public bool PasswordField = false;

        /// <summary>
        /// Character (or string) that masks the input for PasswordField
        /// </summary>
        public string PasswordMask = "*";

        /// <summary>
        /// Key that triggers OnSubmit events. Default = ENTER
        /// </summary>
        public Keys SubmitKey = Keys.Enter;

        /// <summary>
        /// Default Font
        /// </summary>
        public SpriteFont Font
        {
            get { return _font ?? Theme.TextBoxFont; }
            set { _font = value; }
        }
        private SpriteFont _font = null;

        /// <summary>
        /// Text color
        /// </summary>
        public Color TextColor
        {
            get { return _textColor ?? Theme.TextBoxColor; }
            set { _textColor = value; }
        }
        private Color? _textColor = null;

        /// <summary>
        /// Blinking character (or string) when control is active. Default: |
        /// </summary>
        public string CursorChar = "|";
        #endregion

        #region Events and Delegates
        /// <summary>
        /// Delegate for handling OnSubmit events
        /// </summary>
        /// <param name="sender">Control that triggered the event</param>
        public delegate void SubmitEventHandler(GUIControl sender);
        /// <summary>
        /// Submit key was pressed
        /// </summary>
        public event SubmitEventHandler OnSubmit;

        /// <summary>
        /// Delegate for handling OnTextChanged event
        /// </summary>
        /// <param name="sender">Control that triggered the event</param>
        public delegate void ChangedTextEventHandler(GUIControl sender);
        /// <summary>
        /// Text was changed
        /// </summary>
        public event ChangedTextEventHandler OnTextChanged;
        #endregion

        #region Private Properties
        /// <summary>
        /// location of the cursor
        /// </summary>
        private int cursor
        {
            get
            {
                if (_cursor < 0)
                    _cursor = 0;
                if (_cursor > Text.Length)
                    _cursor = Text.Length;

                return _cursor;
            }
            set { _cursor = value; }
        }
        private int _cursor = 0;

        /// <summary>
        /// time interval between cursor blinks
        /// </summary>
        private int lastBlink = 0;

        /// <summary>
        /// checks if carret should be drawn this frame
        /// </summary>
        private bool showCursor = true;

        /// <summary>
        /// old keyboard state for tracking repeated keystrokes
        /// </summary>
        private KeyboardState oldKeyboardState;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates TextBox control
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control</param>
        /// <param name="text">TextBox text</param>
        /// <param name="name">Control name</param>
        public TextBox(Rectangle bounds, string text, string name)
            : base(bounds)
        {
            Text = text;
            Name = name;
            Init();
        }

        /// <summary>
        /// Creates TextBox control
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control</param>
        /// <param name="text">TextBox text</param>
        public TextBox(Rectangle bounds, string text)
            : this(bounds, text, String.Empty)
        {
        }

        /// <summary>
        /// Creates TextBox control
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control</param>
        public TextBox(Rectangle bounds)
            : this(bounds, String.Empty)
        {
        }

        /// <summary>
        /// Creates TextBox control and loads its data from XmlNode
        /// </summary>
        /// <param name="xmlNode">XmlNode containing control data</param>
        public TextBox(XmlNode xmlNode)
            : base(xmlNode)
        {
            Init();
        }

        private void Init()
        {
            oldKeyboardState = Keyboard.GetState();

            // handles keyboard input
            OnKeyDown += new KeyDownEventHandler(TextBox_OnKeyDown);

            // stores old keyboard input
            OnKeyUp += (s, e) => { oldKeyboardState = e; };
        }
        #endregion

        #region Save and Load
        public override void LoadControl(XmlNode xmlNode)
        {
            base.LoadControl(xmlNode);

            if (xmlNode.Attributes["Text"] != null)
                Text = xmlNode.Attributes["Text"].Value;
            if (xmlNode.Attributes["NumbersOnly"] != null)
                NumbersOnly = bool.Parse(xmlNode.Attributes["NumbersOnly"].Value);
            if (xmlNode.Attributes["AlphaNumericOnly"] != null)
                AlphaNumericOnly = bool.Parse(xmlNode.Attributes["AlphaNumericOnly"].Value);
            if (xmlNode.Attributes["PasswordField"] != null)
                PasswordField = bool.Parse(xmlNode.Attributes["PasswordField"].Value);
            if (xmlNode.Attributes["PasswordMask"] != null)
                PasswordMask = xmlNode.Attributes["PasswordMask"].Value;
            if (xmlNode.Attributes["TextColor"] != null)
                TextColor = xmlNode.Attributes["TextColor"].Value.ToXNAColor();
            if (xmlNode.Attributes["CursorChar"] != null)
                CursorChar = xmlNode.Attributes["CursorChar"].Value;
        }

        public override XmlElement SaveControl(XmlDocument xmlDocument)
        {
            XmlElement xmlElement = base.SaveControl(xmlDocument);

            xmlElement.SetAttribute("Text", Text);
            xmlElement.SetAttribute("NumbersOnly", NumbersOnly.ToString());
            xmlElement.SetAttribute("AlphaNumericOnly", AlphaNumericOnly.ToString());
            xmlElement.SetAttribute("PasswordField", PasswordField.ToString());
            xmlElement.SetAttribute("PasswordMask", PasswordMask);
            xmlElement.SetAttribute("TextColor", TextColor.ToXNAString());
            xmlElement.SetAttribute("CursorChar", CursorChar);

            return xmlElement;
        }
        #endregion

        #region Private Methods
        private void TextBox_OnKeyDown(GUIControl sender, KeyboardState keyboardState)
        {
            // handles keyboard input
            foreach (Keys key in keyboardState.GetPressedKeys())
            {
                bool shift = keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift);

                string sKey = key.ToString();

                if (key == Keys.Space)
                    sKey = " ";

                // NumPad numbers
                sKey = sKey.Replace("NumPad", "");

                // Numbers
                if (sKey.Length == 2)
                    sKey = sKey.Replace("D", "");

                #region Keys

                if (!AlphaNumericOnly)
                {
                    if (key == Keys.OemTilde) sKey = "`";
                    if (key == Keys.OemTilde && shift) sKey = "~";

                    if (key == Keys.D1 && shift) sKey = "!";
                    if (key == Keys.D2 && shift) sKey = "@";
                    if (key == Keys.D3 && shift) sKey = "#";
                    if (key == Keys.D4 && shift) sKey = "$";
                    if (key == Keys.D5 && shift) sKey = "%";
                    if (key == Keys.D6 && shift) sKey = "^";
                    if (key == Keys.D7 && shift) sKey = "&";
                    if (key == Keys.D8 && shift) sKey = "*";
                    if (key == Keys.D9 && shift) sKey = "(";
                    if (key == Keys.D0 && shift) sKey = ")";

                    if (key == Keys.OemMinus) sKey = "-";
                    if (key == Keys.OemMinus && shift) sKey = "_";

                    if (key == Keys.OemPlus) sKey = "=";
                    if (key == Keys.OemPlus && shift) sKey = "+";

                    if (key == Keys.Subtract) sKey = "-";
                    if (key == Keys.Add) sKey = "+";
                    if (key == Keys.Multiply) sKey = "*";
                    if (key == Keys.Divide) sKey = "/";
                    if (key == Keys.Decimal) sKey = ".";

                    if (key == Keys.OemOpenBrackets) sKey = "[";
                    if (key == Keys.OemOpenBrackets && shift) sKey = "{";

                    if (key == Keys.OemCloseBrackets) sKey = "]";
                    if (key == Keys.OemCloseBrackets && shift) sKey = "}";

                    if (key == Keys.OemPipe) sKey = "\\";
                    if (key == Keys.OemPipe && shift) sKey = "|";

                    if (key == Keys.OemSemicolon) sKey = ";";
                    if (key == Keys.OemSemicolon && shift) sKey = ":";

                    if (key == Keys.OemQuotes) sKey = "'";
                    if (key == Keys.OemQuotes && shift) sKey = "\"";

                    if (key == Keys.OemComma) sKey = ",";
                    if (key == Keys.OemComma && shift) sKey = "<";

                    if (key == Keys.OemPeriod) sKey = ".";
                    if (key == Keys.OemPeriod && shift) sKey = ">";

                    if (key == Keys.OemQuestion) sKey = "/";
                    if (key == Keys.OemQuestion && shift) sKey = "?";
                }
                
                #endregion

                // if it's not a special key
                if (sKey.Length == 1)
                {
                    if ((NumbersOnly && Char.IsNumber(sKey[0])) || NumbersOnly == false)
                    {
                        // case-sensitive
                        if (shift)
                            sKey = sKey.ToUpper();
                        else
                            sKey = sKey.ToLower();

                        // new key press - handle
                        if (oldKeyboardState.IsKeyUp(key))
                        {
                            Text = Text.Insert(cursor, sKey);
                            cursor++;
                        }
                    }
                }

                // backspace
                if (key == Keys.Back)
                    if (oldKeyboardState.IsKeyUp(key))
                        if (cursor > 0 && cursor == Text.Length) // we need this since cursor is auto-updated
                            Text = Text.Remove(cursor - 1, 1);
                        else if (cursor > 0)
                        {
                            Text = Text.Remove(cursor - 1, 1);
                            cursor--;
                        }

                // delete
                if (key == Keys.Delete)
                    if (oldKeyboardState.IsKeyUp(key))
                        if (cursor < Text.Length)
                        {
                            Text = Text.Remove(cursor, 1);
                        }

                // submit
                if (key == SubmitKey)
                    if (oldKeyboardState.IsKeyUp(key))
                        if (OnSubmit != null)
                            OnSubmit(this);

                // carret movement keys
                if (key == Keys.Left)
                    if (oldKeyboardState.IsKeyUp(key))
                        cursor--;
                if (key == Keys.Right)
                    if (oldKeyboardState.IsKeyUp(key))
                        cursor++;
                if (key == Keys.Home)
                    cursor = 0;
                if (key == Keys.End)
                    cursor = Text.Length;
            }

            oldKeyboardState = keyboardState;
        }
        #endregion

        #region Draw and Update
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
                return;

            // cache
            Rectangle bounds = AbsoluteBounds;
            Color tint = Theme.TextBoxTintColor[(int)state] * Transparency;

            #region Skin
            // left
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X, bounds.Y, Theme.SkinTextBoxLeft.Width, bounds.Height),
                Theme.SkinTextBoxLeft, tint);

            // right
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + bounds.Width - Theme.SkinTextBoxRight.Width, bounds.Y, Theme.SkinTextBoxRight.Width, bounds.Height),
                Theme.SkinTextBoxRight, tint);

            // top
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + Theme.SkinTextBoxLeft.Width, bounds.Y, bounds.Width - Theme.SkinTextBoxLeft.Width - Theme.SkinTextBoxRight.Width, Theme.SkinTextBoxTop.Height),
                Theme.SkinTextBoxTop, tint);

            // bottom
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + Theme.SkinTextBoxLeft.Width, bounds.Y + bounds.Height - Theme.SkinTextBoxBottom.Height, bounds.Width - Theme.SkinTextBoxLeft.Width - Theme.SkinTextBoxRight.Width, Theme.SkinTextBoxBottom.Height),
                Theme.SkinTextBoxBottom, tint);

            // middle
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + Theme.SkinTextBoxLeft.Width, bounds.Y + Theme.SkinTextBoxTop.Height, bounds.Width - Theme.SkinTextBoxLeft.Width - Theme.SkinTextBoxRight.Width, bounds.Height - Theme.SkinTextBoxTop.Height - Theme.SkinTextBoxBottom.Height),
                Theme.SkinTextBoxMiddle, tint);
            #endregion

            // text
            string visibleText = Text;

            // if it's a password fied, mask all input
            if (PasswordField)
            {
                visibleText = String.Empty;
                for (int i = 0; i < Text.Length; i++)
                    visibleText += PasswordMask;
            }

            // cursor pos within visibleText
            int tmpCarret = cursor;

            // trim from the left
            while (Font.MeasureString(visibleText).X > bounds.Width - 8)
            {
                if (tmpCarret == 0)
                    break;
                visibleText = visibleText.Remove(0, 1);
                tmpCarret--;
            }

            // trim from the right
            while (Font.MeasureString(visibleText).X > bounds.Width - 8)
            {
                visibleText = visibleText.Remove(visibleText.Length - 1, 1);
            }

            // draw text
            spriteBatch.DrawString(Font, visibleText,
                new Vector2(bounds.X + 4, bounds.Y + 2), // margins: (2, 4, 2, 4)
                TextColor * Transparency);

            // draw the cursor
            if (Focused)
            {
                if (showCursor)
                {
                    float cursorPos = bounds.X + 3; // left-margin minus 1

                    if (tmpCarret != 0)
                    {
                        cursorPos += Font.MeasureString(visibleText.Substring(0, tmpCarret)).X;
                    }
                    spriteBatch.DrawString(Font, CursorChar,
                        new Vector2(cursorPos, bounds.Y + 2),
                        TextColor * Transparency);
                }
            }

            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            // blink the "I" carret
            if (Focused)
            {
                lastBlink += gameTime.ElapsedGameTime.Milliseconds;
                if (lastBlink > 500)
                {
                    lastBlink = 0;
                    showCursor = !showCursor;
                }
            }

            base.Update(gameTime);
        }
        #endregion
    }
}
