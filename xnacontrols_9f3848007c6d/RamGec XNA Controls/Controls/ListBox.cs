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
/// <summary>ListBox Class</summary>
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RamGecXNAControlsExtensions;

namespace RamGecXNAControls
{
    /// <summary>
    /// ListBox Control
    /// </summary>
    public class ListBox : GUIControl
    {
        #region Public Properties
        /// <summary>
        /// List of items in the control (one per row)
        /// </summary>
        public List<string> Items = new List<string>();

        /// <summary>
        /// Collection of indices that are currently selected from Items list
        /// </summary>
        public List<int> SelectedItems = new List<int>();

        /// <summary>
        /// Allows multiple selections
        /// </summary>
        public bool MultipleSelection = false;

        /// <summary>
        /// Returns currently selected item's text (or the first item from MultipleSelection list)
        /// </summary>
        public string SelectedString
        {
            get
            {
                if (SelectedItems.Count <= 0)
                    return null;
                return Items[SelectedItems[0]];
            }
            set { }
        }

        /// <summary>
        /// Default Font
        /// </summary>
        public SpriteFont Font
        {
            get { return _font ?? Theme.ListBoxFont; }
            set { _font = value; }
        }
        private SpriteFont _font = null;

        /// <summary>
        /// Item color
        /// </summary>
        public Color ItemColor
        {
            get { return _itemColor ?? Theme.ListBoxItemColor; }
            set { _itemColor = value; }
        }
        private Color? _itemColor = null;

        /// <summary>
        /// Selected Item color
        /// </summary>
        public Color SelectedItemColor
        {
            get { return _selectedItemColor ?? Theme.ListBoxSelectedItemColor; }
            set { _selectedItemColor = value; }
        }
        private Color? _selectedItemColor = null;

        /// <summary>
        /// If set, displays icons near every (if assigned) item in the list
        /// </summary>
        public bool ShowIcons = false;

        /// <summary>
        /// Collection of Icons (each index in the Icons list represents each item in Items list)
        /// </summary>
        public List<Texture2D> Icons = new List<Texture2D>();
        #endregion

        #region Events and Delegates
        /// <summary>
        /// Delegate for handling OnSelectItem events
        /// </summary>
        /// <param name="sender">Control that triggered the event</param>
        /// <param name="item">Selected item index</param>
        public delegate void SelectItemEventHandler(GUIControl sender, int item);
        /// <summary>
        /// Item was selected from the list
        /// </summary>
        public event SelectItemEventHandler OnSelectItem;
        #endregion

        #region Private Properties
        /// <summary>
        /// Index of the first displayed element
        /// </summary>
        private int scrollIndex
        {
            get
            {
                if (_scrollIndex > Items.Count)
                {
                    _scrollIndex = 0;
                    scrollerPosition = 0;
                }
                return _scrollIndex;
            }
            set
            {
                _scrollIndex = value;
            }
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
                return (float)displayableItemsCount / Items.Count;
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
                return displayableItemsCount < Items.Count;
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
        #endregion

        #region Constructors
        /// <summary>
        /// Creates ListBox control
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control</param>
        /// <param name="name">Control name</param>
        public ListBox(Rectangle bounds, string name)
            : base(bounds)
        {
            Name = name;
            Init();
        }

        /// <summary>
        /// Creates ListBox control
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control</param>
        public ListBox(Rectangle bounds)
            : this(bounds, String.Empty)
        {
        }

        /// <summary>
        /// Creates ListBox control and loads its data from XmlNode
        /// </summary>
        /// <param name="xmlNode">XmlNode containing control data</param>
        public ListBox(XmlNode xmlNode)
            : base(xmlNode)
        {
            Init();
        }

        private void Init()
        {
            OnMouseMove += new MouseMoveEventHandler(ListBox_OnMouseMove);
            OnMousePressed += new MousePressedEventHandler(ListBox_OnMousePressed);
        }
        #endregion

        #region Save and Load
        public override void LoadControl(XmlNode xmlNode)
        {
            base.LoadControl(xmlNode);

            if (xmlNode.Attributes["MultipleSelection"] != null)
                MultipleSelection = bool.Parse(xmlNode.Attributes["MultipleSelection"].Value);
            if (xmlNode.Attributes["ItemColor"] != null)
                ItemColor = xmlNode.Attributes["ItemColor"].Value.ToXNAColor();
            if (xmlNode.Attributes["SelectedItemColor"] != null)
                ItemColor = xmlNode.Attributes["SelectedItemColor"].Value.ToXNAColor();
            if (xmlNode.Attributes["Items"] != null)
            {
                string[] items = xmlNode.Attributes["Items"].Value.Split(new char[] { '\t' });

                foreach (string item in items)
                    if (item.Length > 0)
                        Items.Add(item);
            }
        }

        public override XmlElement SaveControl(XmlDocument xmlDocument)
        {
            XmlElement xmlElement = base.SaveControl(xmlDocument);

            xmlElement.SetAttribute("MultipleSelection", MultipleSelection.ToString());
            xmlElement.SetAttribute("ItemColor", ItemColor.ToXNAString());
            xmlElement.SetAttribute("SelectedItemColor", SelectedItemColor.ToXNAString());

            StringBuilder sb = new StringBuilder();
            foreach (string item in Items)
            {
                sb.Append(item);
                sb.Append("\t");
            }
            xmlElement.SetAttribute("Items", sb.ToString());

            return xmlElement;
        }
        #endregion

        #region Private Methods
        private void ListBox_OnMousePressed(GUIControl sender, MouseState mouseState)
        {
            // Handles changing Items

            Rectangle bounds = AbsoluteBounds;
            // press was made on the scrollbox - not on items - exit
            if (showScrollBar && (mouseState.X >= bounds.X + bounds.Width - Theme.SkinListBoxScrollbarMiddle.Width))
                return;

            int pos = mouseState.Y - bounds.Y + 1;

            // item on the list that was clicked
            int item = pos / itemHeight;

            // actual item
            item += scrollIndex;

            // make sure that the click occured within actual items (not on the empty area)
            if (item < Items.Count)
            {
                // for not multiple selections, allow only single item to be selected
                if (MultipleSelection)
                {
                    if (SelectedItems.Contains(item))
                        SelectedItems.Remove(item);
                    else
                        SelectedItems.Add(item);
                }
                else
                {
                    SelectedItems.Clear();
                    SelectedItems.Add(item);
                }

                if (OnSelectItem != null)
                    OnSelectItem(this, item);
            }
        }

        void ListBox_OnMouseMove(GUIControl sender, MouseState mouseState)
        {
            // Handles scrollbar

            Rectangle bounds = AbsoluteBounds;
            // scrollbar is drawn and mouse is moving over it
            if (showScrollBar && (mouseState.X > bounds.X + bounds.Width - Theme.SkinListBoxScrollbarMiddle.Width))
            {
                // scrollbar clicked
                if (IsMouseLeftDown)
                {
                    // how much % one item "consume"
                    float steps = 1f / (float)(Items.Count - displayableItemsCount);

                    // relative mouse position within a scroller
                    int relYPos = mouseState.Y - bounds.Y;

                    float realScrollerSize = (bounds.Height - Theme.SkinListBoxScrollerTop.Height - Theme.SkinListBoxScrollerBottom.Height) * scrollerSize;
                    float lowerLimit = Theme.SkinListBoxScrollerTop.Height + (realScrollerSize / 2);
                    float upperLimit = bounds.Height - Theme.SkinListBoxScrollerBottom.Height - (realScrollerSize / 2);

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

        /// <summary>
        /// Gets an associated icon (or null if not set)
        /// </summary>
        /// <param name="item">Item index</param>
        /// <returns>Icon or null</returns>
        private Texture2D GetIcon(int item)
        {
            if (Icons.Count <= item)
                return null;

            return Icons[item];
        }
        #endregion

        #region Draw and Update
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
                return;

            // cache
            Rectangle bounds = AbsoluteBounds;
            Color tint = Theme.ListBoxTintColor[(int)state] * Transparency;

            // we need to cache those values since we won't have access to Theme instance
            itemHeight = (int)Font.MeasureString("j|@").Y + 2;
            
            #region Skin
            // top-left
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X, bounds.Y, Theme.SkinListBoxTopLeft.Width, Theme.SkinListBoxTopLeft.Height),
                Theme.SkinListBoxTopLeft, tint);

            // top-right
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + bounds.Width - Theme.SkinListBoxTopRight.Width, bounds.Y, Theme.SkinListBoxTopRight.Width, Theme.SkinListBoxTopRight.Height), 
                Theme.SkinListBoxTopRight, tint);

            // top
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + Theme.SkinListBoxTopLeft.Width, bounds.Y, bounds.Width - Theme.SkinListBoxTopLeft.Width - Theme.SkinListBoxTopRight.Width, Theme.SkinListBoxTop.Height),
                Theme.SkinListBoxTop, tint);

            // left
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X, bounds.Y + Theme.SkinListBoxTopLeft.Width, Theme.SkinListBoxLeft.Width, bounds.Height - Theme.SkinListBoxTopLeft.Height - Theme.SkinListBoxBottomLeft.Height),
                Theme.SkinListBoxLeft, tint);

            // right
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + bounds.Width - Theme.SkinListBoxRight.Width, bounds.Y + Theme.SkinListBoxTopRight.Height, Theme.SkinListBoxRight.Width, bounds.Height - Theme.SkinListBoxTopRight.Height - Theme.SkinListBoxBottomRight.Height),
                Theme.SkinListBoxRight, tint);

            // bottom-left
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X, bounds.Y + bounds.Height - Theme.SkinListBoxBottomLeft.Height, Theme.SkinListBoxBottomLeft.Width, Theme.SkinListBoxBottomLeft.Height),
                Theme.SkinListBoxBottomLeft, tint);

            // bottom-right
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + bounds.Width - Theme.SkinListBoxBottomRight.Width, bounds.Y + bounds.Height - Theme.SkinListBoxBottomRight.Height, Theme.SkinListBoxBottomRight.Width, Theme.SkinListBoxBottomRight.Height), 
                Theme.SkinListBoxBottomRight, tint);

            // bottom
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + Theme.SkinListBoxBottomLeft.Width, bounds.Y + bounds.Height - Theme.SkinListBoxBottom.Height, bounds.Width - Theme.SkinListBoxBottomLeft.Width - Theme.SkinListBoxBottomRight.Width, Theme.SkinListBoxBottom.Height),
                Theme.SkinListBoxBottom, tint);

            // middle
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + Theme.SkinListBoxLeft.Width, bounds.Y + Theme.SkinListBoxTop.Height, bounds.Width - Theme.SkinListBoxLeft.Width - Theme.SkinListBoxRight.Width, bounds.Height - Theme.SkinListBoxTop.Height - Theme.SkinListBoxBottom.Height), 
                Theme.SkinListBoxMiddle, tint);


            // scrollbar
            if (showScrollBar)
            {
                // top
                spriteBatch.Draw(Theme.Skin,
                    new Rectangle(bounds.X + bounds.Width - Theme.SkinListBoxScrollbarTop.Width, bounds.Y, Theme.SkinListBoxScrollbarTop.Width, Theme.SkinListBoxScrollbarTop.Height),
                    Theme.SkinListBoxScrollbarTop, tint);

                // bottom
                spriteBatch.Draw(Theme.Skin,
                    new Rectangle(bounds.X + bounds.Width - Theme.SkinListBoxScrollbarBottom.Width, bounds.Y + bounds.Height - Theme.SkinListBoxScrollbarBottom.Height, Theme.SkinListBoxScrollbarBottom.Width, Theme.SkinListBoxScrollbarBottom.Height),
                    Theme.SkinListBoxScrollbarBottom, tint);

                // middle
                spriteBatch.Draw(Theme.Skin,
                    new Rectangle(bounds.X + bounds.Width - Theme.SkinListBoxScrollbarMiddle.Width, bounds.Y + Theme.SkinListBoxScrollbarTop.Height, Theme.SkinListBoxScrollbarMiddle.Width, bounds.Height - Theme.SkinListBoxScrollbarTop.Height - Theme.SkinListBoxScrollbarBottom.Height),
                    Theme.SkinListBoxScrollbarMiddle, tint);
                
                int scrollerHeight = (int)((float)(bounds.Height - Theme.SkinListBoxScrollerTop.Height - Theme.SkinListBoxScrollerBottom.Height) * scrollerSize);
                Rectangle scrollerBounds = new Rectangle(
                    bounds.X + bounds.Width - Theme.SkinListBoxScrollerMiddle.Width,
                    bounds.Y + scrollerPosition - (scrollerHeight / 2),
                    Theme.SkinListBoxScrollerMiddle.Width,
                    scrollerHeight);

                // make sure we're within the allowed bounds (this is used once after construction, before user scrolled anything)
                if (scrollerBounds.Y < bounds.Y + Theme.SkinListBoxScrollerTop.Height)
                    scrollerBounds.Y = bounds.Y + Theme.SkinListBoxScrollerTop.Height;

                // scroller - top
                spriteBatch.Draw(Theme.Skin,
                    new Rectangle(scrollerBounds.X, scrollerBounds.Y - Theme.SkinListBoxScrollerTop.Height, Theme.SkinListBoxScrollerTop.Width, Theme.SkinListBoxScrollerTop.Height),
                    Theme.SkinListBoxScrollerTop, tint);

                // scroller - bottom
                spriteBatch.Draw(Theme.Skin,
                    new Rectangle(scrollerBounds.X, scrollerBounds.Y + scrollerBounds.Height, Theme.SkinListBoxScrollerBottom.Width, Theme.SkinListBoxScrollerBottom.Height),
                    Theme.SkinListBoxScrollerBottom, tint);

                // scroller - middle
                spriteBatch.Draw(Theme.Skin,
                    scrollerBounds,
                    Theme.SkinListBoxScrollerMiddle, tint);
            }

            #endregion

            // items
            for (int i = scrollIndex; i < displayableItemsCount + scrollIndex && i < Items.Count; i++)
            {
                string trimmedText = Items[i];

                int iconWidth = ShowIcons ? (int)(itemHeight * Theme.ListBoxIconScale) + Theme.ListBoxIconSpacing + 4 : 0;

                // trim the text
                while (Font.MeasureString(trimmedText).X + iconWidth >= bounds.Width - (showScrollBar ? Theme.SkinListBoxScrollbarMiddle.Width : 0) - 4 &&  // 4 is (left+right) margin
                    trimmedText.Length > 0)
                {
                    trimmedText = trimmedText.Remove(trimmedText.Length - 1, 1);
                }

                // if selected draw background
                if (SelectedItems.Contains(i))
                {
                    // spacing:
                    // left: 2; top: 1; right: 2; bottom: 1; text-left: 4

                    spriteBatch.Draw(Theme.Skin,
                        new Rectangle(bounds.X + 2, bounds.Y + ((i - scrollIndex) * itemHeight), bounds.Width - (showScrollBar ? Theme.SkinListBoxScrollbarMiddle.Width + 4 : 4), itemHeight),
                        Theme.SkinListBoxSelectedBackground, tint);
                    spriteBatch.DrawString(Font, trimmedText,
                        new Vector2(bounds.X + 4 + iconWidth, 1 + bounds.Y + ((i - scrollIndex) * itemHeight)),
                        SelectedItemColor * Transparency);
                }
                else
                {
                    spriteBatch.DrawString(Font, trimmedText,
                        new Vector2(bounds.X + 4 + iconWidth, 1 + bounds.Y + ((i - scrollIndex) * itemHeight)),
                        ItemColor * Transparency);
                }

                // draw icon
                if (ShowIcons && GetIcon(i) != null)
                {
                    iconWidth = iconWidth - Theme.ListBoxIconSpacing - 4;
                    spriteBatch.Draw(GetIcon(i), new Rectangle(bounds.X + 4, bounds.Y + ((i - scrollIndex) * itemHeight)
                        + (itemHeight / 2) - (iconWidth / 2), iconWidth, iconWidth), tint);
                }
            }

            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (SelectedItems.Count > Items.Count)
                SelectedItems.Clear();

            base.Update(gameTime);
        }
        #endregion
    }
}
