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
/// <summary>Image Class</summary>
#endregion

using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RamGecXNAControls
{
    /// <summary>
    /// Image Control
    /// </summary>
    public class Image : GUIControl
    {
        #region Public Properties
        /// <summary>
        /// Image Texture
        /// </summary>
        public Texture2D Texture = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates Image control
        /// </summary>
        /// <param name="bonds">Relative coordinates of the control</param>
        /// <param name="text">Image texture</param>
        /// <param name="name">Control name</param>
        public Image(Rectangle bonds, Texture2D texture, string name)
            : base(bonds)
        {
            Texture = texture;
            Name = name;
        }

        public Image(Rectangle bounds, Texture2D texture)
            : this(bounds, texture, string.Empty)
        {
        }

        /// <summary>
        /// Creates Image control
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control</param>
        public Image(Rectangle bounds)
            : this(bounds, null, string.Empty)
        {
        }

        /// <summary>
        /// Creates Image control and loads its data from XmlNode
        /// </summary>
        /// <param name="xmlNode">XmlNode containing control data</param>
        public Image(XmlNode xmlNode)
            : base(xmlNode)
        {
        }
        #endregion

        #region Save and Load
        public override void LoadControl(XmlNode xmlNode)
        {
            base.LoadControl(xmlNode);
        }

        public override XmlElement SaveControl(XmlDocument xmlDocument)
        {
            return base.SaveControl(xmlDocument);
        }
        #endregion

        #region Draw and Update
        public override void Draw(SpriteBatch spriteBatch)
        {
            // draw only if texture is present
            if (Texture != null)
                spriteBatch.Draw(Texture, AbsoluteBounds, Color.White * Transparency);

            base.Draw(spriteBatch);
        }
        #endregion
    }
}
