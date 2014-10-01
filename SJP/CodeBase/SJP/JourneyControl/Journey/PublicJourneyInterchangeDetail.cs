// *********************************************** 
// NAME             : PublicJourneyInterchangeDetail.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 06 Apr 2011
// DESCRIPTION  	: PublicJourneyInterchangeDetail class representing the detail of a
// interchange journey leg, e.g. walking
// ************************************************
// 
using System;
using System.Collections.Generic;
using TransportDirect.JourneyPlanning.CJPInterface;
using SJP.Common.LocationService;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// PublicJourneyInterchangeDetail class representing the detail of a
    // interchange journey leg
    /// </summary>
    [Serializable()]
    public class PublicJourneyInterchangeDetail : PublicJourneyDetail
    {
        #region Private members

        private List<CheckConstraint> checkConstraints = new List<CheckConstraint>();
        private List<SJPAccessibilityType>legAccessibility = new List<SJPAccessibilityType>();
        
        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor - defined to allow XML serialisation.
        /// </summary>
        public PublicJourneyInterchangeDetail()
        {
        }

        /// <summary>
        /// Takes a CJP leg and creates a new intance of this class populated with
        /// the leg information
        /// </summary>
        /// <param name="leg">A journey leg passed back from the CJP</param>
        public PublicJourneyInterchangeDetail(Leg leg)
            : base(leg)
        {
            InterchangeLeg interchangeLeg = leg as InterchangeLeg;

            // Typical duration returned by CJP is in minutes
            durationSecs = interchangeLeg.typicalDuration * 60;

            if ((interchangeLeg.checkConstraints != null) && (interchangeLeg.checkConstraints.Length > 0))
            {
                // Constraint details applying to this interchange leg
                checkConstraints = new List<CheckConstraint>(interchangeLeg.checkConstraints);
            }

            if (interchangeLeg.legAccessibility != null)
            {
                // Accessible information applying to this interchange leg
                Accessibility cjpLegAccessibility = interchangeLeg.legAccessibility;

                legAccessibility = PopulateStopAccessibility(cjpLegAccessibility, false);
            }
            else if ((interchangeLeg.alight != null) && (interchangeLeg.alight.stop != null) && (interchangeLeg.alight.stop.accessibility != null))
            {
                // If no LegAccessibility, then use the accessible information applying to the alight
                Accessibility cjpAccessibilityAlight = interchangeLeg.alight.stop.accessibility;

                legAccessibility = PopulateStopAccessibility(cjpAccessibilityAlight, false);
            }

            if (interchangeLeg.navigationPath != null)
            {
                NavigationPath cjpNavPath = interchangeLeg.navigationPath;
                
                // Set coordinates for this interchange leg (used in handing off mapping of walk legs)
                if (cjpNavPath.pathLinks != null)
                {
                    List<OSGridReference> osgrs = new List<OSGridReference>();

                    foreach (PathLinkInSequence pl in cjpNavPath.pathLinks)
                    {
                        // From location
                        if ((pl.from != null) && (pl.from.coordinate != null))
                        {
                            // Create the OSGR
                            OSGridReference osgr = new OSGridReference(pl.from.coordinate.easting, pl.from.coordinate.northing);

                            if (osgr.IsValid)
                            {
                                // Add the OSGR to the array
                                osgrs.Add(osgr);
                            }
                        }

                        // To location
                        if ((pl.to != null) && (pl.to.coordinate != null))
                        {
                            // Create the OSGR
                            OSGridReference osgr = new OSGridReference(pl.to.coordinate.easting, pl.to.coordinate.northing);

                            if (osgr.IsValid)
                            {
                                // Add the OSGR to the array
                                osgrs.Add(osgr);
                            }
                        }
                    }

                    this.geometry = new Dictionary<int, OSGridReference[]>();
                    this.geometry.Add(0, osgrs.ToArray());
                }
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read/Write. Constraint details applying to this interchange leg
        /// </summary>
        public List<CheckConstraint> CheckConstraints
        {
            get { return checkConstraints; }
            set { checkConstraints = value; }
        }

        /// <summary>
        /// Read/Write. Accessibility details for the interchange leg
        /// </summary>
        public List<SJPAccessibilityType> InterchangeLegAccessibility
        {
            get { return legAccessibility; }
            set { legAccessibility = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns true if this PublicJourneyDetail contains "check constraints" which 
        /// requires an instruction to be displayed in the UI.
        /// May only be true for PublicJourneyInterchangeDetail detail types
        /// </summary>
        public override bool HasCheckConstraint()
        {
            if (checkConstraints != null)
            {
                // Only want to display a check constraint instruction for a "security" or "egress" check constraint,
                foreach (CheckConstraint cc in checkConstraints)
                {
                    if ((cc.checkProcess == CheckProcess.securityCheck)
                        || (cc.checkProcess == CheckProcess.baggageSecurityCheck)
                        || (cc.checkProcess == CheckProcess.egress))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Read only. Duration (seconds) total for the check constraints within this PublicJourneyInterchangeDetail
        /// </summary>
        public int DurationCheckConstraints
        {
            get 
            {
                int durationSecsCC = 0;

                if (checkConstraints != null)
                {
                    foreach (CheckConstraint cc in checkConstraints)
                    {
                        durationSecsCC += cc.averageDelay * 60;
                    }
                }

                return durationSecsCC;
            }
        }

        #endregion
    }
}
