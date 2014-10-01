// *********************************************** 
// NAME             : ImageMapPolygonRegion.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 05 May 2011
// DESCRIPTION  	: Server side representation of an HTML imagemap AREA element with SHAPE=POLYGON.
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Text;
using System.Globalization;
using SJP.Common.PropertyManager;
using SJP.Common.ResourceManager;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    /// Server side representation of an HTML imagemap AREA element with SHAPE=POLYGON.
    /// </summary>
    public class ImageMapPolygonRegion : ImageMapRegion
    {
        #region Private fields

        /// <summary>
        /// Backing field for the Points property
        /// </summary>
        private readonly Point[] points;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. Initialises the object with the supplied data
        /// </summary>
        /// <param name="id">ID of the new region</param>
        /// <param name="toolTip">Tooltip to display when the user points at the region</param>
        /// <param name="highlightImageUrl">Image to use to "highlight" the region</param>
        /// <param name="selectedImageUrl">Image to use when the region is selected.</param>
        /// <param name="points">Array of Point objects which define the perimeter of the area</param>
        public ImageMapPolygonRegion(string id, string toolTip, string highlightImageUrl, string selectedImageUrl, Point[] points) :
            base(id, toolTip, highlightImageUrl, selectedImageUrl)
        {
            this.points = (Point[])points.Clone();
        }

        /// <summary>
        /// Constructor. Loads the region data from the properties service.
        /// </summary>
        /// <param name="baseKey">The key which forms the base for all relevent properties</param>
        /// <param name="id">The id of the region to load the data for</param>
        /// <param name="properties">The property provider to use to retrieve data</param>
        /// <param name="resourceManager">The resource manager to use when converting resource ids from the 
        /// properties service into the text values and image urls to use.</param>
        public ImageMapPolygonRegion(string baseKey, string id, IPropertyProvider properties, SJPResourceManager resourceManager) :
            base(baseKey, id, properties, resourceManager)
        {
            int noOfPoints = Convert.ToInt32(properties[string.Format(CultureInfo.InvariantCulture, ImageMapPropertyKeys.RegionPolygonPoints, new object[] { baseKey, id })], CultureInfo.InvariantCulture);
            points = new Point[noOfPoints];

            for (int i = 1; i <= noOfPoints; i++)
            {
                int x = Convert.ToInt32(properties[string.Format(CultureInfo.InvariantCulture, ImageMapPropertyKeys.RegionPolygonPointX, new object[] { baseKey, id, i })], CultureInfo.InvariantCulture);
                int y = Convert.ToInt32(properties[string.Format(CultureInfo.InvariantCulture, ImageMapPropertyKeys.RegionPolygonPointY, new object[] { baseKey, id, i })], CultureInfo.InvariantCulture);
                points[i - 1] = new Point(x, y);
            }
        }

        #endregion

        #region Public properties


        /// <summary>
        /// Read only property returning the value for the SHAPE attribute of the MAP element.
        /// </summary>
        public override string Shape
        {
            get { return "poly"; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the array of Point objects which define the perimeter of the area. 
        /// Updating this array has no effect on the points defined by the control.
        /// </summary>
        public Point[] GetPoints()
        {
            return (Point[])points.Clone();
        }

        /// <summary>
        /// Returns the value for the COORDS attribute of the MAP element.
        /// </summary>
        /// <returns></returns>
        public override string GetCoordsAttributeValue()
        {
            if (points.Length == 0)
                return string.Empty;

            StringBuilder sb = new StringBuilder();

            foreach (Point p in points)
            {
                sb.Append(p.X);
                sb.Append(",");
                sb.Append(p.Y);
                sb.Append(",");
            }
            //Remove the trailing comma that we now have
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        #endregion
    }

}