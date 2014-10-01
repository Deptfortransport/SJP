// *********************************************** 
// NAME             : Retailer.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 23 Mar 2011
// DESCRIPTION  	: Retailer class representing a (ticket) retailer
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Text;
using SJP.Common;
using System.Collections;

namespace SJP.UserPortal.Retail
{
    /// <summary>
    /// Retailer class
    /// </summary>
    [Serializable()]
    public class Retailer : IComparer<Retailer>
    {
        #region Private members

        /// <summary>
        /// A unique identifier for retailer
        /// </summary>
        private string id;

        /// <summary>
        /// The mode type for the retailer
        /// </summary>
        private List<SJPModeType> sjpModeTypes;

        /// <summary>
        /// The name of the retailer suitable for display purposes
        /// </summary>
        private string name;

        /// <summary>
        /// The URL of the retailers website suitable for display purposes
        /// </summary>
        private string displayUrl;

        /// <summary>
        /// The URL of the retailers website
        /// </summary>
        private string websiteUrl;

        /// <summary>
        /// The URL to which XML data is posted
        /// </summary>
        private string handoffUrl;

        /// <summary>
        /// The phone number of the retailer
        /// </summary>
        private string phoneNumber;

        /// <summary>
        /// The display phone number of the retailer
        /// </summary>
        private string phoneNumberDisplay;

        /// <summary>
        /// The resource manager base key containing the resources for retailer
        /// </summary>
        private string resourceKey;
                        
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Retailer()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">A unique identifier for retailer</param>
        /// <param name="sjpModeType">The SJPModeType(s) of the retailer</param>
        /// <param name="name">The name of the retailer suitable for display purposes</param>
        /// <param name="websiteUrl">The URL of the retailers website</param>
        /// <param name="handoffUrl">The URL to which XML data is posted</param>
        /// <param name="displayUrl">The URL of the retailers website suitable for display purposes</param>
        /// <param name="resourceKey">The resource manager key containing the resources</param>
        public Retailer(string id, List<SJPModeType> sjpModeTypes, string name, 
            string websiteUrl, string handoffUrl, string displayUrl, 
            string phoneNumber, string phoneNumberDisplay, string resourceKey)
        {
            this.id = id;
            this.sjpModeTypes = sjpModeTypes;
            this.name = name;
            this.websiteUrl = websiteUrl;
            this.handoffUrl = handoffUrl;
            this.displayUrl = displayUrl;
            this.phoneNumber = phoneNumber;
            this.phoneNumberDisplay = phoneNumberDisplay;
            this.resourceKey = resourceKey;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Read/Write property for retailer id
        /// </summary>
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Read/Write property for retailer mode types supported
        /// </summary>
        public List<SJPModeType> Modes
        {
            get { return sjpModeTypes; }
            set { sjpModeTypes = value; }
        }

        /// <summary>
        /// Read/Write property for retailer name
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Read/Write property for the URL of the retailers public website (for display)
        /// </summary>
        public string DisplayUrl
        {
            get { return displayUrl; }
            set { displayUrl = value; }
        }

        /// <summary>
        /// Read/Write property for the URL of the retailers public website (for redirection)
        /// </summary>
        public string WebsiteUrl
        {
            get { return websiteUrl; }
            set { websiteUrl = value; }
        }

        /// <summary>
        /// Read/Write property for handoff URL (the URL we post the XML to)
        /// </summary>
        public string HandoffUrl
        {
            get { return handoffUrl; }
            set { handoffUrl = value; }
        }

        /// <summary>
        /// Read/Write property for retailer phone number
        /// </summary>
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }

        /// <summary>
        /// Read/Write property for retailer phone number to display
        /// </summary>
        public string PhoneNumberDisplay
        {
            get { return phoneNumberDisplay; }
            set { phoneNumberDisplay = value; }
        }

        /// <summary>
        /// Read/Write property for resource manager base key containing resources for retailer
        /// </summary>
        public string ResourceKey
        {
            get { return resourceKey; }
            set { resourceKey = value; }
        }
                                
        /// <summary>
        /// Read only property that returns true if retailer can perform ticket handoff
        /// </summary>
        public bool HandoffSupported
        {
            get { return !(string.IsNullOrEmpty(handoffUrl)); }
        }
        
        #endregion

        #region Public methods

        /// <summary>
        /// Overridden ToString method
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            
            sb.Append(string.Format("Id[{0}] ", id));
            sb.Append(string.Format("Modes[{0}] ", sjpModeTypes));
            sb.Append(string.Format("Name[{0}] ", name));
            sb.Append(string.Format("WebsiteUrl[{0}] ", websiteUrl));
            sb.Append(string.Format("HandoffUrl[{0}] ", handoffUrl));
            sb.Append(string.Format("DisplayUrl[{0}] ", displayUrl));
            sb.Append(string.Format("ResourceKey[{0}]", resourceKey));

            return sb.ToString();
        }

        #endregion

        #region IComparer methods

        /// <summary>
        /// Compares a Retailer returning "Live" retailers above "Test" retailers,
        /// then by a normal string compare on the Id value.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(Retailer x, Retailer y)
        {
            int retval = 0;

            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal. 
                    retval = 0;
                }
                else
                {
                    // If x is null and y is not null, y
                    // is greater. 
                    retval = - 1;
                }
            }
            else
            {
                // If x is not null...
                //
                if (y == null)
                // ...and y is null, x is greater.
                {
                    retval = 1;
                }
                else
                {
                    
                    // A "Test" retailer should come after a "Live" retailer
                    if (x.Id.ToUpper().StartsWith("TEST"))
                    {
                        if (y.Id.ToUpper().StartsWith("TEST"))
                        {
                            // sort them with ordinary string comparison.
                            retval = x.Id.CompareTo(y.Id);
                        }
                        else
                        {
                            // y is not a "Test" retailer, so is greater
                            retval = 1;
                        }
                    }
                    else
                    {
                        // ...and y is not null, and both are not "Test",
                        // sort them with ordinary string comparison.
                        //
                        retval = x.Id.CompareTo(y.Id);
                    }
                }
            }

            return retval;
        }

        #endregion
    }
}
