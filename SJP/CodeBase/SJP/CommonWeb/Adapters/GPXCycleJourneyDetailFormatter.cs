// *********************************************** 
// NAME             : GPXCycleJourneyDetailFormatter.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 13 May 2011
// DESCRIPTION  	: Cycle journey detail formatter for the GPX file download
// ************************************************


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using SJP.Common.EventLogging;
using SJP.Common.LocationService;
using SJP.Common.ResourceManager;
using SJP.UserPortal.CyclePlannerService.CyclePlannerWebService;
using SJP.UserPortal.JourneyControl;
using Logger = System.Diagnostics.Trace;

namespace SJP.Common.Web
{
    /// <summary>
    /// Cycle journey detail formatter for the GPX file download
    /// </summary>
    public class GPXCycleJourneyDetailFormatter : CycleJourneyDetailFormatter
    {
       #region Constructors

        /// <summary>
        /// Constructs a Formatter
        /// </summary>
        /// <param name="cycleJourney"></param>
        /// <param name="currentLanguage"></param>
        public GPXCycleJourneyDetailFormatter(
            JourneyLeg cycleJourney,
            Language currentLanguage,
            SJPResourceManager resourceManager)
            : base(cycleJourney, currentLanguage,true, resourceManager, false)
		{
		}

		#endregion Constructors

        #region Public static Methods
        /// <summary>
        /// Creates a formatter for specified journey result
        /// </summary>
        /// <param name="result"></param>
        public static GPXCycleJourneyDetailFormatter GetGPXCycleJourneyDetailFormatter(ISJPJourneyResult result, Language currentLanguage, SJPResourceManager resourceManager)
        {
            JourneyLeg cycleLeg = null;

            return new GPXCycleJourneyDetailFormatter(cycleLeg, currentLanguage, resourceManager);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Generates the GPX file for a cycle journey.
        /// Saves the file on a file share and returns the full path to the file
        /// </summary>
        /// <returns>Byte array representing the gpx file</returns>
        public byte[] GenerateGPXFile()
        {
            try
            {

                string message = "CycleJourneyGPXDownload page - generating GPX file for cycle journey.";
                OperationalEvent operationalEvent = new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, message);
                Logger.Write(operationalEvent);

                JourneyLeg cycleJourney = journey;

                if (cycleJourney != null)
                {
                    // Check the latitude longitude coordinates were set correctly during the journey response processing from the Cycle Trip Planner.
                    // If at least one detail contains LatLong then assume ok.
                    bool latitudeLongitudesAreValid = false;

                    var detailWithPopulatedLatLong = cycleJourney.JourneyDetails
                                                    .Where(jd => jd.LatitudeLongitudes != null)
                                                    .Union(
                                                        cycleJourney.JourneyDetails
                                                            .Where(jd => jd.LatitudeLongitudes.Length > 0)
                                                    );
                    latitudeLongitudesAreValid = detailWithPopulatedLatLong.Count() > 0;

                    if (latitudeLongitudesAreValid)
                    {
                        // local variables
                        string dateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ";
                        string journeyName = cycleJourney.LegStart.Location.DisplayName + " to " + cycleJourney.LegEnd.Location.DisplayName;

                        double lattitude = 0;
                        double longitude = 0;
                        double startLat = 0;
                        double startLon = 0;
                        double endLat = 0;
                        double endLon = 0;

                        // Start coordinates
                        startLat = cycleJourney.LegStart.Location.LatitudeLongitudeCoordinate.Latitude;
                        startLon = cycleJourney.LegStart.Location.LatitudeLongitudeCoordinate.Longitude;

                        // End coordinates
                        endLat = cycleJourney.LegEnd.Location.LatitudeLongitudeCoordinate.Latitude;
                        endLon = cycleJourney.LegEnd.Location.LatitudeLongitudeCoordinate.Longitude;

                        // Xml document settings
                        XmlWriterSettings settings = new XmlWriterSettings();
                        settings.Encoding = Encoding.UTF8;
                        settings.Indent = true;
                        settings.ConformanceLevel = ConformanceLevel.Document;
                        settings.OmitXmlDeclaration = false;

                        try
                        {
                            using (MemoryStream ms = new MemoryStream())
                            using (StreamWriter xmlStream = new StreamWriter(ms, Encoding.UTF8))
                            // Create and set up the GPX file
                            using (XmlWriter xmlWriter = XmlWriter.Create(xmlStream, settings))
                            {
                                xmlWriter.WriteStartDocument();

                                #region Header (MetaData)
                                // <gpx>
                                xmlWriter.WriteStartElement("gpx", "http://www.topografix.com/GPX/1/1");

                                xmlWriter.WriteAttributeString("xmlns", "xsd", null, "http://www.w3.org/2001/XMLSchema");
                                xmlWriter.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                                xmlWriter.WriteAttributeString("xsi", "schemaLocation", null, "http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd");
                                xmlWriter.WriteAttributeString("version", "1.1");
                                xmlWriter.WriteAttributeString("creator", "TransportDirect.info");

                                // <metadata>
                                xmlWriter.WriteStartElement("metadata");

                                // <desc>
                                xmlWriter.WriteStartElement("desc");
                                xmlWriter.WriteString("GPX file for " + journeyName);
                                xmlWriter.WriteEndElement();

                                // <author>
                                xmlWriter.WriteStartElement("author");
                                xmlWriter.WriteStartElement("name");
                                xmlWriter.WriteString("TransportDirect.info");
                                xmlWriter.WriteEndElement(); //</name>
                                xmlWriter.WriteEndElement(); //</author>

                                // <time>
                                xmlWriter.WriteStartElement("time");
                                xmlWriter.WriteString(DateTime.Now.ToString(dateTimeFormat));
                                xmlWriter.WriteEndElement();

                                xmlWriter.WriteEndElement(); // </metadata>
                                #endregion

                                #region Locations (Waypoints)

                                // Start Location
                                xmlWriter.WriteStartElement("wpt");
                                xmlWriter.WriteAttributeString("lat", startLat.ToString());
                                xmlWriter.WriteAttributeString("lon", startLon.ToString());

                                xmlWriter.WriteStartElement("name");
                                xmlWriter.WriteValue("Start: " + cycleJourney.LegStart.Location.DisplayName);
                                xmlWriter.WriteEndElement(); // </name>                      

                                xmlWriter.WriteStartElement("desc");
                                xmlWriter.WriteString("Start of journey at " + cycleJourney.LegStart.Location.DisplayName);
                                xmlWriter.WriteEndElement(); // </desc>

                                xmlWriter.WriteStartElement("sym");
                                xmlWriter.WriteValue("Waypoint");
                                xmlWriter.WriteEndElement(); // </sym>

                                xmlWriter.WriteEndElement(); // </wpt>

                                // End Location
                                xmlWriter.WriteStartElement("wpt");
                                xmlWriter.WriteAttributeString("lat", endLat.ToString());
                                xmlWriter.WriteAttributeString("lon", endLon.ToString());

                                xmlWriter.WriteStartElement("name");
                                xmlWriter.WriteValue("End: " + cycleJourney.LegEnd.Location.DisplayName);
                                xmlWriter.WriteEndElement(); // </name>                      

                                xmlWriter.WriteStartElement("desc");
                                xmlWriter.WriteString("End of journey at " + cycleJourney.LegEnd.Location.DisplayName);
                                xmlWriter.WriteEndElement(); // </desc>

                                xmlWriter.WriteStartElement("sym");
                                xmlWriter.WriteValue("Waypoint");
                                xmlWriter.WriteEndElement(); // </sym>

                                xmlWriter.WriteEndElement(); // </wpt>

                                #endregion

                                List<FormattedJourneyDetail> journeyDetails = GetJourneyDetails();

                                // Get the actual cycle journey detail objects
                                List<CycleJourneyDetail> cycleJourneyDetails = cycleJourney.JourneyDetails.Cast<CycleJourneyDetail>().ToList();

                                // Get the coordinates for the entire journey
                                List<LatitudeLongitude> gridReferences = GetAllLatitudeLongitudes();

                                #region Route (including the journey directions)

                                // <rte>
                                xmlWriter.WriteStartElement("rte");

                                xmlWriter.WriteStartElement("name");
                                xmlWriter.WriteString(journeyName);
                                xmlWriter.WriteEndElement(); // </name>

                                string direction = string.Empty;
                                string symbol = "Dot";

                                // Loop through the directions (first and last are start and end locations)
                                for (int i = 0; i < journeyDetails.Count; i++)
                                {
                                    FormattedCycleJourneyDetail cycleFormattedDetail = journeyDetails[i] as FormattedCycleJourneyDetail;
                                    // Get the direction instruction
                                    direction = cycleFormattedDetail.Instruction;


                                    if (i == 0) // First direction
                                    {
                                        #region Start
                                        // Set the lat/lon from First grid reference in journey
                                        lattitude = gridReferences[0].Latitude;
                                        longitude = gridReferences[0].Longitude;

                                        WriteRoutePoint(xmlWriter, lattitude.ToString(), longitude.ToString(), direction, symbol);
                                        #endregion
                                    }
                                    else if (i == journeyDetails.Count - 1) // Last direction
                                    {
                                        #region End
                                        // Set the lat/lon from Last grid reference in journey
                                        lattitude = gridReferences[gridReferences.Count - 1].Latitude;
                                        longitude = gridReferences[gridReferences.Count - 1].Longitude;

                                        WriteRoutePoint(xmlWriter, lattitude.ToString(), longitude.ToString(), direction, symbol);
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Directions
                                        // Get the detail the instruction relates to
                                        CycleJourneyDetail cjd = cycleJourneyDetails[i - 1];
                                        bool writeDirection = true;

                                        // Write the points contained for this CycleJourneyDetail
                                        LatitudeLongitude[] coordinates = cjd.LatitudeLongitudes;

                                        if (coordinates != null && coordinates.Length > 0)
                                        {
                                            // loop through each coordinate there is for this detail, ignoring the last 
                                            // one in each array as this is used as the start for the next detail
                                            int items = (coordinates.Length == 1) ? 1 : (coordinates.Length - 1);

                                            for (int j = 0; j < items; j++)
                                            {
                                                lattitude = coordinates[j].Latitude;
                                                longitude = coordinates[j].Longitude;

                                                if (writeDirection)
                                                {
                                                    // For the first osgr on this detail, add the Direction instruction
                                                    WriteRoutePoint(xmlWriter, lattitude.ToString(), longitude.ToString(), direction, symbol);

                                                    writeDirection = false;
                                                }
                                                else
                                                {
                                                    WriteRoutePoint(xmlWriter, lattitude.ToString(), longitude.ToString(), string.Empty, string.Empty);
                                                }
                                            }
                                        }

                                        #endregion
                                    }
                                }

                                xmlWriter.WriteEndElement(); //</rte>

                                #endregion

                                xmlWriter.WriteEndElement(); // </gpx>

                                xmlWriter.WriteEndDocument();

                                xmlWriter.Flush();

                                // return gpx xml
                                return ms.ToArray();
                            }
                        }
                        catch (Exception ex)
                        {
                            message = string.Format("GPX file create failed  - CycleJourney [{0} to {1}]. A problem occurred while creating GPX xml file",
                                cycleJourney.LegStart.Location.DisplayName, cycleJourney.LegEnd.Location.DisplayName);
                            SJPException SJPException = new SJPException(message, ex, true, SJPExceptionIdentifier.CYXmlGPXFileTransformationFailed);
                            throw SJPException;
                        }
                    }
                    else
                    {
                        message = string.Format("GPX file create failed  - CycleJourney [{0} to {1}]. The latitude and longitude valid flag was false. A problem must have occurred when processing the journey response from the Cycle Trip Planner.",
                            cycleJourney.LegStart.Location.DisplayName, cycleJourney.LegEnd.Location.DisplayName);
                        SJPException SJPException = new SJPException(message, true, SJPExceptionIdentifier.CYXmlGPXFileTransformationFailed);
                        throw SJPException;
                    }

                }
                else
                {
                    SJPException SJPException = new SJPException("GPX file create failed  - CycleJourney was null", true, SJPExceptionIdentifier.CYXmlGPXFileTransformationFailed);
                    throw SJPException;
                }

            }
            catch (SJPException tdEx)
            {
                OperationalEvent operationalEvent = new OperationalEvent
                    (SJPEventCategory.Infrastructure, SJPTraceLevel.Error, tdEx.Message);
                Logger.Write(operationalEvent);
            }
            catch (Exception ex)
            {
                OperationalEvent operationalEvent = new OperationalEvent
                    (SJPEventCategory.Infrastructure, SJPTraceLevel.Error, ex.Message, ex);
                Logger.Write(operationalEvent);
            }
        
            // Errors have occurred so return an empty gpx xml
            return null;
        }
               
        #endregion

        #region Private Methods
        /// <summary>
        /// Writes a rtept to the XmlWriter
        /// </summary>
        /// <param name="xmlWriter"></param>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <param name="name"></param>
        /// <param name="symbol"></param>
        private void WriteRoutePoint(XmlWriter xmlWriter, string lat, string lon, string name, string symbol)
        {
            xmlWriter.WriteStartElement("rtept");

            xmlWriter.WriteAttributeString("lat", lat);
            xmlWriter.WriteAttributeString("lon", lon);

            if (!string.IsNullOrEmpty(name))
            {
                xmlWriter.WriteStartElement("name");
                xmlWriter.WriteString(name);
                xmlWriter.WriteEndElement(); // </name>
            }

            if (!string.IsNullOrEmpty(symbol))
            {
                xmlWriter.WriteStartElement("sym");
                xmlWriter.WriteValue(symbol);
                xmlWriter.WriteEndElement(); // </sym>
            }

            xmlWriter.WriteEndElement(); // </rtept>
        }

        /// <summary>
        /// Returns all LatitudeLongitudes for the journey as an array of LatitudeLongitudes.
        /// </summary>
        /// <returns></returns>
        private List<LatitudeLongitude> GetAllLatitudeLongitudes()
        {
            // Temp array used to group together polylines
            List<LatitudeLongitude> gridReferences = new List<LatitudeLongitude>();

            foreach (JourneyDetail detail in journey.JourneyDetails)
            {
                // Get the geometry values for this detail
                LatitudeLongitude[] latlongs = detail.LatitudeLongitudes;

                if (latlongs != null && latlongs.Length > 0)
                {
                    gridReferences.AddRange(latlongs);
                }
            }

            return gridReferences;
        }


        #endregion

        #region Override Protected methods

        /// <summary>
        /// A hook method called by ProcessJourney to process each cycle journey
        /// instruction. The returned string array contains formatted details for
        /// step number, total distance, directions and arrival time in that order
        /// </summary>
        /// <param name="journeyDetailIndex">Array index into cycleJourney.Details</param>
        /// <returns>details for each instruction</returns>
        protected override FormattedJourneyDetail ProcessJourneyDetail(int journeyDetailIndex, bool showCongestionCharge)
        {
            FormattedCycleJourneyDetail details = new FormattedCycleJourneyDetail();

            // Note that the Accumulated Distance is updated in GetDirections, therefore the TotalDistance
            // displayed is the total prior to the current instruction rather than after it	
            CycleJourneyDetail cycleDetail = journey.JourneyDetails[journeyDetailIndex] as CycleJourneyDetail;

            details.Step = GetCurrentStepNumber();

            details.TotalDistance = TotalDistance;

            if (cycleDetail.StopoverSection)
            {
                details.TotalDistance = null;
            }

            //is the journey a return and was congestion charge shown for the outward already?
            details.Instruction = GetDirections(journeyDetailIndex, showCongestionCharge);
            details.PathName = GetCyclePathName(journeyDetailIndex);
            details.PathImageUrl = GetCyclePathImage(journeyDetailIndex);
            details.PathImageText = GetCyclePathImageText(journeyDetailIndex);
            details.ManoeuvreImage = GetManoeuvreImageUrl();
            details.ManoeuvreImageText = GetManoeuvreImageText();
            details.CycleInstruction = GetCycleInstructionDetails(journeyDetailIndex);
            details.CycleAttributes = GetCycleAttributeDetails(journeyDetailIndex, true);
            details.ArriveTime = GetArrivalTime(journeyDetailIndex);
            details.Distance = GetDistance(journeyDetailIndex);

            return details;
        }

        /// <summary>
        /// A hook method called by ProcessCycleJourney to add the first element to the ordered list 
        /// of journey instructions. The returned string array contains step number, a "Leave from ..." 
        /// instruction and "not applicable" indicators for accumulated distance and arrival time.
        /// </summary>
        /// <returns>details for first instruction</returns>
        protected override FormattedJourneyDetail AddFirstDetailLine()
        {
            FormattedCycleJourneyDetail details = new FormattedCycleJourneyDetail();

            //step number
            details.Step = GetCurrentStepNumber();

            //accumulated distance
            details.TotalDistance = TotalDistance;

            //instruction
            details.Instruction = leaveFrom + journey.LegStart.Location.DisplayName;

            //cycle path name
            details.PathName = string.Empty;

            //cycle path image
            details.PathImageUrl = string.Empty;

            details.ManoeuvreImage = string.Empty;

            //cycle instruction text
            details.CycleInstruction = string.Empty;

            //cycle attribute details
            details.CycleAttributes = string.Empty;

            //arrival time
            details.ArriveTime = null;


            return details;
        }

        /// <summary>
        /// A hook method called by ProcessCycleJourney to add the last element to the 
        /// ordered list of journey instructions. The returned string array contains step number,
        /// an "arrive at ..." instruction and arrival time, and an empty string for accumulated distance
        /// </summary>
        /// <returns>details for last instruction</returns>
        protected override FormattedJourneyDetail AddLastDetailLine()
        {
            FormattedCycleJourneyDetail details = new FormattedCycleJourneyDetail();

            //step number
            details.Step = GetCurrentStepNumber();

            int journeyDetailIndex = journey.JourneyDetails.Count - 1;

            //accumulated distance
            details.TotalDistance = TotalDistance;

            //instruction
            details.Instruction = arriveAt + journey.LegEnd.Location.DisplayName;

            //cycle path name
            details.PathName = string.Empty;

            //cycle path image
            details.PathImageUrl = string.Empty;

            details.ManoeuvreImage = string.Empty;

            //cycle instruction text
            details.CycleInstruction = string.Empty;

            //cycle attribute details
            details.CycleAttributes = string.Empty;

            //arrival time
            details.ArriveTime = GetArrivalTime(journeyDetailIndex);


            return details;
        }

        /// <summary>
        /// Adds 'continue for...' text to the journey directions
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="nextDetailHasJunctionExitJunction"></param>
        /// <param name="routeText"></param>
        /// <returns></returns>
        protected override string AddContinueFor(CycleJourneyDetail detail,
            bool nextDetailHasJunctionExitJunction, string routeText)
        {
            //convert metres to miles
            string milesDistance = ConvertMetresToMileage(detail.Distance);
            string distanceInKm = ConvertMetresToKm(detail.Distance);
            string distanceInMiles = string.Empty;
            string kmDistance = string.Empty;
            string ferryCrossing = GetResourceString("RouteText.FerryCrossing");

            //Switches the default display of either Miles or Kms depending on roadUnits

            distanceInMiles =  milesDistance + space + miles;
            kmDistance = distanceInKm + space + "km";


            //check if this text should be added
            if (detail.Ferry)
            {
                //no need to add the "continues for..." message in these situations
                routeText = ferryCrossing;

                return routeText;
            }

            else if (
                (detail.Distance <= immediateTurnDistance) &&

                !((detail.JunctionType == JunctionType.Entry) &&
                detail.JunctionSection &&
                (detail.PlaceName != null && detail.PlaceName.Length > 0) &&
                nextDetailHasJunctionExitJunction) &&

                !(detail.JunctionSection &&
                ((detail.JunctionType == JunctionType.Exit) ||
                (detail.JunctionType == JunctionType.Merge)))
                )
            {
                //no need to add the "continues for..." message in these situations
                return routeText;

            }

            else if ((detail.SlipRoad) && (detail.Distance < slipRoadDistance) &&

                !((detail.JunctionType == JunctionType.Entry) &&
                detail.JunctionSection &&
                (detail.PlaceName != null && detail.PlaceName.Length > 0) &&
                nextDetailHasJunctionExitJunction) &&

                !(detail.JunctionSection &&
                ((detail.JunctionType == JunctionType.Exit) ||
                (detail.JunctionType == JunctionType.Merge)) &&
                ((detail.JunctionNumber != null) && (detail.JunctionNumber.Length > 0))))
            {
                //no need to add the "continues for..." message in these situations
                return routeText;
            }
            else
            {
                //in all other cases add "continues for..." to the end of the instruction 
                routeText = routeText + continueFor + space + kmDistance;

            }

            return routeText;
        }

        /// <summary>
        /// Returns formatted string of the road name for the supplied
        /// instruction. Overrides default implmentation to enclose road
        /// name in HTML bold element.
        /// </summary>
        /// <param name="detail">Details of journey instruction</param>
        /// <returns>The road name</returns>
        protected override string FormatRoadName(CycleJourneyDetail detail)
        {
            //Check for a motorway
            if (detail.RoadNumber != null)
            {

                if ((detail.RoadNumber.StartsWith("M") | (detail.RoadNumber.EndsWith("(M)"))) && (detail.RoadName != string.Empty))
                {
                    return detail.RoadNumber + " (" + detail.RoadName + ")";
                }
                else if ((detail.RoadNumber.StartsWith("M")) | (detail.RoadNumber.EndsWith("(M)")))
                {
                    return  detail.RoadNumber ;
                }
                else
                {
                    return base.FormatRoadName(detail);
                }
            }
            else
            {
                return " ";
            }
        }

        /// <summary>
        /// A hook method called by ProcessCycleJourney to filter the details returned to only include
        /// the instructions at the start and end index
        /// </summary>
        /// <returns>Returns an ordered list of journey instructions where each 
        /// element is a string array of details (e.g, road name, distance, directions), filtered 
        /// to only include instructions between the start and end indexs</returns>
        protected override List<FormattedJourneyDetail> FilterCycleJourneyDetails(List<FormattedJourneyDetail> details, int startIndex, int endIndex)
        {
            List<FormattedJourneyDetail> filteredDetails = new List<FormattedJourneyDetail>();

            // The details array passed in will have the actual instructions, 
            // plus a first and last instruction which says leave/arrive at location.
            // The start/end index supplied do not "know" about these additional instructions, 
            // therefore, we need to include these instructions when applicable.

            for (int i = 0; i < details.Count; i++)
            {
                FormattedJourneyDetail detail = details[i];

                int stepNumber = detail.Step;

                // start index is "actual" first instruction, so also need to add the "leaving from" direction
                if ((startIndex == 1) && (stepNumber == 1))
                {
                    filteredDetails.Add(detail);
                }
                // end index is "actual" last instruction, so also need to add the "arrive at" direction
                else if ((endIndex == details.Count - 2) && (stepNumber == details.Count))
                {
                    filteredDetails.Add(detail);
                }
                // this detail is within the range asked for, so add to filtered array
                else if ((stepNumber >= startIndex + 1) && (stepNumber <= endIndex + 1))
                {
                    filteredDetails.Add(detail);
                }
            }

            return filteredDetails;
        }


        #endregion



    }
}