// *********************************************** 
// NAME             : ImageMapPropertyKeys.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 04 May 2011
// DESCRIPTION  	: Keys used for loading ImageMap initialisation info from the properties service
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    /// Keys used for loading ImageMap initialisation info from the properties service
    /// </summary>
    public sealed class ImageMapPropertyKeys
    {
        /// <summary>
        /// Private default constructor to prevent instantiation
        /// </summary>
        private ImageMapPropertyKeys()
        {
        }

        public const string ImageUrlResourceId = "{0}.ImageUrlResourceId";
        public const string RegionIds = "{0}.RegionIds";
        public const string RegionToolTip = "{0}.{1}.ToolTip";
        public const string RegionHighlightImageResourceId = "{0}.{1}.HighlightImageResourceId";
        public const string RegionSelectedImageResourceId = "{0}.{1}.SelectedImageResourceId";
        public const string RegionType = "{0}.{1}.RegionType";

        public const string RegionCircleCentreX = "{0}.{1}.centre.x";
        public const string RegionCircleCentreY = "{0}.{1}.centre.y";
        public const string RegionCircleCentreRadius = "{0}.{1}.radius";

        public const string RegionRectangleTopLeftX = "{0}.{1}.topleft.x";
        public const string RegionRectangleTopLeftY = "{0}.{1}.topleft.y";
        public const string RegionRectangleBottomRightX = "{0}.{1}.bottomright.x";
        public const string RegionRectangleBottomRightY = "{0}.{1}.bottomright.y";

        public const string RegionPolygonPoints = "{0}.{1}.points";
        public const string RegionPolygonPointX = "{0}.{1}.point.{2}.x";
        public const string RegionPolygonPointY = "{0}.{1}.point.{2}.y";
    }
}