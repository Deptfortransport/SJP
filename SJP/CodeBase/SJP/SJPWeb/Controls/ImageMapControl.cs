// *********************************************** 
// NAME             : ImageMapControl.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 05 May 2011
// DESCRIPTION  	: Generates an HTML image with an associated image map.
// This is a standard control for generating image maps and should be used wherever
// that type of functionality is required. The following should be taken into consideration:
// <list type="bullet">
// <item>If JavaScript is disabled on the client, the control renders links back to the page
// for each region in the map. These are read in the Load event handler and an event is raised
// at this point if a region is specified in the querystring. If JavaScript is enabled, the 
// control causes postbacks in the usual way.</item>
// </list>
// ************************************************


using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.UI;
using SJP.Common.PropertyManager;
using SJP.Common.ResourceManager;
using SJP.Common.Web;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    /// Generates an HTML image with an associated image map.
    /// This is a standard control for generating image maps and should be used wherever
    /// that type of functionality is required. The following should be taken into consideration:
    /// <list type="bullet">
    /// <item>If JavaScript is disabled on the client, the control renders links back to the page
    /// for each region in the map. These are read in the Load event handler and an event is raised
    /// at this point if a region is specified in the querystring. If JavaScript is enabled, the 
    /// control causes postbacks in the usual way.</item>
    /// </list>
    /// </summary>
    [DefaultProperty("ImageUrl"),
    ToolboxData("<{0}:ImageMapControl runat=server></{0}:ImageMapControl>"), ComVisible(false)]
    public class ImageMapControl : System.Web.UI.WebControls.WebControl, IPostBackEventHandler
    {
        #region Private fields

        private string imageUrl;
        private ImageMapRegion selectedRegion;
        private ArrayList regions = new ArrayList();
        private bool isReadOnly = false;

        #endregion

        #region Events

        /// <summary>
        /// Raised when the user clicks a region. Note that this event may be raised during
        /// execution of the Load event handlers if the user does not have JavaScript enabled.
        /// </summary>
        public event EventHandler RegionClicked;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ImageMapControl()
            : base()
        {
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read/write property specifying the image to use when no region is selected.
        /// </summary>
        public string ImageUrl
        {
            get { return imageUrl; }
            set { imageUrl = value; }
        }

        /// <summary>
        /// Read/write property specifying the currently selected region. If set, the image 
        /// given by it's SelectedImage property will be used for the base state of the image.
        /// </summary>
        public ImageMapRegion SelectedRegion
        {
            get { return selectedRegion; }
            set
            {
                if (regions.Contains(value))
                    selectedRegion = value;
                else
                    selectedRegion = null;
            }
        }

        /// <summary>
        /// Read/write property specifying the id of the currently selected region. If an Id is
        /// passed that doesn't exist, the SelectedRegion is set to null.
        /// </summary>
        public string SelectedRegionId
        {
            get
            {
                if (selectedRegion == null)
                    return string.Empty;
                else
                    return selectedRegion.Id;
            }
            set
            {
                selectedRegion = null;
                foreach (ImageMapRegion r in regions)
                    if (r.Id == value)
                    {
                        selectedRegion = r;
                    }
            }
        }

        /// <summary>
        /// Retrieves the array of regions currently specified. Modifying the contents of the array
        /// will not affect the control.
        /// </summary>
        public ImageMapRegion[] GetRegions()
        {
            return (ImageMapRegion[])regions.ToArray(typeof(ImageMapRegion));
        }

        /// <summary>
        /// Determines if image map needs to be read only
        /// </summary>
        public bool IsReadOnly
        {
            get { return isReadOnly; }
            set { isReadOnly = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new region.
        /// </summary>
        /// <param name="region">The region to add. If it is already present, it will be ignored.</param>
        public void AddRegion(ImageMapRegion region)
        {
            if (!regions.Contains(region))
                regions.Add(region);
        }

        /// <summary>
        /// Initialises the properties of the control and the individual regions from the 
        /// properties service.
        /// </summary>
        /// <param name="baseKey">The base string for the property keys to load. See DD078 v1
        /// for information on the names to use for the properties.</param>
        /// <param name="rm">The resource manager to use when converting resource ids
        /// into real strings (all image urls and displayable text is read from here - values
        /// stored in the properties service are assumed to be resource ids</param>
        public void InitialiseFromProperties(string baseKey, SJPResourceManager resourceManager)
        {
            // Read the base properties
            IPropertyProvider properties = (IPropertyProvider)Properties.Current;

            imageUrl = resourceManager.GetString(CurrentLanguage.Value, properties[string.Format(CultureInfo.InvariantCulture, ImageMapPropertyKeys.ImageUrlResourceId, new object[] { baseKey })]);

            string regionIdsString = properties[string.Format(CultureInfo.InvariantCulture, ImageMapPropertyKeys.RegionIds, new object[] { baseKey })];
            string[] regionIds;
            if ((regionIdsString == null) || (regionIdsString.Length == 0))
                regionIds = new string[0];
            else
                regionIds = regionIdsString.Split(',');

            foreach (string regionId in regionIds)
            {
                string currentId = regionId.Trim();
                if (currentId.Length == 0)
                    continue;

                string type = properties[string.Format(CultureInfo.InvariantCulture, ImageMapPropertyKeys.RegionType, new object[] { baseKey, currentId })];

                switch (type.ToLower(CultureInfo.InvariantCulture))
                {
                    case "circle":
                        regions.Add(new ImageMapCircleRegion(baseKey, currentId, properties, resourceManager));
                        break;

                    case "rectangle":
                        regions.Add(new ImageMapRectangleRegion(baseKey, currentId, properties, resourceManager));
                        break;

                    case "polygon":
                        regions.Add(new ImageMapPolygonRegion(baseKey, currentId, properties, resourceManager));
                        break;
                }
            }
        }

        /// <summary>
        /// Raises the RegionClicked event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnRegionClicked(EventArgs e)
        {
            EventHandler h = RegionClicked;
            if (h != null)
                h(this, e);
        }

        /// <summary>
        /// Returns the ImageMapRegion object with the given Id, or null if not found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private ImageMapRegion GetRegionById(string id)
        {
            foreach (ImageMapRegion r in regions)
                if (r.Id == id)
                    return r;
            return null;
        }

        #endregion

        #region Overridden methods

        /// <summary>
        /// Overridden OnLoad method. If the Page is being loaded for the first time
        /// (ie it's not a postback), the QueryString is interrogated to see if this is as
        /// a result of a user clicking a map region with javascript disabled. If so,
        /// the event is raised
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                // Check the QueryString for a region parameter
                string newId = Page.Request.QueryString.Get("SelectedRegion");
                if ((newId != null) && (newId.Length != 0))
                {
                    RaisePostBackEvent(Page.Server.UrlDecode(newId));
                }

            }
            base.OnLoad(e);
        }

        /// <summary>
        /// Overloaded Render method. Writes out the following HTML elements:
        /// <list type="bullet">
        /// <item>MAP - defines the imagemap and contains an AREA element for every ImageMapRegion
        /// object in the regions collection.</item>
        /// <item>IMG - image element which uses the map.</item>
        /// </list>
        /// If JavaScript is being used, and there are highlight images (images that are used when
        /// the users moves the mouse over a defined area to give the impression that the area is
        /// highlighted) then a script block is written out to preload the images in question.
        /// The ImageMap script block (accessed from the script repository) is also added to the
        /// page to provide the preload and swapImage methods.
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
           
            string baseImage;
            if (selectedRegion == null)
                baseImage = imageUrl;
            else if ((selectedRegion.SelectedImageUrl != null) && (selectedRegion.SelectedImageUrl.Length != 0))
                baseImage = selectedRegion.SelectedImageUrl;
            else
                baseImage = imageUrl;

            // If JavaScript is enabled, generate a script block to preload the highlight images
            StringBuilder scriptBlock = new StringBuilder();

            // First, render the map - but only if we're not in read only mode
            if (!isReadOnly)
            {
                writer.WriteBeginTag("map");
                writer.WriteAttribute("name", this.UniqueID + "map");
                writer.WriteAttribute("id", this.ClientID + "map");
                writer.Write(HtmlTextWriter.TagRightChar);

                foreach (ImageMapRegion r in regions)
                {
                    writer.WriteBeginTag("area");
                    writer.WriteAttribute("shape", r.Shape);
                    writer.WriteAttribute("coords", r.GetCoordsAttributeValue());
                    writer.WriteAttribute("title", r.ToolTip);
                    writer.WriteAttribute("style", "cursor: hand");
                    writer.WriteAttribute("alt", r.ToolTip);

                    
                    writer.WriteAttribute("href", FormatUrlForRegion(r.Id));
                    writer.WriteLine(HtmlTextWriter.SelfClosingTagEnd);

                }
                writer.WriteEndTag("map");
                writer.WriteLine();
            }

            // Now render the image
            writer.WriteBeginTag("img");
            writer.WriteAttribute("id", this.ClientID);
            writer.WriteAttribute("src", ResolveClientUrl(((SJPPage)Page).ImagePath + baseImage));
            writer.WriteAttribute("title", this.ToolTip);
            writer.WriteAttribute("alt", this.ToolTip);
            writer.WriteAttribute("border", "0");

            if (!isReadOnly)
                writer.WriteAttribute("usemap", string.Format(CultureInfo.InvariantCulture, "#{0}map", new object[] { this.UniqueID }));

            writer.WriteLine(HtmlTextWriter.SelfClosingTagEnd);


        }

        /// <summary>
        /// Generates the URL to use for the href for a map area when JavaScript is disabled.
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        private string FormatUrlForRegion(string regionId)
        {
            StringBuilder sb = new StringBuilder(255);
            string url = Page.Request.Url.ToString();
            sb.Append(url);

            // See if there is already a SelectedRegion item in the URL. If so, overwrite it.
            if (url.IndexOf("SelectedRegion=") >= 0)
            {
                string currentUrlValue = Page.Request.QueryString["SelectedRegion"];
                sb.Replace("SelectedRegion=" + currentUrlValue, "SelectedRegion=" + Page.Server.UrlEncode(regionId));
            }
            // See if the querystring already has parameters and if so add this to the end
            else if (url.IndexOf("?") >= 0)
            {
                sb.Append("&SelectedRegion=");
                sb.Append(Page.Server.UrlEncode(regionId));
            }
            // No parameters yet, so need to start the list with a ?
            else
            {
                sb.Append("?SelectedRegion=");
                sb.Append(Page.Server.UrlEncode(regionId));
            }

            return Page.Server.HtmlEncode(sb.ToString());
        }

        #endregion

        #region IPostBackEventHandler Members

        /// <summary>
        /// Called when a JavaScript postback has been performed due to a user with javascript
        /// enabled clicking on a map region. Calls the OnRegionClicked method to raise the 
        /// RegionClicked event
        /// </summary>
        /// <param name="eventArgument"></param>
        public void RaisePostBackEvent(string eventArgument)
        {
            // Find the region that's been selected
            selectedRegion = GetRegionById(eventArgument);
            OnRegionClicked(EventArgs.Empty);
        }

        #endregion
    }




}