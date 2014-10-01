// *********************************************** 
// NAME             : StopEventRunner.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 26 Apr 2011
// DESCRIPTION  	: Validates user input and initiate stop event request
// ************************************************
// 

using System.Diagnostics;
using SJP.Common;
using SJP.Common.EventLogging;
using SJP.UserPortal.JourneyControl;

namespace SJP.UserPortal.JourneyPlanRunner
{
    /// <summary>
    /// Validates user input and initiate stop event request
    /// </summary>
    public class StopEventRunner: JourneyPlanRunnerBase
    {
        #region Constructor
        
        /// <summary>
        /// Constructor
        /// </summary>
        public StopEventRunner()
            : base()
        {
        }

        #endregion
        
        #region Public Methods

        /// <summary>
        /// ValidateAndRun
        /// </summary>
        /// <param name="journeyRequest">ISJPJourneyRequest</param>
        public override bool ValidateAndRun(ISJPJourneyRequest journeyRequest, string language, bool submitRequest)
        {
            if (journeyRequest == null)
            {
                Trace.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, "No journey request provided"));
                throw new SJPException("JourneyRequest object is null", true, SJPExceptionIdentifier.JPRInvalidSJPJourneyRequest);
            }

            #region Validations

            //
            // Perform date validations
            //
            PerformDateValidations(journeyRequest);

            #endregion

            if (listErrors.Count == 0)
            {
                if (submitRequest)
                {
                    // All input journey parameters were correctly formed so invoke the Stop Event Manager
                    InvokeStopEventManager(journeyRequest, language);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        
        #endregion
    }
}
