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
/// <summary>GUIList Class</summary>
#endregion

using System.Collections.Generic;

namespace RamGecXNAControls
{
    /// <summary>
    /// Custom List collection (adds Parent parameter)
    /// </summary>
    /// <typeparam name="T">Type - GUIControl</typeparam>
    public class GUIList<T> : IList<GUIControl>
    {
        #region Private Properties and Methods
        /// <summary>
        /// Compare two controls by their Z buffer (highest -> lowest)
        /// </summary>
        private static int CompareByZIndex(GUIControl a, GUIControl b)
        {
            if (a == null || b == null)
            {
                return 0;
            }
            else
            {
                return b.ZIndex.CompareTo(a.ZIndex);
            }
        }

        /// <summary>
        /// Parent Control that created this list
        /// </summary>
        private GUIControl parent = null;

        /// <summary>
        /// Parent GUIManager
        /// </summary>
        private GUIManager parentGUIManager = null;

        /// <summary>
        /// Actual list that stores GUIControls
        /// </summary>
        private List<GUIControl> data = new List<GUIControl>();
        #endregion

        #region Constructor
        /// <summary>
        /// Creates GUIList collection
        /// </summary>
        /// <param name="parent">Parent control</param>
        public GUIList(GUIControl parent, GUIManager guiManager)
            : base()
        {
            this.parent = parent;
            this.parentGUIManager = guiManager;

            //while (parent != null && parent.Theme != null)
            //    parent = parent.Parent;

            //this.theme = theme;
        }
        #endregion

        #region Methods overwritten from IList
        public int IndexOf(GUIControl item)
        {
            return data.IndexOf(item);
        }

        public void Insert(int index, GUIControl item)
        {
            item.Parent = parent;
            item.ParentGUIManager = parentGUIManager;
            data.Insert(index, item);

            // windows are always sorted, that's how we handle active and "window on top of another" issues
            if (item is Window)
                data.Sort(CompareByZIndex);
        }

        public void RemoveAt(int index)
        {
            data.RemoveAt(index);
        }

        public GUIControl this[int index]
        {
            get
            {
                return data[index];
            }
            set
            {
                // assign parent
                value.Parent = parent;
                value.ParentGUIManager = parentGUIManager;
                data[index] = value;

                // windows are always sorted, that's how we handle active and "window on top of another" issues
                if (value is Window)
                    data.Sort(CompareByZIndex);
            }
        }

        public void Add(GUIControl item)
        {
            // assign parent
            item.Parent = parent;
            item.ParentGUIManager = parentGUIManager;
            data.Add(item);

            // windows are always sorted, that's how we handle active and "window on top of another" issues
            if (item is Window)
                data.Sort(CompareByZIndex);
        }

        public void Sort()
        {
            data.Sort(CompareByZIndex);
        }

        public void Clear()
        {
            data.Clear();
        }

        public bool Contains(GUIControl item)
        {
            return data.Contains(item);
        }

        public void CopyTo(GUIControl[] array, int arrayIndex)
        {
            data.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return data.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(GUIControl item)
        {
            return data.Remove(item);
        }

        public IEnumerator<GUIControl> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return data.GetEnumerator();
        }
        #endregion
    }
}
