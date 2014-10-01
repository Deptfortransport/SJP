// *********************************************** 
// NAME             : ImageMapRegion.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 04 May 2011
// DESCRIPTION  	: Server side representation of a generic HTML imagemap AREA element
// ************************************************


using System.Globalization;
using SJP.Common;
using SJP.Common.PropertyManager;
using SJP.Common.ResourceManager;
using SJP.Common.Web;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    /// Server side representation of a generic HTML imagemap AREA element.
    /// </summary>
    public abstract class ImageMapRegion
    {
        #region Private fields

        private readonly string id;
        private readonly string toolTip;
        private readonly string highlightImageUrl;
        private readonly string selectedImageUrl;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. Initialises the object with the supplied data
        /// </summary>
        /// <param name="id">ID of the new region</param>
        /// <param name="toolTip">Tooltip to display when the user points at the region</param>
        /// <param name="highlightImageUrl">Image to use to "highlight" the region</param>
        /// <param name="selectedImageUrl">Image to use when the region is selected.</param>
        protected ImageMapRegion(string id, string toolTip, string highlightImageUrl, string selectedImageUrl)
        {
            this.id = id;
            this.toolTip = toolTip;
            this.highlightImageUrl = highlightImageUrl;
            this.selectedImageUrl = selectedImageUrl;
        }

        /// <summary>
        /// Constructor. Loads the region data from the properties service.
        /// </summary>
        /// <param name="baseKey">The key which forms the base for all relevent properties</param>
        /// <param name="id">The id of the region to load the data for</param>
        /// <param name="p">The property provider to use to retrieve data</param>
        /// <param name="rm">The resource manager to use when converting resource ids from the 
        /// properties service into the text values and image urls to use.</param>
        protected ImageMapRegion(string baseKey, string id, IPropertyProvider properties, SJPResourceManager resourceManager)
        {
            Language language = CurrentLanguage.Value;

            this.id = id;
            this.toolTip = resourceManager.GetString(language, properties[string.Format(CultureInfo.InvariantCulture, ImageMapPropertyKeys.RegionToolTip, new object[] { baseKey, id })]);
            this.highlightImageUrl = resourceManager.GetString(language, properties[string.Format(CultureInfo.InvariantCulture, ImageMapPropertyKeys.RegionHighlightImageResourceId, new object[] { baseKey, id })]);
            this.selectedImageUrl = resourceManager.GetString(language, properties[string.Format(CultureInfo.InvariantCulture, ImageMapPropertyKeys.RegionSelectedImageResourceId, new object[] { baseKey, id })]);
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read only property returning the Id for the map area
        /// </summary>
        public string Id
        {
            get { return id; }
        }

        /// <summary>
        /// Read only property returning the tooltip for the area
        /// </summary>
        public string ToolTip
        {
            get { return toolTip; }
        }

        /// <summary>
        /// Read only property returning the URL of the image to use when the user is pointing
        /// at the area. Generally, this will be a copy of the base image used by the ImageMapControl
        /// with the area corresponding to the ImageMapRegion instance shaded in a different colour.
        /// </summary>
        public string HighlightImageUrl
        {
            get { return highlightImageUrl; }
        }

        /// <summary>
        /// Read only property returning the URL of the image to use when the area has been 
        /// selected. Generally, this will be a copy of the base image used by the ImageMapControl
        /// with the area corresponding to the ImageMapRegion instance shaded in a different colour.
        /// </summary>
        public string SelectedImageUrl
        {
            get { return selectedImageUrl; }
        }

        /// <summary>
        /// Read only property returning the value for the SHAPE attribute of the MAP element.
        /// Subclasses must implement this property.
        /// </summary>
        public abstract string Shape { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the value for the COORDS attribute of the MAP element. Subclasses must 
        /// implement this method.
        /// </summary>
        /// <returns></returns>
        public abstract string GetCoordsAttributeValue();

        #endregion

    }

}