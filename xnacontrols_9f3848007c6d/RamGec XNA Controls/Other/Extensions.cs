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
/// <summary>Extensions Class</summary>
#endregion

using System;
using Microsoft.Xna.Framework;

namespace RamGecXNAControlsExtensions
{
    /// <summary>
    /// Class containing extension methods for RamGec XNA Controls
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Converts string to Color structure
        /// </summary>
        /// <param name="rgbColorString">Input string. Format: "R, G, B"</param>
        /// <returns>Converted Color structure</returns>
        public static Color ToXNAColor(this String rgbColorString)
        {
            string[] str = rgbColorString.Split(new char[] { ',' });
            return new Color(Int32.Parse(str[0]), Int32.Parse(str[1]), Int32.Parse(str[2]));
        }

        /// <summary>
        /// Converts Color data structure to string ommiting Alpha parameter
        /// </summary>
        /// <param name="color">Input Color</param>
        /// <returns>string: "R, G, B"</returns>
        public static String ToXNAString(this Color color)
        {
            string rgbColorString = color.ToString();
            rgbColorString = rgbColorString.Replace("{", "");
            rgbColorString = rgbColorString.Replace("}", "");
            rgbColorString = rgbColorString.Replace("R", "");
            rgbColorString = rgbColorString.Replace("G", "");
            rgbColorString = rgbColorString.Replace("B", "");
            rgbColorString = rgbColorString.Replace(":", "");
            rgbColorString = rgbColorString.Substring(0, rgbColorString.LastIndexOf(" "));
            rgbColorString = rgbColorString.Replace(" ", ", ");
            return rgbColorString;
        }

        /// <summary>
        /// Converts string to Rectangle structure
        /// </summary>
        /// <param name="str">Input string formatted as Rectangle.ToString()</param>
        /// <returns>Converted Rectangle structure</returns>
        public static Rectangle ToXNARectangle(this String str)
        {
            // parse Rectangle data from XML
            string[] boundsSplit = str.Split(new char[] { ':' });
            Rectangle bounds = new Rectangle();
            bounds.X = Int32.Parse(boundsSplit[1].Substring(0, boundsSplit[1].IndexOf(' ')));
            bounds.Y = Int32.Parse(boundsSplit[2].Substring(0, boundsSplit[2].IndexOf(' ')));
            bounds.Width = Int32.Parse(boundsSplit[3].Substring(0, boundsSplit[3].IndexOf(' ')));
            bounds.Height = Int32.Parse(boundsSplit[4].Substring(0, boundsSplit[4].IndexOf('}')));

            return bounds;
        }
    }
}
