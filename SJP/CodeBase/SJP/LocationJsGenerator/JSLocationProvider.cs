// *********************************************** 
// NAME             : JSLocationProvider.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Provides method to access location data in database and process the alias data
//                    defined in csv file
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.LocationService;
using System.IO;
using System.Diagnostics;
using SJP.Common.EventLogging;

namespace SJP.Common.LocationJsGenerator
{
    /// <summary>
    /// Provides method to access location data in database and process the alias data
    /// defined in csv file
    /// </summary>
    class JSLocationProvider
    {
        #region Private Fields

        private Dictionary<char, List<JSLocation>> locationsGroups = new Dictionary<char, List<JSLocation>>();

        #endregion

        #region Public Methods

        /// <summary>
        /// Processes alias data and creates a list of location data grouped by first letter of location
        /// </summary>
        /// <returns>Dictionary of lists of locations where key is first letter of location</returns>
        public Dictionary<char, List<JSLocation>> GetJsLocationData(JSGeneratorMode mode)
        {
            LoadLocationData(mode);

            LoadAliasData(mode);

            return locationsGroups;
        }

        #endregion

        #region Private Methods

        #region Alias data

        /// <summary>
        /// Loads alias data from the alias file defined
        /// </summary>
        private void LoadAliasData(JSGeneratorMode mode)
        {
            try
            {
                if (!string.IsNullOrEmpty(JSGeneratorSettings.AliasFile))
                {
                    // skip the heading row
                    string[] allLines = File.ReadAllLines(JSGeneratorSettings.AliasFile).Skip(1).ToArray();

                    var query = from line in allLines.Where(line => string.IsNullOrEmpty(line) != true)

                                let data = line.Split(',')

                                select new

                                {

                                    DataType = data[0],
                                    DataId = data[1],
                                    Alias = data[2],


                                };


                    foreach (var aliasEntry in query)
                    {
                        AddAliasLocation(aliasEntry.DataType, aliasEntry.DataId, aliasEntry.Alias, mode);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("Error loading alias data : {0}" , ex.StackTrace);

                Trace.Write(
                  new OperationalEvent(
                      SJPEventCategory.Business,
                      SJPTraceLevel.Error,
                      message));

                throw new SJPException(message, true,SJPExceptionIdentifier.LJSGenAliasDataLoadFailed);
            }


        }

        /// <summary>
        /// Adds alias location to the private location data dictionary object
        /// </summary>
        /// <param name="dataType">Type of Location data i.e. R = Rail</param>
        /// <param name="alias">Alias for the location</param>
        private void AddAliasLocation(string dataType, string dataId, string alias, JSGeneratorMode mode)
        {
            try
            {
                
                AddAlias(dataType, dataId, alias, mode);
                   

            }
            catch (Exception ex)
            {
                string message = string.Format("Error adding alias location with datatype : {0}, dataId : {1} and alias : {2} - {3}",
                    dataType,
                    dataId,
                    alias,
                    ex.StackTrace);

                Trace.Write(
                  new OperationalEvent(
                      SJPEventCategory.Business,
                      SJPTraceLevel.Error,
                      message));

                throw new SJPException(message, true,SJPExceptionIdentifier.LJSGenAliasLocatonAddFailed);
            }
        }

        /// <summary>
        /// Adds alias identified by NaPTAN, locality or datasetid
        /// </summary>
        /// <param name="dataType">Type of data i.e. R= Rail </param>
        /// <param name="dataId">Data identifier i.e. NaPTAN, Locality</param>
        /// <param name="alias">Alias for the location</param>
        private void AddAlias(string dataType, string dataId, string alias, JSGeneratorMode mode)
        {
            using (SJPGazetteerDataContext db = new SJPGazetteerDataContext(JSGeneratorSettings.ConnectionString))
            {
                SJPNonPostcodeLocation sjploc = null;

                #region Load

                switch (mode)
                {
                    case JSGeneratorMode.SJPMobile: // SJPMobile does not show locality locations in the js dropdown
                        {
                            var dbloc = from loc in db.SJPNonPostcodeLocations
                                        where loc.Type != "VENUEPOI" && loc.Type != "LOCALITY"
                                              && (loc.Naptan == dataId || loc.DATASETID == dataId || loc.LocalityID == dataId)
                                        select loc;

                            sjploc = dbloc.FirstOrDefault();
                        }
                        break;
                    case JSGeneratorMode.SJPWeb:
                    default:
                        {
                            var dbloc = from loc in db.SJPNonPostcodeLocations
                                        where loc.Type != "VENUEPOI" 
                                              && (loc.Naptan == dataId || loc.DATASETID == dataId || loc.LocalityID == dataId)
                                        select loc;

                            sjploc = dbloc.FirstOrDefault();
                        }
                        break;
                }

                #endregion

               if (sjploc != null)
                {

                    JSLocation aliasLoc = new JSLocation(sjploc.DisplayName, GetLocationType(sjploc.Type), GetLocationId(sjploc), alias);

                    if (locationsGroups.ContainsKey(alias.ToLower()[0]))
                    {
                        locationsGroups[alias.ToLower()[0]].Add(aliasLoc);
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Loads location data from the database group by alphabets and stores in private dictionary 
        /// where key is the alphabet the location starting from
        /// </summary>
        private void LoadLocationData(JSGeneratorMode mode)
        {
            try
            {
                using (SJPGazetteerDataContext db = new SJPGazetteerDataContext(JSGeneratorSettings.ConnectionString))
                {
                    #region Load

                    switch (mode)
                    {
                        case JSGeneratorMode.SJPMobile: // SJPMobile does not show locality locations in the js dropdown
                            {

                                var lGroups = from loc in db.SJPNonPostcodeLocations
                                              where loc.Type != "VENUEPOI" && loc.Type != "LOCALITY"
                                              group new JSLocation(loc.DisplayName, GetLocationType(loc.Type), GetLocationId(loc)) by loc.DisplayName.ToLower()[0] into g
                                              orderby g.Key
                                              select new { FirstLetter = g.Key, locations = g };


                                locationsGroups = lGroups.ToDictionary(e => e.FirstLetter, e => e.locations.ToList());
                            }
                            break;
                        case JSGeneratorMode.SJPWeb:
                        default:
                            {

                                var lGroups = from loc in db.SJPNonPostcodeLocations
                                              where loc.Type != "VENUEPOI"
                                              group new JSLocation(loc.DisplayName, GetLocationType(loc.Type), GetLocationId(loc)) by loc.DisplayName.ToLower()[0] into g
                                              orderby g.Key
                                              select new { FirstLetter = g.Key, locations = g };


                                locationsGroups = lGroups.ToDictionary(e => e.FirstLetter, e => e.locations.ToList());
                            }
                            break;
                    }

                    #endregion

                    int locationCount = db.SJPNonPostcodeLocations.Count();

                    Trace.Write(
                          new OperationalEvent(
                              SJPEventCategory.Business,
                              SJPTraceLevel.Info,
                              string.Format("location data count : {0} grouped in parts : {1}", locationCount, locationsGroups.Keys.Count)));
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("Error loading location data from database : {0}", ex.StackTrace);

                Trace.Write(
                  new OperationalEvent(
                      SJPEventCategory.Business,
                      SJPTraceLevel.Error,
                      message));

                throw new SJPException(message, true,SJPExceptionIdentifier.LJSGenLocatonDataLoadFailed);
            }
           
        }

        /// <summary>
        /// Returns the identifier for the location
        /// </summary>
        /// <param name="loc">SJPNonPostcodeLocation object</param>
        /// <returns>Location identifier string</returns>
        private string GetLocationId(SJPNonPostcodeLocation loc)
        {
            string id = string.Empty;

            switch (loc.Type.ToUpper())
            {
                case ("COACH"):
                case ("RAIL STATION"):
                case ("AIRPORT"):
                case ("TMU"):
                case ("FERRY"):
                    id = loc.Naptan;
                    break;
                case ("EXCHANGE GROUP"):
                    id = loc.DATASETID;
                    break;
                case ("LOCALITY"):
                    id = loc.LocalityID;
                    break;

            }

            return id;
        }

        /// <summary>
        /// Method to determine the type of location
        /// </summary>
        /// <param name="locType">location type string in database</param>
        /// <returns>SJPLocationType enum value</returns>
        private SJPLocationType GetLocationType(string locType)
        {
            SJPLocationType type = SJPLocationType.Unknown;

            switch (locType.ToUpper())
            {
                case ("COACH"):
                case ("RAIL STATION"):
                case ("AIRPORT"):
                case ("TMU"):
                case ("FERRY"):
                    type = SJPLocationType.Station;
                    break;
                case ("EXCHANGE GROUP"):
                    type = SJPLocationType.StationGroup;
                    break;
                case ("LOCALITY"):
                    type = SJPLocationType.Locality;
                    break;

            }

            return type;
        }

        #endregion
    }
}
