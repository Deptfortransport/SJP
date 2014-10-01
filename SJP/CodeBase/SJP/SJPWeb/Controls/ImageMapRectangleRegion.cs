// *********************************************** 
// NAME             : ImageMapRectangleRegion.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 05 May 2011
// DESCRIPTION  	: Server side representation of an HTML imagemap AREA element with SHAPE=RECTANGLE.
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Drawing;
using SJP.Common.PropertyManager;
using SJP.Common.ResourceManager;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    /// Server side representation of an HTML imagemap AREA element with SHAPE=RECTANGLE.
    /// </summary>
    public class ImageMapRectangleRegion : ImageMapRegion
    {
        #region Private fields

        /// <summary>
        /// Backing field for the TopLeft property
        /// </summary>
        private readonly Point topLeft;

        /// <summary>
        /// Backing field for the BottomRight property
        /// </summary>
        private readonly Point bottomRight;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. Initialises the object with the supplied data
        /// </summary>
        /// <param name="id">ID of the new region</param>
        /// <param name="toolTip">Tooltip to display when the user points at the region</param>
        /// <param name="highlightImageUrl">Image to use to "highlight" the region</param>
        /// <param name="selectedImageUrl">Image to use when the region is selected.</param>
        /// <param name="topLeft">Co-ordinates of the top left corner of the rectangle</param>
        /// <param name="bottomRight">Co-ordinates of the bottom right corner of the rectangle</param>
        public ImageMapRectangleRegion(string id, string toolTip, string highlightImageUrl, string selectedImageUrl, Point topLeft, Point bottomRight) :
            base(id, toolTip, highlightImageUrl, selectedImageUrl)
        {
            this.topLeft = topLeft;
            this.bottomRight = bottomRight;
        }

        /// <summary>
        /// Constructor. Loads the region data from the properties service.
        /// </summary>
        /// <param name="baseKey">The key which forms the base for all relevent properties</param>
        /// <param name="id">The id of the region to load the data for</param>
        /// <param name="properties">The property provider to use to retrieve data</param>
        /// <param name="resourceManager">The resource manager to use when converting resource ids from the 
        /// properties service into the text values and image urls to use.</param>
        public ImageMapRectangleRegion(string baseKey, string id, IPropertyProvider properties, SJPResourceManager resourceManager) :
            base(baseKey, id, properties, resourceManager)
        {
            int tx = Convert.ToInt32(properties[string.Format(CultureInfo.InvariantCulture, ImageMapPropertyKeys.RegionRectangleTopLeftX, new object[] { baseKey, id })], CultureInfo.InvariantCulture);
            int ty = Convert.ToInt32(properties[string.Format(CultureInfo.InvariantCulture, ImageMapPropertyKeys.RegionRectangleTopLeftY, new object[] { baseKey, id })], CultureInfo.InvariantCulture);
            int bx = Convert.ToInt32(properties[string.Format(CultureInfo.InvariantCulture, ImageMapPropertyKeys.RegionRectangleBottomRightX, new object[] { baseKey, id })], CultureInfo.InvariantCulture);
            int by = Convert.ToInt32(properties[string.Format(CultureInfo.InvariantCulture, ImageMapPropertyKeys.RegionRectangleBottomRightY, new object[] { baseKey, id })], CultureInfo.InvariantCulture);

            topLeft = new Point(tx, ty);
            bottomRight = new Point(bx, by);
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read only property returning the co-ordinates of the top left point of the rectangle
        /// </summary>
        public Point TopLeft
        {
            get { return topLeft; }
        }

        /// <summary>
        /// Read only property returning the co-ordinates of the bottom right point of the rectangle
        /// </summary>
        public Point BottomRight
        {
            get { return bottomRight; }
        }

        /// <summary>
        /// Read only property returning the value for the SHAPE attribute of the MAP element.
        /// </summary>
        public override string Shape
        {
            get { return "rectangle"; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the value for the COORDS attribute of the MAP element.
        /// </summary>
        /// <returns></returns>
        public override string GetCoordsAttributeValue()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0},{1},{2},{3}", new object[] { topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y });
        }

        #endregion
    }


}