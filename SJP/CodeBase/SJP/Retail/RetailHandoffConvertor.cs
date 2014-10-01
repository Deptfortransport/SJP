// *********************************************** 
// NAME             : RetailHandoffConvertor.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 08 Apr 2011
// DESCRIPTION  	: RetailHandoffConvertor class to convert an SJP Journey into a retail handoff xml document
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using SJP.Common;
using SJP.Common.EventLogging;
using SJP.Common.Extenders;
using SJP.Common.LocationService;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;
using SJP.UserPortal.JourneyControl;
using Logger = System.Diagnostics.Trace;

namespace SJP.UserPortal.Retail
{
    /// <summary>
    /// RetailHandoffConvertor class to convert an SJP Journey into a retail handoff xml document
    /// </summary>
    public sealed class RetailHandoffConvertor
    {
        #region Private members

        // const values for the JourneyWeb2.1 Origin and Destination NaPTAN values;
        private const string ORIGIN_NAPTAN = "Origin";
        private const string DESTINATION_NAPTAN = "Destination";

        // format for all datetimes added to xml
        private const string DATETIME_FORMAT = "yyyy-MM-ddTHH:mm:ss";

        // Element names
        private const string ELE_Root = "BookingHandoff";
        private const string ELE_Reference = "HandOffReference";
        private const string ELE_Generated = "DateTimeGenerated";
        private const string ELE_Journeys = "Journeys";
        private const string ELE_Accesible = "AccessibleJourneys";
        private const string ELE_Journey = "Journey";
        private const string ELE_Outward = "OutwardJourney";
        private const string ELE_JourneyLeg = "JourneyLeg";
        private const string ELE_LegId = "LegId";
        private const string ELE_LegBoard = "LegBoard";
        private const string ELE_LegAlight = "LegAlight";
        private const string ELE_LegBoardTime = "LegBoardTime";
        private const string ELE_LegAlightTime = "LegAlightTime";
        private const string ELE_OperatorCode = "OperatorCode";
        private const string ELE_OperatorName = "OperatorName";
        private const string ELE_ServiceNumber = "ServiceNumber";
        private const string ELE_LegMode = "LegMode";
        private const string ELE_LegInGamesTravelcardZone = "LegInGamesTravelcardZone";
        private const string ELE_SiteName = "SiteName";
        private const string ELE_SiteAtcoCode = "SiteAtcoCode";

        // Dummy NaPTAN to use where location has invalid NaPTAN. 
        // This is to satisfy the retail handoff schema which states the naptan field must be contain 
        // a value in this format
        private const string DUMMY_NAPTAN = "0000XXXXXX";
            
        #endregion

        #region Public methods

        /// <summary>
        /// Generates the Retail Handoff XML document for the supplied journeys
        /// </summary>
        /// <param name="sessionId">Used for adding a reference identifier</param>
        /// <param name="journeyOutward">The outward journey</param>
        /// <param name="journeyReturn">The return journey</param>
        /// <param name="outwardOriginNaptan">(Optional) NaPTAN to use for the outward journey start location if no NaPTAN exists</param>
        /// <param name="outwardDestinationNaptan">(Optional) NaPTAN to use for the outward journey end location if no NaPTAN exists</param>
        /// <param name="returnOriginNaptan">(Optional) NaPTAN to use for the return journey start location if no NaPTAN exists</param>
        /// <param name="returnDestinationNaptan">(Optional) NaPTAN to use for the return journey end location if no NaPTAN exists</param>
        /// <returns>The XML document</returns>
        public static IXPathNavigable GenerateHandoffXml(string sessionId, Journey journeyOutward, Journey journeyReturn,
            string outwardOriginNaptan, string outwardDestinationNaptan,
            string returnOriginNaptan, string returnDestinationNaptan)
        {
            IPropertyProvider pp = Properties.Current;

            XmlDataDocument result = new XmlDataDocument();

            string xmlns = pp["Retail.RetailHandoffXml.Xmlns"];
            string xmlnsxs = pp["Retail.RetailHandoffXml.Xmlns.Xs"];

            XmlElement rootElement = result.CreateElement(ELE_Root);
            rootElement.SetAttribute("xmlns", xmlns);
            rootElement.SetAttribute("xmlns:xs", xmlnsxs);

            // Add XML declaration
            XmlDeclaration xmldecl = result.CreateXmlDeclaration("1.0", "UTF-8", "");
            XmlElement doc = result.DocumentElement;
            result.InsertBefore(xmldecl, doc);

            #region Add handoff details

            // Add reference
            rootElement.AppendChild(AddReference(result, sessionId));
            
            // Add datetime
            rootElement.AppendChild(AddDateTimeGenerated(result));

            // Add journeys
            rootElement.AppendChild(AddJourneys(result, journeyOutward, journeyReturn, 
                outwardOriginNaptan, outwardDestinationNaptan,
                returnOriginNaptan, returnDestinationNaptan));

            // Handoff details all created, add to the xml result
            result.AppendChild(rootElement);

            #endregion

            // Log before validating to allow parse errors to be detected if raised
            if (SJPTraceSwitch.TraceVerbose)
            {
                // Log handoff xml
                Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Verbose,
                    result.OuterXml.ToString()));
            }

            #region Validate 

            // Get the schema to validate against
            RetailHandoffSchema handoffSchema = SJPServiceDiscovery.Current.Get<RetailHandoffSchema>(ServiceDiscoveryKey.RetailerHandoffSchema);

            // Validate newly created document against schema
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas.Add(handoffSchema.Schema);
            settings.ConformanceLevel = ConformanceLevel.Document;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);

            using (StringReader stringReader = new StringReader(result.OuterXml))
            {
                XmlReader reader = XmlReader.Create(stringReader, settings);

                // Load and Validate the xml
                XmlDocument document = new XmlDocument();
                document.Load(reader);
                document.Validate(ValidationHandler);
            }
            
            #endregion
            
            return (IXPathNavigable)result;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Called by the validation handler if an error occurs during validation.
        /// An operational event is logged and a exception is thrown.
        /// </summary>
        /// <param name="sender">Notifying object</param>
        /// <param name="args">Event arguments</param>
        private static void ValidationHandler(object sender, ValidationEventArgs args)
        {
            string message = string.Format("Error occurred validating Retail Handoff XML: {0}. Severity {1}",
                args.Message,
                args.Severity);
            
            Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, message, args.Exception));
            
            throw new SJPException(message, args.Exception, true, SJPExceptionIdentifier.REErrorValidatingHandoffXml);
        }

        #region Add XML elements

        /// <summary>
        /// Creates a reference number xml element
        /// </summary>
        private static XmlElement AddReference(XmlDocument doc, string referenceNumber)
        {
            XmlElement eleReference = doc.CreateElement(ELE_Reference);
            eleReference.InnerText = referenceNumber;

            return eleReference;
        }

        /// <summary>
        /// Creates a date time generated xml element
        /// </summary>
        private static XmlElement AddDateTimeGenerated(XmlDocument doc)
        {
            XmlElement eleDateTimeGenerated = doc.CreateElement(ELE_Generated);
            //2011-06-30T09:00:00
            eleDateTimeGenerated.InnerText = DateTime.Now.ToString(DATETIME_FORMAT);

            return eleDateTimeGenerated;
        }

        /// <summary>
        /// Creates a date time generated xml element
        /// </summary>
        private static XmlElement AddJourneys(XmlDocument doc, Journey journeyOutward, Journey journeyReturn,
            string outwardOriginNaptan, string outwardDestinationNaptan,
            string returnOriginNaptan, string returnDestinationNaptan)
        {
            XmlElement eleJourneys = doc.CreateElement(ELE_Journeys);

            #region Accessible flag

            bool accessible = false;
            if ((journeyOutward != null) && (journeyOutward.AccessibleJourney))
                accessible = true;
            else if ((journeyReturn != null) && (journeyReturn.AccessibleJourney))
                accessible = true;

            XmlElement eleAccessible = doc.CreateElement(ELE_Accesible);
            eleAccessible.InnerText =  accessible.ToString().ToLower();

            eleJourneys.AppendChild(eleAccessible);

            #endregion

            XmlElement eleJourney = null;
            XmlElement eleOutward = null;
            
            int legCount = -1;
            bool firstLeg = false;
            bool lastLeg = false;

            // Outward journey
            if (journeyOutward != null)
            {
                eleJourney = doc.CreateElement(ELE_Journey);

                eleOutward = doc.CreateElement(ELE_Outward);
                eleOutward.InnerText = true.ToString().ToLower();

                eleJourney.AppendChild(eleOutward);
                                
                foreach (JourneyLeg leg in journeyOutward.JourneyLegs)
                {
                    legCount++;

                    firstLeg = (legCount == 0);
                    lastLeg = (legCount == journeyOutward.JourneyLegs.Count - 1);

                    // Pass in the destination naptan to use if the last journey leg end location has none
                    eleJourney.AppendChild(AddJourneyLeg(doc, leg, legCount, firstLeg, lastLeg,
                        outwardOriginNaptan, outwardDestinationNaptan));
                }

                eleJourneys.AppendChild(eleJourney);
            }

            legCount = -1;

            // Return journey
            if (journeyReturn != null)
            {
                eleJourney = doc.CreateElement(ELE_Journey);

                eleOutward = doc.CreateElement(ELE_Outward);
                eleOutward.InnerText = false.ToString().ToLower();

                eleJourney.AppendChild(eleOutward);

                foreach (JourneyLeg leg in journeyReturn.JourneyLegs)
                {
                    legCount++;

                    firstLeg = (legCount == 0);
                    lastLeg = (legCount == journeyReturn.JourneyLegs.Count - 1);

                    // Pass in the origin naptan to use if the first journey leg start location has none
                    eleJourney.AppendChild(AddJourneyLeg(doc, leg, legCount, firstLeg, lastLeg,
                        returnOriginNaptan, returnDestinationNaptan));
                }

                eleJourneys.AppendChild(eleJourney);
            }

            return eleJourneys;
        }

        /// <summary>
        /// Creates a joruney leg node element and populates the attributes for a LegBoard or LegAlight
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="nodeName"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        private static XmlElement AddJourneyLeg(XmlDocument doc, JourneyLeg leg, int legCount,
            bool firstLeg, bool lastLeg, string startLocationNaptan, string endLocationNaptan)
        {
            XmlElement journeyLeg = null;
            XmlElement legId = null;
            XmlElement legBoard = null;
            XmlElement legAlight = null;
            XmlElement legBoardTime = null;
            XmlElement legAlightTime = null;
            XmlElement operatorCode = null;
            XmlElement operatorName = null;
            XmlElement serviceNumber = null;
            XmlElement legMode = null;
            XmlElement legInGamesTravelcardZone = null;

            journeyLeg = doc.CreateElement(ELE_JourneyLeg);

            // Leg id
            legId = doc.CreateElement(ELE_LegId);
            legId.InnerText = legCount.ToString();

            // Leg board and alight
            if ((firstLeg) || (lastLeg))
            {
                legBoard = AddLegNode(doc, ELE_LegBoard, leg.LegStart.Location, startLocationNaptan);
                legAlight = AddLegNode(doc, ELE_LegAlight, leg.LegEnd.Location, endLocationNaptan);
            }
            else
            {
                legBoard = AddLegNode(doc, ELE_LegBoard, leg.LegStart.Location, string.Empty);
                legAlight = AddLegNode(doc, ELE_LegAlight, leg.LegEnd.Location, string.Empty);
            }
            
            // Leg datetimes
            legBoardTime = doc.CreateElement(ELE_LegBoardTime);
            legBoardTime.InnerText = leg.StartTime.ToString(DATETIME_FORMAT);

            legAlightTime = doc.CreateElement(ELE_LegAlightTime);
            legAlightTime.InnerText = leg.EndTime.ToString(DATETIME_FORMAT);

            // Service details
            operatorCode = doc.CreateElement(ELE_OperatorCode);
            operatorName = doc.CreateElement(ELE_OperatorName);
            serviceNumber = doc.CreateElement(ELE_ServiceNumber);

            if (leg.JourneyDetails.Count > 0)
            {
                if (leg.JourneyDetails[0] is PublicJourneyDetail)
                {
                    PublicJourneyDetail pjd = (PublicJourneyDetail)leg.JourneyDetails[0];

                    if (pjd.Services.Count > 0)
                    {
                        operatorCode.InnerText = pjd.Services[0].OperatorCode;
                        operatorName.InnerText = pjd.Services[0].OperatorName;
                        serviceNumber.InnerText = pjd.Services[0].ServiceNumber;
                    }
                }
            }

            // Leg mode
            legMode = doc.CreateElement(ELE_LegMode);
            legMode.InnerText = leg.Mode.ToString().LowercaseFirst();
            
            // Leg in games travelcard zone
            ITravelcardCatalogue travelcardCatalogue = SJPServiceDiscovery.Current.Get<ITravelcardCatalogue>(ServiceDiscoveryKey.TravelcardCatalogue);
            bool showTravelcardSwitch = Properties.Current["Retail.Travelcard.ProcessJourneyLeg.Switch"].Parse(false);
            bool includeTravelcardSwitch = Properties.Current["Retail.Travelcard.RetailHandoffXml.IncludeTravelcard.Switch"].Parse(false);
            bool hasTravelcard = (showTravelcardSwitch && includeTravelcardSwitch && travelcardCatalogue.HasTravelcard(leg));

            legInGamesTravelcardZone = doc.CreateElement(ELE_LegInGamesTravelcardZone);
            legInGamesTravelcardZone.InnerText = hasTravelcard.ToString().ToLower();


            journeyLeg.AppendChild(legId);
            journeyLeg.AppendChild(legBoard);
            journeyLeg.AppendChild(legAlight);
            journeyLeg.AppendChild(legBoardTime);
            journeyLeg.AppendChild(legAlightTime);
            journeyLeg.AppendChild(operatorCode);
            journeyLeg.AppendChild(operatorName);
            journeyLeg.AppendChild(serviceNumber);
            journeyLeg.AppendChild(legMode);

            if (includeTravelcardSwitch)
                journeyLeg.AppendChild(legInGamesTravelcardZone);

            return journeyLeg;
        }

        /// <summary>
        /// Creates a leg node element and populates the attributes for a LegBoard or LegAlight
        /// </summary>
        private static XmlElement AddLegNode(XmlDocument doc, string nodeName, SJPLocation location, string locationNaptan)
        {
            XmlElement eleLegNode = doc.CreateElement(nodeName);

            XmlElement eleSiteName = doc.CreateElement(ELE_SiteName);
            eleSiteName.InnerText = location.Name;
            
            // If the location naptan is invalid, and an alternative naptan was provided use that
            string naptan = ParseNaPTAN(location.Naptan);

            if (((string.IsNullOrEmpty(naptan)) || (naptan.Equals(DUMMY_NAPTAN)))
                && (!string.IsNullOrEmpty(locationNaptan)))
            {
                naptan = locationNaptan;
            }

            XmlElement eleSiteAtcoCode = doc.CreateElement(ELE_SiteAtcoCode);
            eleSiteAtcoCode.InnerText = naptan;

            eleLegNode.AppendChild(eleSiteName);
            eleLegNode.AppendChild(eleSiteAtcoCode);

            return eleLegNode;
        }

        /// <summary>
        /// Converts all non standard naptans to a common value
        /// </summary>
        /// <param name="naptan"></param>
        /// <returns></returns>
        private static string ParseNaPTAN(List<string> naptans)
        {
            if ((naptans == null) || (naptans.Count == 0))
            {
                return DUMMY_NAPTAN;
            }

            // Use the first naptan in the list
            string naptan = naptans[0];

            if (string.IsNullOrEmpty(naptan) || naptan.Equals(ORIGIN_NAPTAN) || naptan.Equals(DESTINATION_NAPTAN))
            {
                return DUMMY_NAPTAN;
            }
            else
            {
                return naptan;
            }
        }

        #endregion

        #endregion
    }
}
