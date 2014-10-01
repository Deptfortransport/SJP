// *********************************************** 
// NAME             : JourneyDetail.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 25 Mar 2011
// DESCRIPTION  	: JourneyDetail base class containing common details to JourneyDetailPublic, 
// JourneyDetailRoad, and JourneyDetailCycle
// ************************************************
// 


using System;
using System.Collections.Generic;
using SJP.Common;
using SJP.Common.LocationService;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// JourneyDetail base class
    /// </summary>
    [Serializable()]
    public class JourneyDetail
    {
        #region Private members

        protected SJPModeType mode;
        protected int durationSecs;
        protected int distance;
        protected Dictionary<int, OSGridReference[]> geometry = new Dictionary<int,OSGridReference[]>();
        protected LatitudeLongitude[] latlongCoordinates = new LatitudeLongitude[0];

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public JourneyDetail()
        {
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read/Write. Mode for this journey detail
        /// </summary>
        public SJPModeType Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        /// <summary>
        /// Read/Write. Duration (seconds) for this journey detail
        /// </summary>
        public int Duration
        {
            get { return durationSecs; }
            set { durationSecs = value; }
        }

        /// <summary>
        /// Read/Write. Distance (metres) for this journey detail
        /// </summary>
        /// <remarks>
        /// For a PT journey leg, this value is not available.
        /// </remarks>
        public int Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        /// <summary>
        /// Read/Write. The geometry in OSGR format
        /// </summary>
        public Dictionary<int, OSGridReference[]> Geometry
        {
            get { return geometry; }
            set { geometry = value; }
        }

        /// <summary>
        /// Read/Write. The latitude longitude coordinates for this detail.
        /// Assumes the SJPJourneyResult object has populated this array.
        /// </summary>
        public LatitudeLongitude[] LatitudeLongitudes
        {
            get { return latlongCoordinates; }
            set { latlongCoordinates = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Method returns the OSGR coordinates (the Geometry) of this detail, removing all the 
        /// grouping of the coordiantes and returning one array of OSGRs
        /// </summary>
        /// <returns></returns>
        public OSGridReference[] GetAllOSGRGridReferences()
        {
            // Temp array used to group together the coordinates
            List<OSGridReference> tempGridReferences = new List<OSGridReference>();

            if (geometry != null && geometry.Count > 0)
            {
                // Loop through each geometry there is for this detail
                foreach (KeyValuePair<int, OSGridReference[]> kvpGeometry in geometry)
                {
                    for (int i = 0; i < (kvpGeometry.Value.Length); i++)
                    {
                        OSGridReference osgr = kvpGeometry.Value[i];

                        if (osgr.IsValid)
                        {
                            tempGridReferences.Add(osgr);
                        }
                    }
                }
            }

            // Filter the grid references to remove adjacent OSGR duplicates. This is needed because each OSGR array
            // in the Geometry dictionary (may) Starts with the previous's End OSGR.
            List<OSGridReference> gridReferences = new List<OSGridReference>();

            OSGridReference current = new OSGridReference();
            OSGridReference previous = new OSGridReference();

            for (int j = 0; j < tempGridReferences.Count; j++)
            {
                current = tempGridReferences[j];

                // Is this a duplicate?
                if ((current.Easting == previous.Easting) && (current.Northing == previous.Northing))
                {
                    // The current OSGR matches the previous OSGR so do not add.
                }
                else
                {
                    gridReferences.Add(current);
                }

                // Assign the previous OSGR ready for the next loop
                previous = current;
            }

            return gridReferences.ToArray();
        }

        /// <summary>
        /// Method which populates the Latitude Longitude coordiantes for this detail with coordinates provided
        /// </summary>
        /// <param name="coordinates"></param>
        public void UpdateLatitudeLongitudeCoordinates(LatitudeLongitude[] coordinates)
        {
            if (coordinates != null)
            {
                latlongCoordinates = coordinates;
            }
        }

        #endregion
    }
}
