// *********************************************** 
// NAME             : FeaturesControlAdapter.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 21 Mar 2012
// DESCRIPTION  	: Features control adapter containing common methods required by the features controls
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.UserPortal.JourneyControl;

namespace SJP.Common.Web
{
    /// <summary>
    /// Features control adapter containing common methods required by the features controls
    /// </summary>
    public class FeaturesControlAdapter
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public FeaturesControlAdapter()
        {
        }
        
        #endregion

        #region Public methods

        /// <summary>
        /// Returns a list of FeatureIcons for Rail vehicles
        /// </summary>
        /// <param name="Icons"></param>
        /// <returns></returns>
        public List<FeatureIcon> GetRailVehicleFeatureIcons(List<int> icons)
        {
            List<FeatureIcon> featureIcons = new List<FeatureIcon>();

            if (icons != null)
            {
                bool feature21Present = false;
                bool feature22Present = false;
                bool feature30Present = false;
                bool feature31Present = false;
                bool feature32Present = false;

                foreach (int icon in icons)
                {
                    // simple cases first -- 1:1 relationship between features and icons ...
                    switch (icon)
                    {
                        case 0:
                            featureIcons.Add(new FeatureIcon("0", "RailVehicleFeaturesIcon.ImageURL.0", "RailVehicleFeaturesIcon.AltTextToolTip.0", "RailVehicleFeaturesIcon.AltTextToolTip.0", "RailVehicleFeaturesIcon.AltTextToolTip.0"));
                            break;
                        case 1:
                            featureIcons.Add(new FeatureIcon("1", "RailVehicleFeaturesIcon.ImageURL.1", "RailVehicleFeaturesIcon.AltTextToolTip.1", "RailVehicleFeaturesIcon.AltTextToolTip.1", "RailVehicleFeaturesIcon.AltTextToolTip.1"));
                            break;
                        case 2:
                            featureIcons.Add(new FeatureIcon("2", "RailVehicleFeaturesIcon.ImageURL.2", "RailVehicleFeaturesIcon.AltTextToolTip.2", "RailVehicleFeaturesIcon.AltTextToolTip.2", "RailVehicleFeaturesIcon.AltTextToolTip.2"));
                            break;
                        case 3:
                            featureIcons.Add(new FeatureIcon("3", "RailVehicleFeaturesIcon.ImageURL.3", "RailVehicleFeaturesIcon.AltTextToolTip.3", "RailVehicleFeaturesIcon.AltTextToolTip.3", "RailVehicleFeaturesIcon.AltTextToolTip.3"));
                            break;
                        case 4:
                            featureIcons.Add(new FeatureIcon("4", "RailVehicleFeaturesIcon.ImageURL.4", "RailVehicleFeaturesIcon.AltTextToolTip.4", "RailVehicleFeaturesIcon.AltTextToolTip.4", "RailVehicleFeaturesIcon.AltTextToolTip.4"));
                            break;
                        case 5:
                            featureIcons.Add(new FeatureIcon("5", "RailVehicleFeaturesIcon.ImageURL.5", "RailVehicleFeaturesIcon.AltTextToolTip.5", "RailVehicleFeaturesIcon.AltTextToolTip.5", "RailVehicleFeaturesIcon.AltTextToolTip.5"));
                            break;
                        case 10:
                            featureIcons.Add(new FeatureIcon("10", "RailVehicleFeaturesIcon.ImageURL.10", "RailVehicleFeaturesIcon.AltTextToolTip.10", "RailVehicleFeaturesIcon.AltTextToolTip.10", "RailVehicleFeaturesIcon.AltTextToolTip.10"));
                            break;
                        case 11:
                            featureIcons.Add(new FeatureIcon("11", "RailVehicleFeaturesIcon.ImageURL.11", "RailVehicleFeaturesIcon.AltTextToolTip.11", "RailVehicleFeaturesIcon.AltTextToolTip.11", "RailVehicleFeaturesIcon.AltTextToolTip.11"));
                            break;
                        case 12:
                            featureIcons.Add(new FeatureIcon("12", "RailVehicleFeaturesIcon.ImageURL.12", "RailVehicleFeaturesIcon.AltTextToolTip.12", "RailVehicleFeaturesIcon.AltTextToolTip.12", "RailVehicleFeaturesIcon.AltTextToolTip.12"));
                            break;
                        case 13:
                            featureIcons.Add(new FeatureIcon("13", "RailVehicleFeaturesIcon.ImageURL.13", "RailVehicleFeaturesIcon.AltTextToolTip.13", "RailVehicleFeaturesIcon.AltTextToolTip.13", "RailVehicleFeaturesIcon.AltTextToolTip.13"));
                            break;

                        // flags for more complex cases to be handled later ...
                        case 21:
                            feature21Present = true;
                            break;
                        case 22:
                            feature22Present = true;
                            break;
                        case 30:
                            feature30Present = true;
                            break;
                        case 31:
                            feature31Present = true;
                            break;
                        case 32:
                            feature32Present = true;
                            break;

                        default:
                            //do nothing
                            break;

                    }
                }

                // now do the more complex cases -- to avoid "1st" symbol appearing twice ...

                // if 20 or 21 present, the seating "1st" sysmbol will be displayed and 
                //  we should not use the combined 1st+berth symbols for 30 or 31 ...
                if (feature21Present)
                {
                    featureIcons.Add(new FeatureIcon("21", "RailVehicleFeaturesIcon.ImageURL.21", "RailVehicleFeaturesIcon.AltTextToolTip.21", "RailVehicleFeaturesIcon.AltTextToolTip.21", "RailVehicleFeaturesIcon.AltTextToolTip.21"));

                    if (feature30Present)
                    {
                        featureIcons.Add(new FeatureIcon("30", "RailVehicleFeaturesIcon.ImageURL.32", "RailVehicleFeaturesIcon.AltTextToolTip.30", "RailVehicleFeaturesIcon.AltTextToolTip.30", "RailVehicleFeaturesIcon.AltTextToolTip.30"));
                    }
                    else if (feature31Present)
                    {
                        featureIcons.Add(new FeatureIcon("31", "RailVehicleFeaturesIcon.ImageURL.32", "RailVehicleFeaturesIcon.AltTextToolTip.31", "RailVehicleFeaturesIcon.AltTextToolTip.31", "RailVehicleFeaturesIcon.AltTextToolTip.31"));
                    }
                }
                else if (feature22Present)
                {
                    featureIcons.Add(new FeatureIcon("22", "RailVehicleFeaturesIcon.ImageURL.22", "RailVehicleFeaturesIcon.AltTextToolTip.22", "RailVehicleFeaturesIcon.AltTextToolTip.22", "RailVehicleFeaturesIcon.AltTextToolTip.22"));

                    if (feature30Present)
                    {
                        featureIcons.Add(new FeatureIcon("30", "RailVehicleFeaturesIcon.ImageURL.32", "RailVehicleFeaturesIcon.AltTextToolTip.30", "RailVehicleFeaturesIcon.AltTextToolTip.30", "RailVehicleFeaturesIcon.AltTextToolTip.30"));
                    }
                    else if (feature31Present)
                    {
                        featureIcons.Add(new FeatureIcon("31", "RailVehicleFeaturesIcon.ImageURL.32", "RailVehicleFeaturesIcon.AltTextToolTip.31", "RailVehicleFeaturesIcon.AltTextToolTip.31", "RailVehicleFeaturesIcon.AltTextToolTip.31"));
                    }
                }
                else  // neither 21 nor 22 present, so we can use "combined" symbols for 30 or 31 ...
                {
                    if (feature30Present)
                    {
                        featureIcons.Add(new FeatureIcon("30", "RailVehicleFeaturesIcon.ImageURL.30", "RailVehicleFeaturesIcon.AltTextToolTip.30", "RailVehicleFeaturesIcon.AltTextToolTip.30", "RailVehicleFeaturesIcon.AltTextToolTip.30"));
                    }
                    else if (feature31Present)
                    {
                        featureIcons.Add(new FeatureIcon("31", "RailVehicleFeaturesIcon.ImageURL.31", "RailVehicleFeaturesIcon.AltTextToolTip.31", "RailVehicleFeaturesIcon.AltTextToolTip.31", "RailVehicleFeaturesIcon.AltTextToolTip.31"));
                    }
                }

                if (feature32Present)		// mutually exclusive with 30 and 31
                {
                    featureIcons.Add(new FeatureIcon("32", "RailVehicleFeaturesIcon.ImageURL.32", "RailVehicleFeaturesIcon.AltTextToolTip.32", "RailVehicleFeaturesIcon.AltTextToolTip.32", "RailVehicleFeaturesIcon.AltTextToolTip.32"));
                }
            }

            return featureIcons;
        }

        /// <summary>
        /// Returns a list of FeatureIcons for Accessible features
        /// </summary>
        /// <param name="Icons"></param>
        /// <returns></returns>
        public List<FeatureIcon> GetAccessibleFeatureIcons(List<SJPAccessibilityType> accessbilityTypes)
        {
            List<FeatureIcon> featureIcons = new List<FeatureIcon>();

            if (accessbilityTypes != null)
            {
                foreach (SJPAccessibilityType sat in accessbilityTypes)
                {
                    featureIcons.Add(new FeatureIcon(
                        sat.ToString(),
                        string.Format("AccessibleFeaturesIcon.ImageURL.{0}", sat.ToString()),
                        string.Format("AccessibleFeaturesIcon.AltTextToolTip.{0}", sat.ToString()),
                        string.Format("AccessibleFeaturesIcon.AltTextToolTip.{0}", sat.ToString()),
                        string.Format("AccessibleFeaturesIcon.AltTextToolTip.{0}", sat.ToString())));
                }
            }

            return featureIcons;
        }

        #endregion

    }
}
