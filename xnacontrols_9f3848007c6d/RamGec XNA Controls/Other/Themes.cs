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
/// <summary>Themes Class</summary>
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RamGecXNAControls
{   
    /// <summary>
    /// Themes and Skins class
    /// </summary>
    [Serializable]
    public class Themes
    {
        #region Skins
        #region Button Skin
        public Rectangle SkinButtonLeft = new Rectangle(0, 40, 10, 22);
        public Rectangle SkinButtonRight = new Rectangle(40, 40, 10, 22);
        public Rectangle SkinButtonMiddle = new Rectangle(10, 40, 30, 22);
        #endregion

        #region Chart Skin
        public Rectangle SkinChartTopLeft = new Rectangle(130, 0, 4, 4);
        public Rectangle SkinChartTopRight = new Rectangle(146, 0, 4, 4);
        public Rectangle SkinChartTop = new Rectangle(134, 0, 12, 4);
        public Rectangle SkinChartLeft = new Rectangle(130, 4, 4, 12);
        public Rectangle SkinChartRight = new Rectangle(146, 4, 4, 12);
        public Rectangle SkinChartBottomLeft = new Rectangle(130, 16, 4, 4);
        public Rectangle SkinChartBottomRight = new Rectangle(146, 16, 4, 4);
        public Rectangle SkinChartBottom = new Rectangle(134, 16, 12, 4);
        public Rectangle SkinChartMiddle = new Rectangle(134, 4, 12, 12);
        #endregion

        #region CheckBox Skin
        public Rectangle SkinCheckBoxChecked = new Rectangle(70, 0, 20, 20);
        public Rectangle SkinCheckBox = new Rectangle(50, 0, 20, 20);
        #endregion

        #region GroupBox Skin
        public Rectangle SkinGroupBoxTopLeft = new Rectangle(130, 0, 4, 4);
        public Rectangle SkinGroupBoxTopRight = new Rectangle(146, 0, 4, 4);
        public Rectangle SkinGroupBoxTop = new Rectangle(134, 0, 12, 4);
        public Rectangle SkinGroupBoxLeft = new Rectangle(130, 4, 4, 12);
        public Rectangle SkinGroupBoxRight = new Rectangle(146, 4, 4, 12);
        public Rectangle SkinGroupBoxBottomLeft = new Rectangle(130, 16, 4, 4);
        public Rectangle SkinGroupBoxBottomRight = new Rectangle(146, 16, 4, 4);
        public Rectangle SkinGroupBoxBottom = new Rectangle(134, 16, 12, 4);
        public Rectangle SkinGroupBoxMiddle = new Rectangle(134, 4, 12, 12);
        #endregion

        #region ListBox Skin
        public Rectangle SkinListBoxTopLeft = new Rectangle(30, 0, 4, 4);
        public Rectangle SkinListBoxTopRight = new Rectangle(46, 0, 4, 4);
        public Rectangle SkinListBoxTop = new Rectangle(34, 0, 12, 4);
        public Rectangle SkinListBoxLeft = new Rectangle(30, 4, 4, 12);
        public Rectangle SkinListBoxRight = new Rectangle(46, 4, 4, 12);
        public Rectangle SkinListBoxBottomLeft = new Rectangle(30, 16, 4, 4);
        public Rectangle SkinListBoxBottomRight = new Rectangle(46, 16, 4, 4);
        public Rectangle SkinListBoxBottom = new Rectangle(34, 16, 12, 4);
        public Rectangle SkinListBoxMiddle = new Rectangle(34, 4, 12, 12);
        public Rectangle SkinListBoxScrollbarTop = new Rectangle(110, 20, 14, 10);
        public Rectangle SkinListBoxScrollbarBottom = new Rectangle(110, 40, 14, 10);
        public Rectangle SkinListBoxScrollbarMiddle = new Rectangle(110, 30, 14, 8);
        public Rectangle SkinListBoxScrollerTop = new Rectangle(130, 20, 14, 10);
        public Rectangle SkinListBoxScrollerBottom = new Rectangle(130, 40, 14, 10);
        public Rectangle SkinListBoxScrollerMiddle = new Rectangle(130, 30, 14, 10);
        public Rectangle SkinListBoxSelectedBackground = new Rectangle(60, 70, 10, 20);
        #endregion

        #region Progress Skin
        public Rectangle SkinProgressLeft = new Rectangle(30, 0, 4, 20);
        public Rectangle SkinProgressRight = new Rectangle(46, 0, 4, 20);
        public Rectangle SkinProgressTop = new Rectangle(34, 0, 12, 4);
        public Rectangle SkinProgressBottom = new Rectangle(34, 16, 12, 4);
        public Rectangle SkinProgressMiddle = new Rectangle(34, 4, 12, 12);
        public Rectangle SkinProgressIndicator = new Rectangle(50, 70, 10, 20);
        #endregion

        #region RadioButton Skin
        public Rectangle SkinRadioButtonChecked = new Rectangle(90, 0, 20, 20);
        public Rectangle SkinRadioButton = new Rectangle(110, 0, 20, 20);
        #endregion

        #region Tabs and TabsContainer Skin
        public Rectangle SkinTabsContainerLeft = new Rectangle(50, 50, 10, 10);
        public Rectangle SkinTabsContainerBottomLeft = new Rectangle(50, 60, 10, 10);
        public Rectangle SkinTabsContainerRight = new Rectangle(100, 50, 10, 10);
        public Rectangle SkinTabsContainerTopRight = new Rectangle(100, 40, 10, 10);
        public Rectangle SkinTabsContainerBottomRight = new Rectangle(100, 60, 10, 10);
        public Rectangle SkinTabsContainerBottom = new Rectangle(60, 60, 10, 10);
        public Rectangle SkinTabsContainerMiddle = new Rectangle(60, 50, 10, 10);
        public Rectangle SkinTabsContainerTop = new Rectangle(75, 40, 1, 10);

        public Rectangle SkinCurrentTabLeft = new Rectangle(50, 20, 8, 26);
        public Rectangle SkinCurrentTabMiddle = new Rectangle(58, 20, 4, 26);
        public Rectangle SkinCurrentTabRight = new Rectangle(62, 20, 8, 26);
        public Rectangle SkinTabLeft = new Rectangle(80, 20, 8, 26);
        public Rectangle SkinTabMiddle = new Rectangle(88, 20, 4, 26);
        public Rectangle SkinTabRight = new Rectangle(92, 20, 8, 26);
        #endregion

        #region TextArea Skin
        public Rectangle SkinTextAreaTopLeft = new Rectangle(30, 0, 4, 4);
        public Rectangle SkinTextAreaTopRight = new Rectangle(46, 0, 4, 4);
        public Rectangle SkinTextAreaTop = new Rectangle(34, 0, 12, 4);
        public Rectangle SkinTextAreaLeft = new Rectangle(30, 4, 4, 12);
        public Rectangle SkinTextAreaRight = new Rectangle(46, 4, 4, 12);
        public Rectangle SkinTextAreaBottomLeft = new Rectangle(30, 16, 4, 4);
        public Rectangle SkinTextAreaBottomRight = new Rectangle(46, 16, 4, 4);
        public Rectangle SkinTextAreaBottom = new Rectangle(34, 16, 12, 4);
        public Rectangle SkinTextAreaMiddle = new Rectangle(34, 4, 12, 12);
        public Rectangle SkinTextAreaScrollbarTop = new Rectangle(110, 20, 14, 10);
        public Rectangle SkinTextAreaScrollbarBottom = new Rectangle(110, 40, 14, 10);
        public Rectangle SkinTextAreaScrollbarMiddle = new Rectangle(110, 30, 14, 8);
        public Rectangle SkinTextAreaScrollerTop = new Rectangle(130, 20, 14, 10);
        public Rectangle SkinTextAreaScrollerBottom = new Rectangle(130, 40, 14, 10);
        public Rectangle SkinTextAreaScrollerMiddle = new Rectangle(130, 30, 14, 10);
        public Rectangle SkinTextAreaSelectedBackground = new Rectangle(60, 70, 10, 20);
        #endregion

        #region TextBox Skin
        public Rectangle SkinTextBoxLeft = new Rectangle(30, 0, 4, 20);
        public Rectangle SkinTextBoxRight = new Rectangle(46, 0, 4, 20);
        public Rectangle SkinTextBoxTop = new Rectangle(34, 0, 12, 4);
        public Rectangle SkinTextBoxBottom = new Rectangle(34, 16, 12, 4);
        public Rectangle SkinTextBoxMiddle = new Rectangle(34, 4, 12, 12);
        #endregion

        #region Window Skin
        public Rectangle SkinWindowTitleLeft = new Rectangle(0, 0, 10, 24);
        public Rectangle SkinWindowTitleRight = new Rectangle(20, 0, 10, 24);
        public Rectangle SkinWindowTitle = new Rectangle(10, 0, 10, 24);
        public Rectangle SkinWindowLeft = new Rectangle(0, 24, 10, 8);
        public Rectangle SkinWindowBottomLeft = new Rectangle(0, 32, 10, 8);
        public Rectangle SkinWindowRight = new Rectangle(20, 24, 10, 8);
        public Rectangle SkinWindowBottomRight = new Rectangle(20, 32, 10, 8);
        public Rectangle SkinWindowBottom = new Rectangle(10, 32, 10, 8);
        public Rectangle SkinWindowBody = new Rectangle(10, 24, 10, 8);
        #endregion

        #region Hint Skin
        public Rectangle SkinHintTopLeft = new Rectangle(0, 90, 10, 10);
        public Rectangle SkinHintTopRight = new Rectangle(40, 90, 10, 10);
        public Rectangle SkinHintBottomLeft = new Rectangle(0, 120, 10, 10);
        public Rectangle SkinHintBottomRight = new Rectangle(40, 120, 10, 10);
        public Rectangle SkinHintTop = new Rectangle(10, 90, 10, 10);
        public Rectangle SkinHintBottom = new Rectangle(10, 120, 10, 10);
        public Rectangle SkinHintLeft = new Rectangle(0, 100, 10, 20);
        public Rectangle SkinHintRight = new Rectangle(40, 100, 10, 20);
        public Rectangle SkinHintMiddle = new Rectangle(10, 100, 10, 20);
        public Rectangle SkinHintTail = new Rectangle(20, 129, 20, 20);
        #endregion
        #endregion

        #region Theme Properties
        private int iconSize = 48;
        private Rectangle SkinIconsStartingPosition = new Rectangle(0, 150, 0, 0);

        #region Button
        [XmlIgnore()]
        private SpriteFont _buttonFont = null;
        [XmlIgnore()]
        public SpriteFont ButtonFont { get { return _buttonFont ?? DefaultFont; } set { _buttonFont = value; } }
        public Color ButtonColor = new Color(36, 36, 36);
        public Color[] ButtonTintColor = new Color[4] { new Color(255, 255, 255), new Color(240, 240, 250), new Color(255, 255, 255), new Color(220, 220, 250) };
        public int ButtonIconSpacing = 4;
        public float ButtomIconScale = 0.6f;
        #endregion

        #region Chart
        [XmlIgnore()]
        private SpriteFont _chartFont = null;
        [XmlIgnore()]
        public SpriteFont ChartFont { get { return _chartFont ?? DefaultFont; } set { _chartFont = value; } }
        public Color ChartColor = new Color(64, 64, 70);
        public Color[] ChartTintColor = new Color[4] { new Color(255, 255, 255), new Color(255, 255, 255), new Color(255, 255, 255), new Color(255, 255, 255) };
        public int ChartMargin = 20;
        #endregion

        #region CheckBox
        [XmlIgnore()]
        private SpriteFont _checkBoxFont = null;
        [XmlIgnore()]
        public SpriteFont CheckBoxFont { get { return _checkBoxFont ?? DefaultFont; } set { _checkBoxFont = value; } }
        public Color CheckBoxColor = new Color(64, 64, 70);
        public Color[] CheckBoxTintColor = new Color[4] { new Color(255, 255, 255), new Color(255, 255, 255), new Color(255, 255, 255), new Color(255, 255, 255) };
        public int CheckBoxSpacing = 6;
        #endregion

        #region GroupBox
        [XmlIgnore()]
        private SpriteFont _groupBoxFont = null;
        [XmlIgnore()]
        public SpriteFont GroupBoxFont { get { return _groupBoxFont ?? DefaultFont; } set { _groupBoxFont = value; } }
        public Color GroupBoxTitleColor = new Color(64, 64, 70);
        public Color[] GroupBoxTintColor = new Color[4] { new Color(255, 255, 255), new Color(255, 255, 255), new Color(255, 255, 255), new Color(255, 255, 255) };
        #endregion

        #region Label
        [XmlIgnore()]
        private SpriteFont _labelFont = null;
        [XmlIgnore()]
        public SpriteFont LabelFont { get { return _labelFont ?? DefaultFont; } set { _labelFont = value; } }
        public Color[] LabelTintColor = new Color[4] { new Color(64, 64, 70), new Color(64, 64, 70), new Color(64, 64, 70), new Color(64, 64, 70) };
        #endregion

        #region ListBox
        [XmlIgnore()]
        private SpriteFont _listBoxFont = null;
        [XmlIgnore()]
        public SpriteFont ListBoxFont { get { return _listBoxFont ?? DefaultFont; } set { _listBoxFont = value; } }
        public Color ListBoxItemColor = new Color(36, 36, 36);
        public Color ListBoxSelectedItemColor = new Color(36, 36, 36);
        public Color[] ListBoxTintColor = new Color[4] { new Color(255, 255, 255), new Color(255, 255, 255), new Color(255, 255, 255), new Color(255, 255, 255) };
        public int ListBoxIconSpacing = 4;
        public float ListBoxIconScale = 0.6f;
        #endregion

        #region Progress
        [XmlIgnore()]
        public SpriteFont _progressFont = null;
        [XmlIgnore()]
        public SpriteFont ProgressFont { get { return _progressFont ?? DefaultFont; } set { _progressFont = value; } }
        public Color ProgressColor = new Color(64, 64, 64);
        public Color[] ProgressTintColor = new Color[4] { new Color(255, 255, 255), new Color(255, 255, 255), new Color(255, 255, 255), new Color(255, 255, 255) };
        #endregion

        #region RadioButton
        [XmlIgnore()]
        private SpriteFont _radioButtonFont = null;
        [XmlIgnore()]
        public SpriteFont RadioButtonFont { get { return _radioButtonFont ?? DefaultFont; } set { _radioButtonFont = value; } }
        public Color RadioButtonColor = new Color(64, 64, 64);
        public Color[] RadioButtonTintColor = new Color[4] { new Color(255, 255, 255), new Color(255, 255, 255), new Color(255, 255, 255), new Color(255, 255, 255) };
        public int RadioButtonSpacing = 6;
        #endregion

        #region TabsContainer
        [XmlIgnore()]
        private SpriteFont _tabsFont = null;
        [XmlIgnore()]
        public SpriteFont TabsFont { get { return _tabsFont ?? DefaultFont; } set { _tabsFont = value; } }
        public Color TabColor = new Color(64, 64, 64);
        public Color TabColorInactive = new Color(245, 245, 245);
        public Color TabTintColor = new Color(230, 240, 250);
        public Color[] TabsContainerTintColor = new Color[4] { new Color(255, 255, 255), new Color(255, 255, 255), new Color(255, 255, 255), new Color(255, 255, 255) };
        public int TabSpacing = 16;
        #endregion

        #region TextArea
        [XmlIgnore()]
        private SpriteFont _textAreaFont = null;
        [XmlIgnore()]
        public SpriteFont TextAreaFont { get { return _textAreaFont ?? DefaultFont; } set { _textAreaFont = value; } }
        public Color TextAreaColor = new Color(36, 36, 36);
        public Color[] TextAreaTintColor = new Color[4] { new Color(255, 255, 255), new Color(255, 255, 255), new Color(255, 255, 255), new Color(255, 255, 255) };
        public int TextAreaItemHeight = 16;
        #endregion

        #region TextBox
        [XmlIgnore()]
        private SpriteFont _textBoxFont = null;
        [XmlIgnore()]
        public SpriteFont TextBoxFont { get { return _textBoxFont ?? DefaultFont; } set { _textBoxFont = value; } }
        public Color TextBoxColor = new Color(64, 64, 64);
        public Color[] TextBoxTintColor = new Color[4] { new Color(255, 255, 255), new Color(210, 215, 225), new Color(220, 230, 240), new Color(230, 230, 230) };
        #endregion

        #region Window
        [XmlIgnore()]
        private SpriteFont _windowFont = null;
        [XmlIgnore()]
        public SpriteFont WindowFont { get { return _windowFont ?? DefaultFont; } set { _windowFont = value; } }
        public Color WindowTitleColor = new Color(72, 77, 84);
        public Color[] WindowTintColor = new Color[4] { new Color(255, 255, 255), new Color(255, 255, 255), new Color(255, 255, 255), new Color(255, 255, 255) };
        #endregion

        #region Hint
        [XmlIgnore()]
        private SpriteFont _hintFont = null;
        [XmlIgnore()]
        public SpriteFont HintFont { get { return _hintFont ?? DefaultFont; } set { _hintFont = value; } }
        public Color HintColor = new Color(64, 64, 72);
        public Color HintTintColor = new Color(255, 255, 255);
        public float HintTransparency = 0.9f;
        #endregion
        #endregion

        #region Public Properties
        [XmlIgnore()]
        /// <summary>
        /// Default font used for all controls (unless specified differently)
        /// </summary>
        public SpriteFont DefaultFont;

        [XmlIgnore()]
        /// <summary>
        /// Skin texture for a current theme
        /// </summary>
        public Texture2D Skin
        {
            get
            {
                return _skin;
            }
            set
            {
                _skin = value;
                CreateDefaultIcons();
            }
        }
        private Texture2D _skin = null;

        #region Icons
        /// <summary>
        /// File Icon
        /// </summary>
        [XmlIgnore()]
        public Texture2D IconFile = null;

        /// <summary>
        /// Folder Icon
        /// </summary>
        [XmlIgnore()]
        public Texture2D IconFolder = null;

        /// <summary>
        /// Yes Icon
        /// </summary>
        [XmlIgnore()]
        public Texture2D IconYes = null;

        /// <summary>
        /// No Icon
        /// </summary>
        [XmlIgnore()]
        public Texture2D IconNo = null;

        /// <summary>
        /// Up Icon
        /// </summary>
        [XmlIgnore()]
        public Texture2D IconUp = null;

        /// <summary>
        /// Save Icon
        /// </summary>
        [XmlIgnore()]
        public Texture2D IconSave = null;

        /// <summary>
        /// Star Icon
        /// </summary>
        [XmlIgnore()]
        public Texture2D IconStar = null;

        /// <summary>
        /// Warning icon
        /// </summary>
        [XmlIgnore()]
        public Texture2D IconWarning = null;

        /// <summary>
        /// Question Icon
        /// </summary>
        [XmlIgnore()]
        public Texture2D IconQuestion = null;

        /// <summary>
        /// Home Icon
        /// </summary>
        [XmlIgnore()]
        public Texture2D IconHome = null;
        #endregion
        #endregion

        #region Contructors
        /// <summary>
        /// Creates Themes instance
        /// </summary>
        public Themes()
        {   
        }

        /// <summary>
        /// Creates Themes instance
        /// </summary>
        /// <param name="skin">Skin Texture</param>
        /// <param name="defaultFont">Default Font</param>
        public Themes(Texture2D skin, SpriteFont defaultFont)
        {
            Skin = skin;
            DefaultFont = defaultFont;
        }
        #endregion

        #region Private Methods
        private void CreateDefaultIcons()
        {
            Color[] originalData = new Color[Skin.Width * Skin.Height];
            Skin.GetData<Color>(originalData);

            // row 1
            // IconFile
            int offset = 0;
            Color[] copyData = new Color[iconSize * iconSize];
            for (int y = 0; y < iconSize; y++)
                for (int x = 0; x < iconSize; x++)
                    copyData[y * iconSize + x] = originalData[(SkinIconsStartingPosition.Y + y) * Skin.Width + (SkinIconsStartingPosition.X + x) + offset];
            IconFile = new Texture2D(Skin.GraphicsDevice, iconSize, iconSize, false, SurfaceFormat.Color);
            IconFile.SetData<Color>(copyData);

            // IconFolder
            offset += 50;
            for (int y = 0; y < iconSize; y++)
                for (int x = 0; x < iconSize; x++)
                    copyData[y * iconSize + x] = originalData[(SkinIconsStartingPosition.Y + y) * Skin.Width + (SkinIconsStartingPosition.X + x) + offset];
            IconFolder = new Texture2D(Skin.GraphicsDevice, iconSize, iconSize, false, SurfaceFormat.Color);
            IconFolder.SetData<Color>(copyData);

            // IconYes
            offset += 50;
            for (int y = 0; y < iconSize; y++)
                for (int x = 0; x < iconSize; x++)
                    copyData[y * iconSize + x] = originalData[(SkinIconsStartingPosition.Y + y) * Skin.Width + (SkinIconsStartingPosition.X + x) + offset];
            IconYes = new Texture2D(Skin.GraphicsDevice, iconSize, iconSize, false, SurfaceFormat.Color);
            IconYes.SetData<Color>(copyData);

            // IconNo
            offset += 50;
            for (int y = 0; y < iconSize; y++)
                for (int x = 0; x < iconSize; x++)
                    copyData[y * iconSize + x] = originalData[(SkinIconsStartingPosition.Y + y) * Skin.Width + (SkinIconsStartingPosition.X + x) + offset];
            IconNo = new Texture2D(Skin.GraphicsDevice, iconSize, iconSize, false, SurfaceFormat.Color);
            IconNo.SetData<Color>(copyData);

            // IconUp
            offset += 50;
            for (int y = 0; y < iconSize; y++)
                for (int x = 0; x < iconSize; x++)
                    copyData[y * iconSize + x] = originalData[(SkinIconsStartingPosition.Y + y) * Skin.Width + (SkinIconsStartingPosition.X + x) + offset];
            IconUp = new Texture2D(Skin.GraphicsDevice, iconSize, iconSize, false, SurfaceFormat.Color);
            IconUp.SetData<Color>(copyData);

            // row 2
            offset = 0;
            for (int y = 0; y < iconSize; y++)
                for (int x = 0; x < iconSize; x++)
                    copyData[y * iconSize + x] = originalData[(SkinIconsStartingPosition.Y + y + iconSize) * Skin.Width + (SkinIconsStartingPosition.X + x) + offset];
            IconSave = new Texture2D(Skin.GraphicsDevice, iconSize, iconSize, false, SurfaceFormat.Color);
            IconSave.SetData<Color>(copyData);

            // IconFolder
            offset += 50;
            for (int y = 0; y < iconSize; y++)
                for (int x = 0; x < iconSize; x++)
                    copyData[y * iconSize + x] = originalData[(SkinIconsStartingPosition.Y + y + iconSize) * Skin.Width + (SkinIconsStartingPosition.X + x) + offset];
            IconStar = new Texture2D(Skin.GraphicsDevice, iconSize, iconSize, false, SurfaceFormat.Color);
            IconStar.SetData<Color>(copyData);

            // IconYes
            offset += 50;
            for (int y = 0; y < iconSize; y++)
                for (int x = 0; x < iconSize; x++)
                    copyData[y * iconSize + x] = originalData[(SkinIconsStartingPosition.Y + y + iconSize) * Skin.Width + (SkinIconsStartingPosition.X + x) + offset];
            IconWarning = new Texture2D(Skin.GraphicsDevice, iconSize, iconSize, false, SurfaceFormat.Color);
            IconWarning.SetData<Color>(copyData);

            // IconNo
            offset += 50;
            for (int y = 0; y < iconSize; y++)
                for (int x = 0; x < iconSize; x++)
                    copyData[y * iconSize + x] = originalData[(SkinIconsStartingPosition.Y + y + iconSize) * Skin.Width + (SkinIconsStartingPosition.X + x) + offset];
            IconQuestion = new Texture2D(Skin.GraphicsDevice, iconSize, iconSize, false, SurfaceFormat.Color);
            IconQuestion.SetData<Color>(copyData);

            // IconUp
            offset += 50;
            for (int y = 0; y < iconSize; y++)
                for (int x = 0; x < iconSize; x++)
                    copyData[y * iconSize + x] = originalData[(SkinIconsStartingPosition.Y + y + iconSize) * Skin.Width + (SkinIconsStartingPosition.X + x) + offset];
            IconHome = new Texture2D(Skin.GraphicsDevice, iconSize, iconSize, false, SurfaceFormat.Color);
            IconHome.SetData<Color>(copyData);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Saves serialized version of the current Theme instance
        /// </summary>
        /// <param name="path">Save file location and name</param>
        public void Save(string path)
        {
            // standard serialization
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                XmlSerializer xml = new XmlSerializer(typeof(Themes));

                xml.Serialize(fileStream, this);
                fileStream.Close();
            }

            // remove <PackedValue> tags from XML file
            string[] lines = File.ReadAllLines(path);
            List<string> formattedLines = new List<string>();
            
            foreach (string line in lines)
            {
                if (!line.Contains("PackedValue"))
                    formattedLines.Add(line);
            }

            File.WriteAllLines(path, formattedLines.ToArray());
        }
        #endregion
    }
}
