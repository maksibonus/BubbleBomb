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
/// <summary>Chart Class</summary>
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RamGecXNAControlsExtensions;

namespace RamGecXNAControls
{
    /// <summary>
    /// Chart Control
    /// </summary>
    public class Chart : GUIControl
    {
        #region Public Properties
        /// <summary>
        /// Chart Title
        /// </summary>
        public string Title = String.Empty;

        /// <summary>
        /// Width of axis lines
        /// </summary>
        public int AxisLineWidth = 1;

        /// <summary>
        /// Width of chart lines
        /// </summary>
        public int ChartLineWidth = 2;

        /// <summary>
        /// Display Y axis range indicators
        /// </summary>
        public bool ShowYAxisRange = true;

        /// <summary>
        /// Display X axis range indicators
        /// </summary>
        public bool ShowXAxisRange = true;

        /// <summary>
        /// Display grid
        /// </summary>
        public bool ShowGrid = true;

        /// <summary>
        /// Space between grid lines
        /// </summary>
        public int GridSpacing = 40;

        /// <summary>
        /// Display values of each grid line on X/Y axis
        /// </summary>
        public bool ShowGridValues = false;

        /// <summary>
        /// Gets a new copy of Charts list
        /// </summary>
        public List<ChartData> ChartsList
        {
            get
            {
                // make a copy
                List<ChartData> newList = new List<ChartData>(charts.Count);

                charts.ForEach((item) =>
                {
                    newList.Add(new ChartData() { Data = item.Data, LineColor = item.LineColor, Name = item.Name });
                });

                return newList;
            }
            private set { }
        }

        /// <summary>
        /// Default Font
        /// </summary>
        public SpriteFont Font
        {
            get { return _font ?? Theme.ChartFont; }
            set { _font = value; }
        }
        private SpriteFont _font = null;

        /// <summary>
        /// Text color
        /// </summary>
        public Color TextColor
        {
            get { return _textColor ?? Theme.ChartColor; }
            set { _textColor = value; }
        }
        private Color? _textColor = null;

        /// <summary>
        /// Chart Margins inside the control
        /// </summary>
        public int ChartMargin
        {
            get { return _chartMargin ?? Theme.ChartMargin; }
            set { _chartMargin = value; }
        }
        private int? _chartMargin = null;

        /// <summary>
        /// Structure of each chart
        /// </summary>
        public struct ChartData
        {
            public string Name;
            public Color LineColor;
            public List<float> Data;
        }
        #endregion

        #region Private Properties
        /// <summary>
        /// Collection of charts
        /// </summary>
        private List<ChartData> charts = new List<ChartData>();

        // Min/Max values of Y and X axis
        // X axis min is always 0
        float maxY = 0f, minY = 0f;
        int maxX = 0;

        /// <summary>
        /// Blank Texture (1x1 size) used for drawing lines
        /// </summary>
        private Texture2D dotTexture = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a Chart control
        /// </summary>
        /// <param name="bounds">Relative coordinates of the control</param>
        public Chart(Rectangle bounds)
            : base(bounds)
        {
        }

        /// <summary>
        /// Creates Chart control and loads its data from XmlNode
        /// </summary>
        /// <param name="xmlNode">XmlNode containing control data</param>
        public Chart(XmlNode xmlNode)
            : base(xmlNode)
        {
        }
        #endregion

        #region Save and Load
        public override void LoadControl(XmlNode xmlNode)
        {
            base.LoadControl(xmlNode);

            if (xmlNode.Attributes["Title"] != null)
                Title = xmlNode.Attributes["Title"].Value;
            if (xmlNode.Attributes["AxisLineWidth"] != null)
                AxisLineWidth = Int32.Parse(xmlNode.Attributes["AxisLineWidth"].Value);
            if (xmlNode.Attributes["ChartLineWidth"] != null)
                ChartLineWidth = Int32.Parse(xmlNode.Attributes["ChartLineWidth"].Value);
            if (xmlNode.Attributes["GridSpacing"] != null)
                GridSpacing = Int32.Parse(xmlNode.Attributes["GridSpacing"].Value);
            if (xmlNode.Attributes["ShowGrid"] != null)
                ShowGrid = bool.Parse(xmlNode.Attributes["ShowGrid"].Value);
            if (xmlNode.Attributes["ShowGridValues"] != null)
                ShowGridValues = bool.Parse(xmlNode.Attributes["ShowGridValues"].Value);
            if (xmlNode.Attributes["ShowXAxisRange"] != null)
                ShowXAxisRange = bool.Parse(xmlNode.Attributes["ShowXAxisRange"].Value);
            if (xmlNode.Attributes["ShowYAxisRange"] != null)
                ShowYAxisRange = bool.Parse(xmlNode.Attributes["ShowYAxisRange"].Value);
            if (xmlNode.Attributes["TextColor"] != null)
                TextColor = xmlNode.Attributes["TextColor"].Value.ToXNAColor();
            if (xmlNode.Attributes["ChartMargin"] != null)
                ChartMargin = Int32.Parse(xmlNode.Attributes["ChartMargin"].Value);

            if (xmlNode.Attributes["Data"] != null)
            {
                string[] lists = xmlNode.Attributes["Data"].Value.Split(new char[] { ';' });

                foreach (string data in lists)
                {
                    if (data.Length <= 0)
                        break;

                    string[] values = data.Split(new char[] { ',' });

                    List<float> chart = new List<float>();
                    foreach (string number in values)
                    {
                        if (number.Length <= 0)
                            break;

                        chart.Add(float.Parse(number));
                    }

                    int chartID = 0;
                    AddChart(chart, "Chart" + (++chartID).ToString(), Color.CornflowerBlue);
                }
            }
        }

        public override XmlElement SaveControl(XmlDocument xmlDocument)
        {
            XmlElement xmlElement = base.SaveControl(xmlDocument);

            xmlElement.SetAttribute("Title", Title);
            xmlElement.SetAttribute("AxisLineWidth", AxisLineWidth.ToString());
            xmlElement.SetAttribute("ChartLineWidth", ChartLineWidth.ToString());
            xmlElement.SetAttribute("GridSpacing", GridSpacing.ToString());
            xmlElement.SetAttribute("ShowGrid", ShowGrid.ToString());
            xmlElement.SetAttribute("ShowGridValues", ShowGridValues.ToString());
            xmlElement.SetAttribute("ShowXAxisRange", ShowXAxisRange.ToString());
            xmlElement.SetAttribute("ShowYAxisRange", ShowYAxisRange.ToString());
            xmlElement.SetAttribute("TextColor", TextColor.ToXNAString());
            xmlElement.SetAttribute("ChartMargin", ChartMargin.ToString());

            List<Chart.ChartData> charts = ChartsList;
            StringBuilder sb = new StringBuilder();

            foreach (Chart.ChartData chart in charts)
            {
                foreach (float f in chart.Data)
                {
                    sb.Append(f);
                    sb.Append(",");
                }

                sb.Append(";");
            }

            xmlElement.SetAttribute("Data", sb.ToString());

            return xmlElement;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds a new chart to be displayed on this control
        /// </summary>
        /// <param name="data">Chart data. Data list's index represents X, while values represent Y axis</param>
        /// <param name="name">Chart name</param>
        /// <param name="color">Chart line color</param>
        public void AddChart(List<float> data, string name, Color color)
        {
            ChartData chart = new ChartData()
            {
                Name = name,
                LineColor = color,
                Data = data
            };
            charts.Add(chart);
            UpdateMinMaxValues();
        }

        /// <summary>
        /// Removes a chart with a given name from control
        /// </summary>
        /// <param name="name">Chart name</param>
        public void RemoveChart(string name)
        {
            for (int i = 0; i < charts.Count; i++)
            {
                if (charts[i].Name == name)
                {
                    charts.RemoveAt(i);
                    UpdateMinMaxValues();
                    return;
                }
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Updates MinY, MaxY and MaxX values
        /// </summary>
        private void UpdateMinMaxValues()
        {
            // find the max/min value of Y axis
            float localMaximum = 0f, localMinimum = 0f;

            foreach (ChartData chart in charts)
            {
                List<float> data = chart.Data;
                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i] > localMaximum)
                        localMaximum = data[i];
                    if (data[i] < localMinimum)
                        localMinimum = data[i];
                }

                if (maxY < localMaximum)
                    maxY = localMaximum;
                if (minY > localMinimum)
                    minY = localMinimum;

                if (maxX < data.Count)
                    maxX = data.Count;
            }
        }

        /// <summary>
        /// Draws a line
        /// </summary>
        /// <param name="spriteBatch">Target SpriteBatch</param>
        /// <param name="width">Line Width</param>
        /// <param name="color">Line Color</param>
        /// <param name="point1">Starting Point</param>
        /// <param name="point2">Ending Point</param>
        private void DrawLine(SpriteBatch spriteBatch, float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            spriteBatch.Draw(dotTexture, point1, null, color * Transparency,
                angle, Vector2.Zero, new Vector2(length, width),
                SpriteEffects.None, 0);
        }
        #endregion

        #region Draw and Update
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
                return;

            // cache
            Rectangle bounds = AbsoluteBounds;
            Color tint = Theme.ChartTintColor[(int)state] * Transparency;
            Vector2 titleSize = Theme.ChartFont.MeasureString(Title);

            // create a blank texture (needs to be created only once and inside Draw method since we need to access GraphicsDevice)
            if (dotTexture == null)
            {
                dotTexture = new Texture2D(Theme.Skin.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                dotTexture.SetData(new[] { Color.White });
            }

            #region Skin
            // top-left
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X, bounds.Y, Theme.SkinChartTopLeft.Width, Theme.SkinChartTopLeft.Height),
                Theme.SkinChartTopLeft, tint);

            // top-right
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + bounds.Width - Theme.SkinChartTopRight.Width, bounds.Y, Theme.SkinChartTopRight.Width, Theme.SkinChartTopRight.Height),
                Theme.SkinChartTopRight, tint);

            // top
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + Theme.SkinChartTopLeft.Width, bounds.Y, bounds.Width - Theme.SkinChartTopLeft.Width - Theme.SkinChartTopRight.Width, Theme.SkinChartTop.Height),
                Theme.SkinChartTop, tint);

            // left
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X, bounds.Y + Theme.SkinChartTopLeft.Width, Theme.SkinChartLeft.Width, bounds.Height - Theme.SkinChartTopLeft.Height - Theme.SkinChartBottomLeft.Height),
                Theme.SkinChartLeft, tint);

            // right
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + bounds.Width - Theme.SkinChartRight.Width, bounds.Y + Theme.SkinChartTopRight.Height, Theme.SkinChartRight.Width, bounds.Height - Theme.SkinChartTopRight.Height - Theme.SkinChartBottomRight.Height),
                Theme.SkinChartRight, tint);

            // bottom-left
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X, bounds.Y + bounds.Height - Theme.SkinChartBottomLeft.Height, Theme.SkinChartBottomLeft.Width, Theme.SkinChartBottomLeft.Height),
                Theme.SkinChartBottomLeft, tint);

            // bottom-right
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + bounds.Width - Theme.SkinChartBottomRight.Width, bounds.Y + bounds.Height - Theme.SkinChartBottomRight.Height, Theme.SkinChartBottomRight.Width, Theme.SkinChartBottomRight.Height),
                Theme.SkinChartBottomRight, tint);

            // bottom
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + Theme.SkinChartBottomLeft.Width, bounds.Y + bounds.Height - Theme.SkinChartBottom.Height, bounds.Width - Theme.SkinChartBottomLeft.Width - Theme.SkinChartBottomRight.Width, Theme.SkinChartBottom.Height),
                Theme.SkinChartBottom, tint);

            // middle
            spriteBatch.Draw(Theme.Skin,
                new Rectangle(bounds.X + Theme.SkinChartLeft.Width, bounds.Y + Theme.SkinChartTop.Height, bounds.Width - Theme.SkinChartLeft.Width - Theme.SkinChartRight.Width, bounds.Height - Theme.SkinChartTop.Height - Theme.SkinChartBottom.Height),
                Theme.SkinChartMiddle, tint);
            #endregion

            // margins
            int MarginLeft = ChartMargin + ((ShowYAxisRange || ShowGridValues) ? 20 : 0) + (ShowYAxisRange ? 10 : 0);
            int MarginTop = ChartMargin + (ShowYAxisRange ? 20 : 0) + (String.IsNullOrEmpty(Title) ? 0 : 10);
            int MarginBottom = ChartMargin + ((ShowXAxisRange || ShowGridValues) ? 16 : 0) + (ShowXAxisRange ? 8 : 0);
            int MarginRight = ChartMargin + (ShowXAxisRange ? 20 : 0);

            // actual chart plot size
            Rectangle chartSize = new Rectangle(bounds.X + MarginLeft,
                bounds.Y + MarginTop,
                bounds.Width - MarginLeft - MarginRight,
                bounds.Height - MarginTop - MarginBottom);

            float yRange = maxY - minY;

            // show range indicators for Y axis
            if (ShowYAxisRange)
            {
                // max
                Vector2 textSize = Theme.ChartFont.MeasureString(maxY.ToString("F1"));
                spriteBatch.DrawString(Font, maxY.ToString("F1"), new Vector2(chartSize.X - textSize.X - 12, chartSize.Y - textSize.Y), TextColor * Transparency);

                // min
                textSize = Theme.ChartFont.MeasureString(minY.ToString("F1"));
                spriteBatch.DrawString(Font, minY.ToString("F1"), new Vector2(chartSize.X - textSize.X - 12, chartSize.Y + chartSize.Height), TextColor * Transparency);

                // zero
                if (minY < 0.9f && yRange != 0f)
                {
                    textSize = Theme.ChartFont.MeasureString("0");
                    spriteBatch.DrawString(Font, "0", new Vector2(chartSize.X - textSize.X - 12, (maxY / yRange) * chartSize.Height + chartSize.Y - (textSize.Y / 2)), TextColor * Transparency);
                }
            }

            // show range indicators for X axis
            if (ShowXAxisRange)
            {
                // max
                Vector2 textSize = Theme.ChartFont.MeasureString(maxX.ToString());
                spriteBatch.DrawString(Font, maxX.ToString(), new Vector2(chartSize.X + chartSize.Width, chartSize.Y + chartSize.Height + 12), TextColor * Transparency);

                // min
                textSize = Theme.ChartFont.MeasureString("0");
                spriteBatch.DrawString(Font, "0", new Vector2(chartSize.X - textSize.X, chartSize.Y + chartSize.Height + 12), TextColor * Transparency);
            }


            // draw grid
            if (ShowGrid)
            {
                // Y Axis

                // Y=0 location
                float zeroY = chartSize.Y + chartSize.Height;

                // move Y=0 location if it's far from beginning
                if (minY < 0.9f && yRange != 0f)
                {
                    zeroY = (maxY / yRange) * chartSize.Height + chartSize.Y;
                    DrawLine(spriteBatch, AxisLineWidth, Color.Pink, new Vector2(chartSize.X, zeroY), new Vector2(chartSize.X + chartSize.Width, zeroY));
                }

                
                // draw Y>0
                float currentY = zeroY;
                while ((currentY = currentY - GridSpacing) > chartSize.Y)
                {
                    DrawLine(spriteBatch, AxisLineWidth, Color.Pink, new Vector2(chartSize.X, currentY), new Vector2(chartSize.X + chartSize.Width, currentY));

                    // indicate value
                    if (ShowGridValues)
                    {
                        float value = ((currentY - chartSize.Y) / chartSize.Height) * yRange + minY;
                        Vector2 textSize = Theme.ChartFont.MeasureString(value.ToString("F1"));
                        spriteBatch.DrawString(Font, value.ToString("F1"), new Vector2(chartSize.X - textSize.X - 6, currentY - (textSize.Y / 2)), TextColor * Transparency);
                    }
                }

                // draw Y<0
                currentY = zeroY;
                while ((currentY = currentY + GridSpacing) < chartSize.Y + chartSize.Height)
                {
                    DrawLine(spriteBatch, AxisLineWidth, Color.Pink, new Vector2(chartSize.X, currentY), new Vector2(chartSize.X + chartSize.Width, currentY));

                    // indicate value
                    if (ShowGridValues)
                    {
                        float value = ((currentY - chartSize.Y) / chartSize.Height) * yRange + minY;
                        Vector2 textSize = Theme.ChartFont.MeasureString(value.ToString("F1"));
                        spriteBatch.DrawString(Font, value.ToString("F1"), new Vector2(chartSize.X - textSize.X - 6, currentY - (textSize.Y / 2)), TextColor * Transparency);
                    }
                }


                // X Axis
                float currentX = chartSize.X;
                while ((currentX = currentX + GridSpacing) < chartSize.X + chartSize.Width)
                {
                    DrawLine(spriteBatch, AxisLineWidth, Color.Pink, new Vector2(currentX, chartSize.Y), new Vector2(currentX, chartSize.Y + chartSize.Height));

                    // indicate value
                    if (ShowGridValues)
                    {
                        float value = ((currentX - chartSize.X) / chartSize.Width) * maxX;
                        Vector2 textSize = Theme.ChartFont.MeasureString(value.ToString("F1"));
                        spriteBatch.DrawString(Font, value.ToString("F1"), new Vector2(currentX - (textSize.X / 2), chartSize.Y + chartSize.Height + 6), TextColor * Transparency);
                    }
                }
            }

            // draw charts
            for (int i = 0; i < charts.Count && yRange != 0f; i++)
            {
                float prevX = 0f, prevY = 0f, actualX = 0f, actualY = 0f;
                ChartData chart = charts[i];

                // you need at least 2 values to make a chart
                if (chart.Data.Count < 2)
                    continue;

                float xIntervals = (chartSize.Width / (chart.Data.Count - 1));

                for (int x = 0; x < chart.Data.Count; x++)
                {
                    actualY = ((1f - ((chart.Data[x] - minY) / yRange)) * chartSize.Height) + chartSize.Y;
                    actualX = (xIntervals * x) + chartSize.X;

                    if (x == 0)
                    {
                        prevX = actualX;
                        prevY = actualY;
                    }

                    DrawLine(spriteBatch, ChartLineWidth, chart.LineColor, new Vector2(prevX, prevY), new Vector2(actualX, actualY));

                    prevX = actualX;
                    prevY = actualY;
                }
            }

            // draw axis
            // Y-axis line
            DrawLine(spriteBatch, AxisLineWidth, TextColor, new Vector2(bounds.X + MarginLeft, bounds.Y + MarginTop), new Vector2(bounds.X + MarginLeft, bounds.Y + bounds.Height - MarginBottom));

            // X-axis line
            DrawLine(spriteBatch, AxisLineWidth, TextColor, new Vector2(bounds.X + MarginLeft, bounds.Y + bounds.Height - MarginBottom), new Vector2(bounds.X + bounds.Width - MarginRight, bounds.Y + bounds.Height - MarginBottom));

            // title
            if (!String.IsNullOrEmpty(Title))
            {
                Vector2 textSize = Theme.ChartFont.MeasureString(Title);
                spriteBatch.DrawString(Theme.ChartFont, Title,
                    new Vector2((bounds.X + (bounds.Width / 2)) - (textSize.X / 2), bounds.Y + 10),
                    Theme.CheckBoxColor * Transparency);
            }

            base.Draw(spriteBatch);
        }
        #endregion
    }
}
