// *********************************************** 
// NAME             : JSGenerator.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Class providing methods to generate the location javascipts
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
    /// Class providing methods to generate the location javascipts
    /// </summary>
    class JSGenerator
    {
        #region Public Methods

        /// <summary>
        /// Class providing methods to generate the location javascipts
        /// </summary>
        public void CreateScripts(JSGeneratorMode mode)
        {
            // to track empty scripts
            string alphabet = "abcdefghijklmnopqrstuvwxyz";

            JSLocationProvider locationProvider = new JSLocationProvider();

            // Get the location data
            Dictionary<char, List<JSLocation>> locationsGroups = locationProvider.GetJsLocationData(mode);

            #region Scripts with data

            // Create scripts with location data 
            foreach (char c in locationsGroups.Keys)
            {
                CreateJs(c, locationsGroups[c], mode);
            }

            #endregion

            #region Empty Scripts

            // Create empty scripts
            foreach (char c in alphabet)
            {
                if (!locationsGroups.Keys.Contains(c))
                {
                    CreateJs(c, null, mode);
                }
            }

            #endregion
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates a location javascript for the perticular alphabetical group
        /// </summary>
        /// <param name="scriptId">Alphabetical group of the script i.e. a, b, etc.</param>
        /// <param name="locations">Location data loaded from database and aliases</param>
        private void CreateJs(char scriptId, List<JSLocation> locations, JSGeneratorMode mode)
        {
            try
            {
                StringBuilder js = new StringBuilder();
                bool first = true;

                if (locations != null)
                {
                    Trace.Write(
                       new OperationalEvent(
                           SJPEventCategory.Business,
                           SJPTraceLevel.Info,
                           string.Format("Generating script for {0}: [{1}] with locations count[{2}]", mode, scriptId, locations.Count)));


                    js.AppendFormat("var ddData_{0} = [", scriptId);

                    foreach (var loc in locations)
                    {
                        if (!first)
                        {
                            js.Append(",");
                        }
                        else
                            first = false;
                        js.Append(loc.GetJSArrayString());
                    }

                    js.AppendFormat("]; if(typeof registerLocationData == 'function'){{ registerLocationData(ddData_{0}); }}", scriptId);
                }
           
                // write physical script
                WriteScript(scriptId, js, mode);

            }
            catch (Exception ex)
            {
                string message = string.Format("Error generating script for {0}: [{1}] - {2}", mode, scriptId, ex.StackTrace);

                Trace.Write(
                  new OperationalEvent(
                      SJPEventCategory.Business,
                      SJPTraceLevel.Error,
                      message));

                throw new SJPException(message, true, SJPExceptionIdentifier.LJSGenCreateScriptFailed);
            }

        }

        /// <summary>
        /// Writes javascirpt
        /// </summary>
        /// <param name="scriptId">Alphabetical group of the script i.e. a, b, etc.</param>
        /// <param name="js">In memory script content </param>
        private void WriteScript(char scriptId, StringBuilder js, JSGeneratorMode mode)
        {
            string scriptLoc = GetScriptLocation(scriptId, mode);

            Trace.Write(
                   new OperationalEvent(
                       SJPEventCategory.Business,
                       SJPTraceLevel.Info,
                       string.Format("Writing script for : {0} at location : {1}", scriptId, scriptLoc)));

            using (StreamWriter sw = new StreamWriter(scriptLoc))
            {
               sw.Write(js.ToString());
            }
        }

        /// <summary>
        /// Returns the location where script needs generating
        /// </summary>
        /// <param name="scriptId">Alphabetical group of the script i.e. a, b, etc.</param>
        /// <returns>Path/Location for the scripts needs generating</returns>
        private string GetScriptLocation(char scriptId, JSGeneratorMode mode)
        {
            try
            {
                // Get directory to write to
                string scriptPath = JSGeneratorSettings.JSLocation(mode);
                
                if (!Directory.Exists(scriptPath))
                {
                    Directory.CreateDirectory(scriptPath);
                }

                string scriptName = string.Format("{0}_{1}_{2}_{3}.js",
                        JSGeneratorSettings.ScriptName,
                        mode.ToString(),
                        JSGeneratorSettings.Version,
                        scriptId);

                string scriptLoc = Path.Combine(scriptPath, scriptName);

                return scriptLoc;
            }
            catch (Exception ex)
            {
                string message = string.Format("Error Getting location to generate scripts : {0} ", ex.StackTrace);

                Trace.Write(
                  new OperationalEvent(
                      SJPEventCategory.Business,
                      SJPTraceLevel.Error,
                      message));

                throw new SJPException(message,true,SJPExceptionIdentifier.LJSGenGetScriptLocationFailed);
            }
        }

        #endregion


    }
}
