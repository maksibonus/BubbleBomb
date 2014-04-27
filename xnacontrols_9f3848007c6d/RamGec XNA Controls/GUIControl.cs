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
/// <summary>GuiControl Class</summary>
#endregion

using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RamGecXNAControlsExtensions;

// TODO: Global Remove()
// TODO: Save only changed properties

namespace RamGecXNAControls
{   
    /// <summary>
    /// GUIControl class
    /// </summary>
    public abstract class GUIControl
    {
        #region Public Properties
        /// <summary>
        /// Control Theme information
        /// </summary>
        public Themes Theme
        {
            get
            {
                if (_theme == null)
                    return ParentGUIManager.Theme;
                else
                    return _theme;
            }
            set
            {
                _theme = value;
            }
        }
        private Themes _theme = null;

        /// <summary>
        /// GUIManager that handles this control
        /// </summary>
        public GUIManager ParentGUIManager
        {
            get
            {
                if (_parentGUIManager == null && Parent != null)
                {
                    return (_parentGUIManager = Parent.ParentGUIManager); // get and cache
                }
                else
                    return _parentGUIManager;
            }
            set
            {
                _parentGUIManager = value;
            }
        }
        private GUIManager _parentGUIManager = null;

        /// <summary>
        /// Control relative bounds within parent control (location and size)
        /// </summary>
        public Rectangle Bounds;

        /// <summary>
        /// Alpha transparency
        /// 0 - transparent; 1 - opaque
        /// </summary>
        public float Transparency = 1f;

        /// <summary>
        /// Control visibility
        /// </summary>
        public bool Visible = true;

        /// <summary>
        /// Controls Z index (items with higher value will be on top of others that have a lower value)
        /// </summary>
        public int ZIndex = 0;

        /// <summary>
        /// Checks if control has focus
        /// </summary>
        public bool Focused = false;

        /// <summary>
        /// Parent control
        /// </summary>
        public GUIControl Parent = null;

        /// <summary>
        /// List of children controls
        /// </summary>
        public GUIList<GUIControl> Controls;

        /// <summary>
        /// Control Name
        /// </summary>
        public string Name = String.Empty;

        /// <summary>
        /// Determines if user can interact with the control
        /// </summary>
        public bool Enabled = true;
        
        /// <summary>
        /// If set, displays a hint tooltip on right mouse click
        /// </summary>
        public string Hint = String.Empty;

        /// <summary>
        /// Returns absolute viewpoint bounds of a control element
        /// </summary>
        public virtual Rectangle AbsoluteBounds
        {
            get
            {
                if (Parent == null)
                    return Bounds;

                Rectangle result = new Rectangle(Parent.AbsoluteBounds.X + Bounds.X, Parent.AbsoluteBounds.Y + Bounds.Y, Bounds.Width, Bounds.Height);
                return result;

            }
            private set { }
        }

        /// <summary>
        /// Returns if control is actually being drawed on screen
        /// </summary>
        public bool IsDrawable
        {
            get
            {
                if (!Visible)
                    return false;
                if (Parent == null)
                    return Visible;

                return Parent.IsDrawable;
            }
            private set { }
        }

        /// <summary>
        /// Checks if mouse is over control (within bounds)
        /// </summary>
        public bool IsMouseOver
        {
            get
            {
                MouseState mouseState = Mouse.GetState();
                Rectangle result = AbsoluteBounds;
                ConvertToMatrix(ref result.X, ref result.Y);
                ConvertToMatrix(ref result.Width, ref result.Height);

                return (result.Contains(new Point(mouseState.X, mouseState.Y)));
            }
            private set { }
        }

        /// <summary>
        /// Checks if mouse left button is pressed over control (within bounds)
        /// </summary>
        public bool IsMouseLeftDown
        {
            get
            {
                return IsMouseOver && (Mouse.GetState().LeftButton == ButtonState.Pressed);
            }
            private set { }
        }


        /// <summary>
        /// Checks if mouse right button is pressed over control (within bounds)
        /// </summary>
        public bool IsMouseRightDown
        {
            get
            {
                return IsMouseOver && (Mouse.GetState().RightButton == ButtonState.Pressed);
            }
            private set { }
        }
        #endregion

        #region Events and Delegates
        /// <summary>
        /// Delegate for OnMouse events
        /// </summary>
        /// <param name="sender">Control that triggered the event</param>
        /// <param name="mouseState">MouseState</param>
        public delegate void MouseMoveEventHandler(GUIControl sender, MouseState mouseState);
        /// <summary>
        /// Mouse is moving over control element
        /// </summary>
        public event MouseMoveEventHandler OnMouseMove;

        /// <summary>
        /// Delegate for OnMousePressed events
        /// </summary>
        /// <param name="sender">Control that triggered the event</param>
        /// <param name="mouseState">MouseState</param>
        public delegate void MousePressedEventHandler(GUIControl sender, MouseState mouseState);
        /// <summary>
        /// Mouse button has been pressed
        /// </summary>
        public event MousePressedEventHandler OnMousePressed;

        /// <summary>
        /// Delegate for OnMouseReleased events
        /// </summary>
        /// <param name="sender">Control that triggered the event</param>
        /// <param name="mouseState">MouseState</param>
        public delegate void MouseReleasedEventHandler(GUIControl sender, MouseState mouseState);
        /// <summary>
        /// Mouse button has been released
        /// </summary>
        public event MouseReleasedEventHandler OnMouseReleased;

        /// <summary>
        /// Delegate for OnKeyDown events
        /// </summary>
        /// <param name="sender">Control that triggered the event</param>
        /// <param name="keyboardState">KeyboardState</param>
        public delegate void KeyDownEventHandler(GUIControl sender, KeyboardState keyboardState);
        /// <summary>
        /// Key was pressed while control is focused
        /// </summary>
        public event KeyDownEventHandler OnKeyDown;

        /// <summary>
        /// Delegate for OnKeyUp events
        /// </summary>
        /// <param name="sender">Control that triggered the event</param>
        /// <param name="keyboardState">KeyboardState</param>
        public delegate void KeyUpEventHandler(GUIControl sender, KeyboardState keyboardState);
        /// <summary>
        /// Key was released while control is focused
        /// </summary>
        public event KeyUpEventHandler OnKeyUp;

        /// <summary>
        /// Delegate for OnClick events
        /// </summary>
        /// <param name="sender">Control that triggered the event</param>
        public delegate void ClickEventHandler(GUIControl sender);
        /// <summary>
        /// Control was clicked (left mouse button)
        /// </summary>
        public event ClickEventHandler OnClick;
        #endregion

        #region Protected Properties
        /// <summary>
        /// States each control might be assigned
        /// </summary>
        protected enum ControlStates
        {
            Normal = 0,     // regular - no activity
            MouseOver = 1,  // mouse is currently over this control
            Focused = 2,    // control is "selected" (but mouse might have left)
            Pressed = 3     // control is being pressed (left mouse)
        }

        /// <summary>
        /// Control's state (affects tint color)
        /// </summary>
        protected ControlStates state = ControlStates.Normal;
        #endregion

        #region Private Properties
        /// <summary>
        /// Mouse state from the last update cycle
        /// </summary>
        private MouseState _oldMouseState;

        /// <summary>
        /// Keyboard state from the last update cycle
        /// </summary>
        private KeyboardState _oldKeyboardState;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates abstract GUIControl instance
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control</param>
        /// <param name="name">Control name</param>
        public GUIControl(Rectangle bounds, string name)
        {
            Init();
            this.Bounds = bounds;
            this.Name = name;
        }

        /// <summary>
        /// Creates abstract GUIControl instance
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control</param>
        public GUIControl(Rectangle bounds)
            : this(bounds, String.Empty)
        {
        }

        /// <summary>
        /// Creates abstract GUIControl and loads its data from XmlNode
        /// </summary>
        /// <param name="xmlNode">XmlNode containing control data</param>
        public GUIControl(XmlNode xmlNode)
        {
            Init();
            LoadControl(xmlNode);
        }

        private void Init()
        {
            Controls = new GUIList<GUIControl>(this, ParentGUIManager);
            _oldMouseState = Mouse.GetState();
            _oldKeyboardState = Keyboard.GetState();
        }
        #endregion

        #region Save and Load
        /// <summary>
        /// Loads control data from XmlNode
        /// </summary>
        /// <param name="xmlNode">XmlNode containing control data</param>
        public virtual void LoadControl(XmlNode xmlNode)
        {
            // parse Rectangle data from XML
            Bounds = xmlNode.Attributes["Bounds"].Value.ToXNARectangle();

            // generic GUIControl data
            if (xmlNode.Attributes["Name"] != null)
                Name = xmlNode.Attributes["Name"].Value;
            if (xmlNode.Attributes["Visible"] != null)
                Visible = bool.Parse(xmlNode.Attributes["Visible"].Value);
            if (xmlNode.Attributes["Enabled"] != null)
                Enabled = bool.Parse(xmlNode.Attributes["Enabled"].Value);
            if (xmlNode.Attributes["Transparency"] != null)
                Transparency = float.Parse(xmlNode.Attributes["Transparency"].Value);
            if (xmlNode.Attributes["Hint"] != null)
                Hint = xmlNode.Attributes["Hint"].Value;
        }

        /// <summary>
        /// Returns control data (including all children controls) as XmlNode
        /// </summary>
        /// <param name="xmlDocument">Root XmlDocument where XmlElements should be created</param>
        /// <returns>XmlNode containing control data</returns>
        public virtual XmlElement SaveControl(XmlDocument xmlDocument)
        {
            XmlElement xmlElement = xmlDocument.CreateElement(this.ToString());

            // save generic GUIControl data
            xmlElement.SetAttribute("Name", Name);
            xmlElement.SetAttribute("Bounds", Bounds.ToString());
            xmlElement.SetAttribute("Visible", Visible.ToString());
            xmlElement.SetAttribute("Enabled", Enabled.ToString());
            xmlElement.SetAttribute("Transparency", Transparency.ToString());
            xmlElement.SetAttribute("Hint", Hint);

            // save all children controls
            foreach (GUIControl ctrl in Controls)
                xmlElement.AppendChild(ctrl.SaveControl(xmlDocument));

            return xmlElement;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Retrieves the first occurence of a control (of a given name) within children controls
        /// </summary>
        /// <param name="name">Control Name</param>
        /// <returns>GUIControl or null if control was not found</returns>
        public GUIControl GetControl(string name)
        {
            if (name == Name)
                return this;

            GUIControl childrenControl = null;
            foreach (GUIControl control in Controls)
            {
                childrenControl = control.GetControl(name);
                if (childrenControl != null)
                    return childrenControl;
            }

            return null;
        }

        /// <summary>
        /// Retrieves a list of all controls within its children controls
        /// </summary>
        /// <returns>List of all containing controls</returns>
        public List<GUIControl> GetAllControls()
        {
            List<GUIControl> list = new List<GUIControl>();

            list.Add(this);
            foreach (GUIControl control in Controls)
            {
                list.AddRange(control.GetAllControls());
            }

            return list;
        }

        private Matrix matrix;
        public void SetMatrix(Matrix matrix)
        {
            this.matrix = matrix;
        }

        /// <summary>
        /// Checks if location is within current control bounds AND not within its children bounds
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <returns>If location is ONLY on this control</returns>
        public bool InBounds(int x, int y)
        {
            bool finish = false;
            // perform a recursive search
            bool res = InBounds(x, y, out finish);

            Rectangle resultRect = AbsoluteBounds;
            ConvertToMatrix(ref resultRect.X, ref resultRect.Y);
            ConvertToMatrix(ref resultRect.Width, ref resultRect.Height);
            if (!resultRect.Contains(x, y) || !Visible)
                return false;
            // active control was found - this cannot be an active control then
            if (finish == true)
                return false;
            return true;
        }

        /// <summary>
        /// Recursive InBounds search
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="finish">Checks if active control was found</param>
        /// <returns>Returns if this is an active control</returns>
        public bool InBounds(int x, int y, out bool finish)
        {
            finish = false;
            foreach (GUIControl control in Controls)
            {
                if (finish || control.InBounds(x, y, out finish))
                {
                    finish = true;
                    return false;
                }
            }
            Rectangle result= AbsoluteBounds;
            ConvertToMatrix(ref result.X, ref result.Y);
            ConvertToMatrix(ref result.Width, ref result.Height);
            if (!result.Contains(x, y) || !Visible)
                return false;
            return true;
        }

        private void ConvertToMatrix(ref int x, ref int y)
        {
            Matrix temp = new Matrix();
            temp.M11 = x;
            temp.M22 = y;
            temp.M33 = 1;
            temp.M44 = 1;
            Matrix resultMatrix = temp * matrix;
            x = (int)resultMatrix.M11;
            y = (int)resultMatrix.M22;
        }

        private void ConvertToMatrixInvert(ref int x, ref int y)
        {
            Matrix temp = new Matrix();
            temp.M11 = x;
            temp.M22 = y;
            temp.M33 = 1;
            temp.M44 = 1;

            Matrix inverted;
            Matrix.Invert(ref matrix, out inverted);

            Matrix resultMatrix = temp * inverted;
            x = (int)resultMatrix.M11;
            y = (int)resultMatrix.M22;
        }
        /// <summary>
        /// Checks if location is within current control bounds AND not within its children bounds
        /// </summary>
        /// <param name="location">Target location</param>
        /// <returns>If location is ONLY on this control</returns>
        public bool InBounds(Point location)
        {
            return InBounds(location.X, location.Y);
        }
        #endregion

        #region Draw and Update
        /// <summary>
        /// Draws the tooltip with hint
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch</param>
        /// <param name="theme">Current theme</param>
        private void DrawHint(SpriteBatch spriteBatch)
        {
            // TODO: Drawing Z-Index (overlaping of other controls)
            Rectangle controlBounds = AbsoluteBounds;
            Color tint = Theme.HintTintColor * Theme.HintTransparency;
            Vector2 textSize = Theme.HintFont.MeasureString(Hint);
            
            // tooltip's body (without the tail) bounds
            Rectangle bounds = new Rectangle(
                controlBounds.X,
                controlBounds.Y - (int)textSize.Y - Theme.SkinHintTail.Height - Theme.SkinHintTop.Height - Theme.SkinHintBottom.Height, // above the control
                (int)textSize.X + Theme.SkinHintLeft.Width + Theme.SkinHintRight.Width,
                (int)textSize.Y + Theme.SkinHintTop.Height + Theme.SkinHintBottom.Height);

            // align tooltip with the right side of the control
            bounds.X += controlBounds.Width - bounds.Width;

            // top-left
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X, bounds.Y, Theme.SkinHintTopLeft.Width, Theme.SkinHintTopLeft.Height),
                Theme.SkinHintTopLeft, tint);

            // top-right
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + bounds.Width - Theme.SkinHintTopRight.Width, bounds.Y, Theme.SkinHintTopRight.Width, Theme.SkinHintTopRight.Height),
                Theme.SkinHintTopRight, tint);

            // top
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + Theme.SkinHintTopLeft.Width, bounds.Y, bounds.Width - Theme.SkinHintTopLeft.Width - Theme.SkinHintTopRight.Width, Theme.SkinHintTop.Height),
                Theme.SkinHintTop, tint);

            // left
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X, bounds.Y + Theme.SkinHintTopLeft.Width, Theme.SkinHintLeft.Width, bounds.Height - Theme.SkinHintTopLeft.Height - Theme.SkinHintBottomLeft.Height),
                Theme.SkinHintLeft, tint);

            // right
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + bounds.Width - Theme.SkinHintRight.Width, bounds.Y + Theme.SkinHintTopRight.Height, Theme.SkinHintRight.Width, bounds.Height - Theme.SkinHintTopRight.Height - Theme.SkinHintBottomRight.Height),
                Theme.SkinHintRight, tint);

            // bottom-left
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X, bounds.Y + bounds.Height - Theme.SkinHintBottomLeft.Height, Theme.SkinHintBottomLeft.Width, Theme.SkinHintBottomLeft.Height),
                Theme.SkinHintBottomLeft, tint);

            // bottom-right
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + bounds.Width - Theme.SkinHintBottomRight.Width, bounds.Y + bounds.Height - Theme.SkinHintBottomRight.Height, Theme.SkinHintBottomRight.Width, Theme.SkinHintBottomRight.Height),
                Theme.SkinHintBottomRight, tint);

            // bottom
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + Theme.SkinHintBottomLeft.Width, bounds.Y + bounds.Height - Theme.SkinHintBottom.Height, bounds.Width - Theme.SkinHintBottomLeft.Width - Theme.SkinHintBottomRight.Width, Theme.SkinHintBottom.Height),
                Theme.SkinHintBottom, tint);

            // middle
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + Theme.SkinHintLeft.Width, bounds.Y + Theme.SkinHintTop.Height, bounds.Width - Theme.SkinHintLeft.Width - Theme.SkinHintRight.Width, bounds.Height - Theme.SkinHintTop.Height - Theme.SkinHintBottom.Height),
                Theme.SkinHintMiddle, tint);

            // tail
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + bounds.Width - Theme.SkinHintBottomRight.Width - Theme.SkinHintTail.Width, bounds.Y + bounds.Height - 1, Theme.SkinHintTail.Width, Theme.SkinHintTail.Height),
                Theme.SkinHintTail, tint);

            // draw the hint string
            spriteBatch.DrawString(Theme.DefaultFont, Hint, new Vector2(bounds.X + Theme.SkinHintLeft.Width, bounds.Y + Theme.SkinHintTop.Height), Theme.HintColor * Theme.HintTransparency);
        }

        /// <summary>
        /// Handles control drawing
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to use for drawing</param>
        /// <param name="theme">Theme</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
                return;

            // call draw for all children controls
            foreach (GUIControl control in Controls)
                control.Draw(spriteBatch);

            // Hint
            if (Hint != String.Empty)
            {
                MouseState mouseState = Mouse.GetState();
                if (IsMouseRightDown && InBounds(new Point(mouseState.X, mouseState.Y)))
                    DrawHint(spriteBatch);
            }
        }

        /// <summary>
        /// Handles control updating
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public virtual void Update(GameTime gameTime)
        {
            if (!Visible)
                return;

            // clear state (and assign it later in the same loop)
            state = ControlStates.Normal;

            // perform actions only on enabled controls
            if (Enabled)
            {
                // check for mouse events
                MouseState mouseState = Mouse.GetState();

                // remove the focus (assume some other control got it)
                if (mouseState.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released)
                    Focused = false;

                // if no events occured - restore Focused state
                if (Focused)
                    state = ControlStates.Focused;

                // make sure this event applies only to this control (not its children)
                if (InBounds(new Point(mouseState.X, mouseState.Y)))
                {   
                    // mouse is over the control - adjust the state
                    state = ControlStates.MouseOver;

                    // if mouse is pressed, state needs to be adjusted even more
                    if (mouseState.LeftButton == ButtonState.Pressed)
                        state = ControlStates.Pressed;

                    if ((mouseState.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released) ||
                        (mouseState.RightButton == ButtonState.Pressed && _oldMouseState.RightButton == ButtonState.Released) ||
                        (mouseState.MiddleButton == ButtonState.Pressed && _oldMouseState.MiddleButton == ButtonState.Released))
                    {
                        if (OnMousePressed != null)
                            OnMousePressed(this, mouseState);

                        // gain focus
                        Focused = true;
                    }

                
                    if ((mouseState.LeftButton == ButtonState.Released && _oldMouseState.LeftButton == ButtonState.Pressed) ||
                        (mouseState.RightButton == ButtonState.Released && _oldMouseState.RightButton == ButtonState.Pressed) ||
                        (mouseState.MiddleButton == ButtonState.Released && _oldMouseState.MiddleButton == ButtonState.Pressed))
                    {
                        if (OnMouseReleased != null)
                            OnMouseReleased(this, mouseState);

                        // clicks are registered only if LEFT mouse button is pressed
                        if (OnClick != null && _oldMouseState.LeftButton == ButtonState.Pressed)
                            OnClick(this);
                    }

                    if (OnMouseMove != null)
                        OnMouseMove(this, mouseState);
                }

                // store this state as an old one
                _oldMouseState = mouseState;


                // keyboard
                KeyboardState keyboardState = Keyboard.GetState();
                if (Focused)
                {
                    if (OnKeyDown != null)
                        if (keyboardState.GetPressedKeys().Length > 0)// && _oldKeyboardState.GetPressedKeys().Length == 0)
                        {   
                            OnKeyDown(this, keyboardState);
                        }

                    if (OnKeyUp != null)
                        if (_oldKeyboardState.GetPressedKeys().Length > 0)// && keyboardState.GetPressedKeys().Length == 0)
                        {
                            OnKeyUp(this, keyboardState);
                        }
                }
                _oldKeyboardState = keyboardState;

                foreach (GUIControl control in Controls)
                    control.Update(gameTime);
            }
        }
        #endregion
    }
}
