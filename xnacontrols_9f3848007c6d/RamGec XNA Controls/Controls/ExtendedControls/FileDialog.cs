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
/// <summary>FileDialog Class</summary>
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using RamGecXNAControlsExtensions;
using System.IO;

namespace RamGecXNAControls.ExtendedControls
{
    /// <summary>
    /// FileDialog
    /// </summary>
    public class FileDialog
    {
        #region Public Properties
        /// <summary>
        /// Sets or gets currently selected file (full path)
        /// </summary>
        public string Filename
        {
            set
            {
                fileBox.Text = Path.GetFileName(value);
                currentDirectory = Path.GetDirectoryName(value);

                UpdateDirectory();
            }

            get
            {
                return Path.GetFullPath(currentDirectory + "\\" + fileBox.Text);
            }
        }

        /// <summary>
        /// Search pattern for listing files
        /// </summary>
        public string FileFilter = "*.*";
        #endregion

        #region Private Properties
        /// <summary>
        /// FileDialog Window
        /// </summary>
        private Window boxWindow = null;

        private GUIManager guiManager = null;

        private ListBox directoryList = null;
        private ListBox filesList = null;
        private TextBox fileBox = null;

        private string currentDirectory = Environment.CurrentDirectory + "\\";
        #endregion

        #region Events and Delegates
        /// <summary>
        /// Delegate for OnSelectClickCallback
        /// </summary>
        /// <param name="filename">Filename of selected file (including full path)</param>
        public delegate void SelectClickEventHandler(string filename);
        #endregion

        #region Constructors
        /// <summary>
        /// Creates and displayes FileDialog window
        /// </summary>
        /// <param name="location">Location of top-left corner of MessageBox window</param>
        /// <param name="guiManager">GUIManager where this control should be drawed</param>
        /// <param name="text">Message text</param>
        /// <param name="title">MessageBox window title</param>
        /// <param name="onOKClickCallback">Callback method that is called when Sellect button is clicked</param>
        public FileDialog(Point location, GUIManager guiManager, SelectClickEventHandler OnSelectClickCallback)
        {
            this.guiManager = guiManager;

            boxWindow = new Window(Rectangle.Empty, "Select a file", "FileDialog Window");
            boxWindow.TopMost = true;

            // set window size
            boxWindow.Bounds.X = location.X;
            boxWindow.Bounds.Y = location.Y;
            boxWindow.Bounds.Width = 400;
            boxWindow.Bounds.Height = 320;

            // make it in the center of the screen
            if (location == Point.Zero)
            {
                boxWindow.Bounds.X = (guiManager.Game.GraphicsDevice.Viewport.Width / 2) - (boxWindow.Bounds.Width / 2);
                boxWindow.Bounds.Y = (guiManager.Game.GraphicsDevice.Viewport.Height / 2) - (boxWindow.Bounds.Height / 2);
            }

            GroupBox directoryBox = new GroupBox(new Rectangle(10, 40, 160, 200), "Directory");
            boxWindow.Controls.Add(directoryBox);

            directoryList = new ListBox(new Rectangle(10, 20, 140, 170));
            directoryList.ShowIcons = true;
            directoryBox.Controls.Add(directoryList);
            directoryList.OnSelectItem += new ListBox.SelectItemEventHandler(directoryList_OnSelectItem);

            GroupBox filesBox = new GroupBox(new Rectangle(180, 40, 210, 200), "Files");
            boxWindow.Controls.Add(filesBox);
            
            filesList = new ListBox(new Rectangle(10, 20, 190, 170));
            filesList.ShowIcons = true;
            filesBox.Controls.Add(filesList);
            filesList.OnSelectItem += new ListBox.SelectItemEventHandler(filesList_OnSelectItem);

            boxWindow.Controls.Add(new Label(new Rectangle(20, 250, 0, 0), "Filename:"));

            fileBox = new TextBox(new Rectangle(100, 248, 290, 24));
            boxWindow.Controls.Add(fileBox);


            Button closeButton = new Button(new Rectangle(300, 280, 85, 24), "Cancel");
            closeButton.Icon = guiManager.Theme.IconNo;
            closeButton.OnClick += (s) =>
            {
                Close();
            };
            boxWindow.Controls.Add(closeButton);

            Button selectButton = new Button(new Rectangle(210, 280, 85, 24), "Select");
            selectButton.Icon = guiManager.Theme.IconYes;
            selectButton.OnClick += (s) =>
            {
                Close();
                if (OnSelectClickCallback != null)
                    OnSelectClickCallback(Filename);
            };
            boxWindow.Controls.Add(selectButton);

            UpdateDirectory();
        }
        #endregion

        #region Private Methods
        private void filesList_OnSelectItem(GUIControl sender, int item)
        {
            fileBox.Text = filesList.Items[item];
        }

        private void directoryList_OnSelectItem(GUIControl sender, int item)
        {
            currentDirectory = Path.GetFullPath(currentDirectory + directoryList.Items[item] + "\\");
            UpdateDirectory();
        }

        private void UpdateDirectory()
        {
            directoryList.Items.Clear();
            directoryList.SelectedItems.Clear();
            directoryList.Icons.Clear();

            if (currentDirectory != Path.GetPathRoot(currentDirectory))
            {
                directoryList.Items.Add("..");
                directoryList.Icons.Add(guiManager.Theme.IconUp);
            }

            try
            {
                foreach (string directory in Directory.GetDirectories(currentDirectory))
                {
                    try
                    {
                        directoryList.Items.Add(Path.GetFileName(directory));
                        directoryList.Icons.Add(guiManager.Theme.IconFolder);
                    }
                    catch
                    { }
                }
            }
            catch
            { }

            UpdateFilesList();
        }

        private void UpdateFilesList()
        {
            filesList.Items.Clear();
            filesList.SelectedItems.Clear();
            filesList.Icons.Clear();

            try
            {
                foreach (string file in Directory.GetFiles(currentDirectory, FileFilter))
                {
                    try
                    {
                        filesList.Items.Add(Path.GetFileName(file));
                        filesList.Icons.Add(guiManager.Theme.IconFile);
                    }
                    catch
                    { }
                }
            }
            catch
            { }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Show Window (add control to GUIManager)
        /// </summary>
        public void Show()
        {
            guiManager.Controls.Add(boxWindow);

        }

        /// <summary>
        /// Close Window (remove control from GUIManager)
        /// </summary>
        public void Close()
        {
            guiManager.Controls.Remove(boxWindow);
        }
        #endregion
    }
}
