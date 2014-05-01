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
/// <summary>TabControl Class</summary>
#endregion

using System;
using System.Xml;
using Microsoft.Xna.Framework;

namespace RamGecXNAControls
{
    /// <summary>
    /// TabControl Control
    /// </summary>
    public class TabControl : GUIControl
    {
        #region Public Properties
        /// <summary>
        /// TabControl Text
        /// </summary>
        public string Text = String.Empty;

        public override Rectangle AbsoluteBounds
        {
            get
            {
                Rectangle bounds = base.AbsoluteBounds;
                bounds.Y += Theme.SkinCurrentTabMiddle.Height;
                bounds.Height -= Theme.SkinCurrentTabMiddle.Height;
                return bounds;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates TabControl
        /// </summary>
        public TabControl()
            : base(Rectangle.Empty)
        {
        }

        /// <summary>
        /// Creates TabControl control and loads its data from XmlNode
        /// </summary>
        /// <param name="xmlNode">XmlNode containing control data</param>
        public TabControl(XmlNode xmlNode)
            : base(xmlNode)
        {
        }
        #endregion
        
        #region Save and Load
        public override void LoadControl(XmlNode xmlNode)
        {
            base.LoadControl(xmlNode);

            if (xmlNode.Attributes["Text"] != null)
                Text = xmlNode.Attributes["Text"].Value;
        }

        public override XmlElement SaveControl(XmlDocument xmlDocument)
        {
            XmlElement xmlElement = base.SaveControl(xmlDocument);

            xmlElement.SetAttribute("Text", Text);

            return xmlElement;
        }
        #endregion

        // everything else is handled by TabsContainer
    }
}
