// *********************************************** 
// NAME             : FeatureIcon.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 21 Apr 2011
// DESCRIPTION  	: FeatureIcon class to hold feature image details
// ************************************************
// 


namespace SJP.Common.Web
{
    /// <summary>
    /// FeatureIcon class to hold feature image details
    /// </summary>
    public class FeatureIcon
    {
        #region Private members

        private string featureType;
        private string imageUrlResource;
        private string toolTipResource;
        private string altTextResource;
        private string description;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param param name="featureType">Type of feature, e.g. the SJPAccessibilityType value</param>
        /// <param name="url"></param>
        /// <param name="toolTip"></param>
        /// <param name="altText"></param>
        /// <param name="description"></param>
        public FeatureIcon(string featureType, string url, string toolTip, string altText, string description)
        {
            this.featureType = featureType;
            this.imageUrlResource = url;
            this.toolTipResource = toolTip;
            this.altTextResource = altText;
            this.description = description;

        }
        #endregion

        #region Properties

        /// <summary>
        /// Read/Write. FeatureType
        /// </summary>
        public string FeatureType
        {
            get { return featureType; }
            set { featureType = value; }
        }

        /// <summary>
        /// Read/Write. ImageUrlResource
        /// </summary>
        public string ImageUrlResource
        {
            get { return imageUrlResource; }
            set { imageUrlResource = value; }
        }

        /// <summary>
        /// Read/Write. ToolTipResource
        /// </summary>
        public string ToolTipResource
        {
            get { return toolTipResource; }
            set { toolTipResource = value; }
        }

        /// <summary>
        /// Read/Write. AltTextResource
        /// </summary>
        public string AltTextResource
        {
            get { return altTextResource; }
            set { altTextResource = value; }
        }

        /// <summary>
        /// Read/Write. Description
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        #endregion
    }
}