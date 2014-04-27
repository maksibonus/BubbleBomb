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
/// <summary>GUIManager Class</summary>
#endregion

using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RamGecXNAControls
{   
    /// <summary>
    /// GUIManager Class that handles all controls
    /// </summary>
    public class GUIManager
    {
        #region Public Properties
        /// <summary>
        /// Game instance
        /// </summary>
        public Game Game;

        /// <summary>
        /// Current theme
        /// </summary>
        public Themes Theme = new Themes();

        /// <summary>
        /// List of all controls this GUIManager instance handles
        /// </summary>
        public GUIList<GUIControl> Controls;
        #endregion

        #region Private Properties
        /// <summary>
        /// Content Manager
        /// </summary>
        private ContentManager content;

        /// <summary>
        /// List that keeps track of all controls added via LoadControls method
        /// </summary>
        private List<GUIControl> loadedControls;

        /// <summary>
        /// XmlDocument of all saved controls used for SaveControls method
        /// </summary>
        private XmlDocument saveXmlDocument;

        /// <summary>
        /// A first window in Controls list that is set to TopMost (draw it last)
        /// </summary>
        private Window topMostWindow = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a GUIManager instance that handles all GUI Controls
        /// </summary>
        /// <param name="game">Game instance</param>
        /// <param name="themesPath">Location of RamGecXNAControls theme folder</param>
        /// <param name="themeName">Theme Name</param>
        public GUIManager(Game game, string themesPath, string themeName)
        {
            this.Game = game;

            // create a separte content manager
            content = new ContentManager(game.Services);

            LoadTheme(themesPath, themeName);

            Controls = new GUIList<GUIControl>(null, this);

            // debug:
            //Theme.Save("theme.xml");
        }

        /// <summary>
        /// Creates a GUIManager instance that handles all GUI Controls
        /// By default loads "default" theme from "Themes" folder
        /// </summary>
        /// <param name="game">Game instance</param>
        public GUIManager(Game game)
            : this(game, "Themes", "Default")
        {
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Loads a control from XmlNode
        /// </summary>
        /// <param name="parentNode">Parent XmlNode</param>
        /// <param name="parentControl">Parent control (GUIControl or GUIManager)</param>
        private void LoadControl(XmlNode parentNode, object parentControl)
        {
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                GUIControl control = null;

                if (node.Name == "RamGecXNAControls.Button")
                    control = new Button(node);
                if (node.Name == "RamGecXNAControls.Chart")
                    control = new Chart(node);
                if (node.Name == "RamGecXNAControls.CheckBox")
                    control = new CheckBox(node);
                if (node.Name == "RamGecXNAControls.GroupBox")
                    control = new GroupBox(node);
                if (node.Name == "RamGecXNAControls.Image")
                    control = new Image(node);
                if (node.Name == "RamGecXNAControls.Label")
                    control = new Label(node);
                if (node.Name == "RamGecXNAControls.ListBox")
                    control = new ListBox(node);
                if (node.Name == "RamGecXNAControls.Progress")
                    control = new Progress(node);
                if (node.Name == "RamGecXNAControls.RadioButton")
                    control = new RadioButton(node);
                if (node.Name == "RamGecXNAControls.TabControl")
                    control = new TabControl(node);
                if (node.Name == "RamGecXNAControls.TabsContainer")
                    control = new TabsContainer(node);
                if (node.Name == "RamGecXNAControls.TextArea")
                    control = new TextArea(node);
                if (node.Name == "RamGecXNAControls.TextBox")
                    control = new TextBox(node);
                if (node.Name == "RamGecXNAControls.Window")
                    control = new Window(node);

                if (parentControl is GUIControl)
                    (parentControl as GUIControl).Controls.Add(control);
                else if (parentControl is GUIManager)
                    (parentControl as GUIManager).Controls.Add(control);

                // track loaded controls
                loadedControls.Add(control);

                // load the actual control data
                LoadControl(node, control);
            }
        }        
        #endregion

        #region Public Methods
        /// <summary>
        /// Loads and uses specified theme
        /// </summary>
        /// <param name="themesPath">Location of RamGecXNAControls theme folder</param>
        /// <param name="themeName">Theme Name</param>
        public void LoadTheme(string themesPath, string themeName)
        {
            // load theme.xml file
            XmlSerializer deserializer = new XmlSerializer(typeof(Themes));
            using (FileStream fileStream = new FileStream(themesPath + "\\" + themeName + "\\theme.xml", FileMode.Open))
            {
                Theme = (Themes)deserializer.Deserialize(fileStream);
                fileStream.Close();
            }

            // load current skin
            Theme.Skin = content.Load<Texture2D>(themesPath + "\\" + themeName + "\\skin");

            // load fonts
            Theme.DefaultFont = content.Load<SpriteFont>(themesPath + "\\" + themeName + "\\font");
            Theme.DefaultFont.DefaultCharacter = '?';
        }

        /// <summary>
        /// Retrieves the first occurence of a control (of a given name) within children controls
        /// </summary>
        /// <param name="name">Control Name</param>
        /// <returns>GUIControl or null</returns>
        public GUIControl GetControl(string name)
        {   
            GUIControl childrenControl = null;
            foreach (GUIControl control in Controls)
            {
                if (control.Name == name)
                    return control;

                childrenControl = control.GetControl(name);
                if (childrenControl != null)
                    return childrenControl;
            }

            return null;
        }

        /// <summary>
        /// Retrieves the control which bounds are covering set point
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <returns>GUIControl or null</returns>
        public GUIControl GetControl(int x, int y)
        {
            List<GUIControl> all = GetAllControls();

            if (all != null)
                foreach (GUIControl control in all)
                {
                    if (control.InBounds(x, y) && control.IsDrawable)
                        return control;
                }

            return null;
        }
        private Matrix lastMatrix;
        public void SetMatrix(Matrix matrix)
        {
            List<GUIControl> all = GetAllControls();
            lastMatrix = matrix;
            if (all != null)
                foreach (GUIControl control in all)      
                {
                    control.SetMatrix(matrix);
                } 
        }


        /// <summary>
        /// Retrieves a list of all controls in a hierarchy tree
        /// </summary>
        /// <returns></returns>
        public List<GUIControl> GetAllControls()
        {
            List<GUIControl> list = new List<GUIControl>();

            foreach (GUIControl control in Controls)
            {
                list.AddRange(control.GetAllControls());
            }

            return list;
        }

        /// <summary>
        /// Gets a string representing RamGec XNA Control assembly version
        /// </summary>
        /// <returns>Version string</returns>
        public string GetVersion()
        {
            return System.Reflection.Assembly.GetAssembly(typeof(RamGecXNAControls.GUIManager)).GetName().Version.ToString();
        }

        /// <summary>
        /// Save specific control (and all its children) to XML file
        /// </summary>
        /// <param name="path">Path to XML file</param>
        /// <param name="control">Root control</param>
        public void SaveControl(string path, GUIControl control)
        {
            saveXmlDocument = new XmlDocument();
            XmlElement root = saveXmlDocument.CreateElement("RamGecXNAControls.GUIManager");
            root.SetAttribute("Version", GetVersion());

            saveXmlDocument.AppendChild(root);

            root.AppendChild(control.SaveControl(saveXmlDocument));
            //SaveControl(root, control);

            saveXmlDocument.Save(path);
        }

        /// <summary>
        /// Loads controls from a valid XML file
        /// </summary>
        /// <param name="path">Path to the XML File</param>
        public List<GUIControl> LoadControls(string path)
        {
            loadedControls = new List<GUIControl>();

            XmlDocument xml = new XmlDocument();
            xml.Load(path);

            // 0-ed index is a ROOT, which represents GUIManager
            LoadControl(xml.ChildNodes[0], this);

            return loadedControls;
        }
        #endregion

        #region Draw and Update
        /// <summary>
        /// Draws all controls on provided SpriteBatch
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // draw controls in reverse order (by their Z index)
            for (int i = Controls.Count - 1; i >= 0; i--)
            {
                // this is not our TopMost window
                if ((Controls[i] is Window) && (Controls[i] as Window) != topMostWindow)
                {
                    Controls[i].Draw(spriteBatch);
                }
            }

            // finally draw the TopMost window
            if (topMostWindow != null)
                topMostWindow.Draw(spriteBatch);
        }

        /// <summary>
        /// Handles input for all controls
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public void Update(GameTime gameTime)
        {
            bool mouseHandled = false;
            MouseState mouseState = Mouse.GetState();
            GUIControl activeWindow = null;
            topMostWindow = null;

            SetMatrix(lastMatrix);
            // find and update the TopMost window
            for (int i = Controls.Count - 1; i >= 0; i--)
            {   
                if ((Controls[i] is Window) && (Controls[i] as Window).TopMost)
                    topMostWindow = Controls[i] as Window;
            }

            // update the TopMost window
            if (topMostWindow != null && topMostWindow.Bounds.Contains(mouseState.X, mouseState.Y))
            {
                topMostWindow.Update(gameTime);
                mouseHandled = true;
            }

            // update all controls (but only one active window - minus the TopMost one)
            for (int i = 0; i < Controls.Count && !mouseHandled; i++)
            {
                GUIControl control = Controls[i];
                if (!mouseHandled)
                    control.Update(gameTime);

                if (!mouseHandled && control.Bounds.Contains(mouseState.X, mouseState.Y))
                {   
                    mouseHandled = true;

                    if (control is Window)
                    {
                        if (mouseState.LeftButton == ButtonState.Pressed ||
                            mouseState.MiddleButton == ButtonState.Pressed ||
                            mouseState.RightButton == ButtonState.Pressed)
                        {
                            activeWindow = control;
                        }
                    }
                }
            }

            // update Z Index for an active control
            if (activeWindow != null)
            {
                int z = -1;
                foreach (GUIControl control in Controls)
                    if (control.ZIndex > z)
                        z = control.ZIndex;

                activeWindow.ZIndex = z + 1;
                
                Controls.Sort();
            }
        }
        #endregion
    }
}
