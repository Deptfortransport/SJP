// *********************************************** 
// NAME             : ImageMapCircleRegion.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 06 May 2011
// DESCRIPTION  	: Server side representation of an HTML imagemap AREA element with SHAPE=CIRCLE.
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
    /// Server side representation of an HTML imagemap AREA element with SHAPE=CIRCLE.
    /// </summary>
    public class ImageMapCircleRegion : ImageMapRegion
    {
        #region Private fields

        /// <summary>
        /// Backing field for the Centre property
        /// </summary>
        private readonly Point centre;

        /// <summary>
        /// Backing field for the Radius property
        /// </summary>
        private readonly int radius;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. Initialises the object with the supplied data
        /// </summary>
        /// <param name="id">ID of the new region</param>
        /// <param name="toolTip">Tooltip to display when the user points at the region</param>
        /// <param name="highlightImageUrl">Image to use to "highlight" the region</param>
        /// <param name="selectedImageUrl">Image to use when the region is selected.</param>
        /// <param name="centre">Centre of the region</param>
        /// <param name="radius">Radius of the region, in pixels</param>
        public ImageMapCircleRegion(string id, string toolTip, string highlightImageUrl, string selectedImageUrl, Point centre, int radius) :
            base(id, toolTip, highlightImageUrl, selectedImageUrl)
        {
            this.centre = centre;
            this.radius = radius;
        }

        /// <summary>
        /// Constructor. Loads the region data from the properties service.
        /// </summary>
        /// <param name="baseKey">The key which forms the base for all relevent properties</param>
        /// <param name="id">The id of the region to load the data for</param>
        /// <param name="p">The property provider to use to retrieve data</param>
        /// <param name="rm">The resource manager to use when converting resource ids from the 
        /// properties service into the text values and image urls to use.</param>
        public ImageMapCircleRegion(string baseKey, string id, IPropertyProvider properties, SJPResourceManager resourceManager) :
            base(baseKey, id, properties, resourceManager)
        {
            int x = Convert.ToInt32(properties[string.Format(CultureInfo.InvariantCulture, ImageMapPropertyKeys.RegionCircleCentreX, new object[] { baseKey, id })], CultureInfo.InvariantCulture);
            int y = Convert.ToInt32(properties[string.Format(CultureInfo.InvariantCulture, ImageMapPropertyKeys.RegionCircleCentreY, new object[] { baseKey, id })], CultureInfo.InvariantCulture);
            centre = new Point(x, y);
            radius = Convert.ToInt32(properties[string.Format(CultureInfo.InvariantCulture, ImageMapPropertyKeys.RegionCircleCentreRadius, new object[] { baseKey, id })], CultureInfo.InvariantCulture);
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read only property returning the co-ordinates of the centre of the circle.
        /// </summary>
        public Point Centre
        {
            get { return centre; }
        }

        /// <summary>
        /// Read only property returning the radius of the circle, in pixels
        /// </summary>
        public int Radius
        {
            get { return radius; }
        }

        /// <summary>
        /// Read only property returning the value for the SHAPE attribute of the MAP element.
        /// </summary>
        public override string Shape
        {
            get { return "circle"; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the value for the COORDS attribute of the MAP element.
        /// </summary>
        /// <returns></returns>
        public override string GetCoordsAttributeValue()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0},{1},{2}", new object[] { centre.X, centre.Y, radius });
        }

        #endregion

    }

}