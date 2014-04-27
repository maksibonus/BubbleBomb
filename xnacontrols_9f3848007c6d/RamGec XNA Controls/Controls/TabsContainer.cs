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
/// <summary>TabsContainer Class</summary>
#endregion

using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RamGecXNAControlsExtensions;

namespace RamGecXNAControls
{
    /// <summary>
    /// TabsContainer Control
    /// </summary>
    public class TabsContainer : GUIControl
    {
        #region Public Properties
        /// <summary>
        /// Index of a currently active tab
        /// </summary>
        public int CurrentTab = 0;

        /// <summary>
        /// Default Font
        /// </summary>
        public SpriteFont Font
        {
            get { return _font ?? Theme.TabsFont; }
            set { _font = value; }
        }
        private SpriteFont _font = null;

        /// <summary>
        /// Text color
        /// </summary>
        public Color ActiveTabColor
        {
            get { return _activeTabColor ?? Theme.TabColor; }
            set { _activeTabColor = value; }
        }
        private Color? _activeTabColor = null;

        /// <summary>
        /// Text color
        /// </summary>
        public Color InactiveTabColor
        {
            get { return _inactiveTabColor ?? Theme.TabColorInactive; }
            set { _inactiveTabColor = value; }
        }
        private Color? _inactiveTabColor = null;
        #endregion

        #region Private Properties
        /// <summary>
        /// cache of tab rectangles (for handling mouse events)
        /// </summary>
        private List<Rectangle> tabRectangles = new List<Rectangle>();

        /// <summary>
        /// Tab index that mouse is currently over (or -1 if none)
        /// </summary>
        private int mouseOverTab = -1;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates TabsContainer Control
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control</param>
        /// <param name="name">Control name</param>
        public TabsContainer(Rectangle bounds, string name)
            : base(bounds)
        {
            Name = name;
            Init();
        }

        /// <summary>
        /// Creates TabsContainer Control
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control</param>
        public TabsContainer(Rectangle bounds)
            : this(bounds, String.Empty)
        {
        }

        /// <summary>
        /// Creates TabsContainer control and loads its data from XmlNode
        /// </summary>
        /// <param name="xmlNode">XmlNode containing control data</param>
        public TabsContainer(XmlNode xmlNode)
            : base(xmlNode)
        {
            Init();
        }

        private void Init()
        {
            // checks for switching between tabs
            OnMousePressed += new MousePressedEventHandler(TabsContainer_OnMousePressed);
            // checks if mouse over a tab
            OnMouseMove += new MouseMoveEventHandler(TabsContainer_OnMouseMove);
        }
        #endregion

        #region Save and Load
        public override void LoadControl(XmlNode xmlNode)
        {
            base.LoadControl(xmlNode);

            if (xmlNode.Attributes["ActiveTabColor"] != null)
                ActiveTabColor = xmlNode.Attributes["ActiveTabColor"].Value.ToXNAColor();
            if (xmlNode.Attributes["InactiveTabColor"] != null)
                InactiveTabColor = xmlNode.Attributes["InactiveTabColor"].Value.ToXNAColor();
            if (xmlNode.Attributes["CurrentTab"] != null)
                CurrentTab = Int32.Parse(xmlNode.Attributes["CurrentTab"].Value);
            
        }

        public override XmlElement SaveControl(XmlDocument xmlDocument)
        {
            XmlElement xmlElement = base.SaveControl(xmlDocument);

            xmlElement.SetAttribute("ActiveTabColor", ActiveTabColor.ToXNAString());
            xmlElement.SetAttribute("InactiveTabColor", InactiveTabColor.ToXNAString());
            xmlElement.SetAttribute("CurrentTab", CurrentTab.ToString());

            return xmlElement;
        }
        #endregion

        #region Private Methods
        private void TabsContainer_OnMouseMove(GUIControl sender, MouseState mouseState)
        {
            // handles mouse over a tab events

            mouseOverTab = -1;
            for (int i = 0; i < tabRectangles.Count; i++)
            {
                if (tabRectangles[i].Contains(new Point(mouseState.X, mouseState.Y)))
                {
                    mouseOverTab = i;
                }
            }
        }

        private void TabsContainer_OnMousePressed(GUIControl sender, MouseState mouseState)
        {
            // handles changing tabs

            for (int i = 0; i < tabRectangles.Count; i++)
            {
                if (tabRectangles[i].Contains(new Point(mouseState.X, mouseState.Y)))
                {
                    CurrentTab = i;
                }
            }
        }

        /// <summary>
        /// Calculates Tabs rectangles (location and size)
        /// </summary>
        /// <param name="Theme">Currently used Theme</param>
        void CalculateTabRectangles()
        {
            tabRectangles.Clear();

            int prevWidth = 0;
            for (int i = 0; i < Controls.Count; i++)
            {
                int width = Theme.SkinTabLeft.Width + Theme.SkinTabRight.Width + (int)Theme.DefaultFont.MeasureString((Controls[i] as TabControl).Text).X;
                tabRectangles.Add(new Rectangle(AbsoluteBounds.X + prevWidth, AbsoluteBounds.Y, width + 8, Theme.SkinTabMiddle.Height));
                prevWidth += width + Theme.TabSpacing;
            }
        }
        #endregion

        #region Draw and Update
        public override void Draw(SpriteBatch spriteBatch)
        {
            // TODO: recalculate only when needed
            CalculateTabRectangles();

            // cache
            Rectangle bounds = AbsoluteBounds;
            Color tint = Theme.TabsContainerTintColor[(int)state] * Transparency;

            // top overlaps by 1 pixel to hide the border
            // top
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + Theme.SkinTabsContainerLeft.Width, bounds.Y + Theme.SkinTabMiddle.Height - 1, bounds.Width - Theme.SkinTabsContainerLeft.Width - Theme.SkinTabsContainerRight.Width, Theme.SkinTabsContainerTop.Height),
                Theme.SkinTabsContainerTop, tint);

            // left
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X, bounds.Y + Theme.SkinTabMiddle.Height - 1, Theme.SkinTabsContainerLeft.Width, bounds.Height - Theme.SkinTabMiddle.Height + 1),
                Theme.SkinTabsContainerLeft, tint);

            // left-bottom
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X, bounds.Y + bounds.Height, Theme.SkinTabsContainerBottomLeft.Width, Theme.SkinTabsContainerBottomLeft.Height),
                Theme.SkinTabsContainerBottomLeft, tint);

            // rigth
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + bounds.Width - Theme.SkinTabsContainerRight.Width, bounds.Y + Theme.SkinTabMiddle.Height + Theme.SkinTabsContainerTopRight.Height - 1, Theme.SkinTabsContainerRight.Width, bounds.Height - Theme.SkinTabMiddle.Height - Theme.SkinTabsContainerTopRight.Height + 1), 
                Theme.SkinTabsContainerRight, tint);

            // rigth-top
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + bounds.Width - Theme.SkinTabsContainerTopRight.Width, bounds.Y + Theme.SkinTabMiddle.Height - 1, Theme.SkinTabsContainerTopRight.Width, Theme.SkinTabsContainerTopRight.Height),
                Theme.SkinTabsContainerTopRight, tint);

            // right-bottom
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + bounds.Width - Theme.SkinTabsContainerBottomRight.Width, bounds.Y + bounds.Height, Theme.SkinTabsContainerBottomRight.Width, Theme.SkinTabsContainerBottomRight.Height), 
                Theme.SkinTabsContainerBottomRight, tint);

            // middle
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + Theme.SkinTabsContainerLeft.Width, bounds.Y + Theme.SkinTabMiddle.Height + Theme.SkinTabsContainerTop.Height - 1, bounds.Width - Theme.SkinTabsContainerLeft.Width - Theme.SkinTabsContainerRight.Width, bounds.Height - Theme.SkinTabMiddle.Height - 1),
                Theme.SkinTabsContainerMiddle, tint);

            // bottom
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + Theme.SkinTabsContainerBottom.Width, bounds.Y + bounds.Height, bounds.Width - Theme.SkinTabsContainerBottomLeft.Width - Theme.SkinTabsContainerBottomRight.Width, Theme.SkinTabsContainerBottom.Height),
                Theme.SkinTabsContainerBottom, tint);


            int spacing = 0;
            for (int i = 0; i < Controls.Count; i++)
            {
                // check for all children controls and handle only TabControls
                if (!(Controls[i] is TabControl))
                    continue;

                string text = (Controls[i] as TabControl).Text;
                int textLength = (int)Font.MeasureString(text).X;

                // active tab
                if (i == CurrentTab)
                {   
                    // left
                    spriteBatch.Draw(Theme.Skin, 
                        new Rectangle(bounds.X + spacing, bounds.Y, Theme.SkinCurrentTabLeft.Width, Theme.SkinCurrentTabLeft.Height), 
                        Theme.SkinCurrentTabLeft, tint);

                    // middle
                    spriteBatch.Draw(Theme.Skin,
                        new Rectangle(bounds.X + spacing + 8, bounds.Y, textLength + 8, Theme.SkinCurrentTabMiddle.Height), 
                        Theme.SkinCurrentTabMiddle, tint);

                    // rigth
                    spriteBatch.Draw(Theme.Skin,
                        new Rectangle(bounds.X + spacing + 16 + textLength, bounds.Y, Theme.SkinCurrentTabRight.Width, Theme.SkinCurrentTabRight.Height),
                        Theme.SkinCurrentTabRight, tint);

                    // text
                    spriteBatch.DrawString(Font, text, new Vector2(bounds.X + 10 + spacing, bounds.Y + 4), ActiveTabColor * Transparency);

                    spacing += textLength + 16;
                }
                // inactive tab
                else
                {
                    // left
                    spriteBatch.Draw(Theme.Skin,
                        new Rectangle(bounds.X + spacing, bounds.Y, Theme.SkinTabLeft.Width, Theme.SkinTabLeft.Height),
                        Theme.SkinTabLeft, (i == mouseOverTab) ? Theme.TabTintColor : tint);

                    // middle
                    spriteBatch.Draw(Theme.Skin,
                        new Rectangle(bounds.X + spacing + Theme.SkinTabLeft.Width, bounds.Y, textLength + 8, Theme.SkinTabMiddle.Height),
                        Theme.SkinTabMiddle, (i == mouseOverTab) ? Theme.TabTintColor : tint);

                    // right
                    spriteBatch.Draw(Theme.Skin,
                        new Rectangle(bounds.X + spacing + Theme.SkinTabLeft.Width + textLength + 8, bounds.Y, Theme.SkinTabRight.Width, Theme.SkinTabRight.Height), 
                        Theme.SkinTabRight, (i == mouseOverTab) ? Theme.TabTintColor : tint);

                    // text
                    spriteBatch.DrawString(Font, text, new Vector2(bounds.X + 10 + spacing, bounds.Y + 4), InactiveTabColor * Transparency);

                    spacing += textLength + 16;
                }

                spacing += Theme.TabSpacing;
            }


            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            // reset the mouse over tab
            mouseOverTab = -1;

            // reset visibility and bounds (if control is resized)
            foreach (GUIControl control in Controls)
            {
                if (control is TabControl)
                {
                    control.Visible = false;

                    // update width/height, since TabsContainer might be resized
                    control.Bounds.Width = Bounds.Width;
                    control.Bounds.Height = Bounds.Height;
                }
            }

            // show controls only from an active tab
            if (Controls.Count > CurrentTab)
                Controls[CurrentTab].Visible = true;

            base.Update(gameTime);
        }
        #endregion
    }
}
