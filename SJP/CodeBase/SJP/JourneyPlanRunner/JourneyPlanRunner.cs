// *********************************************** 
// NAME             : JourneyPlanRunner.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 29 Mar 2011
// DESCRIPTION  	: Validates user input and initiate journey request
// ************************************************

using System.Diagnostics;
using SJP.Common;
using SJP.Common.EventLogging;
using SJP.UserPortal.JourneyControl;

namespace SJP.UserPortal.JourneyPlanRunner
{
    /// <summary>
    /// Validates user input and initiate journey request
    /// </summary>
    public class JourneyPlanRunner : JourneyPlanRunnerBase
    {
        #region Constructor
        
        /// <summary>
        /// Constructor
        /// </summary>
        public JourneyPlanRunner()
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
            // Perform location validations
            //
            PerformLocationValidations(journeyRequest);

            //
            // Perform date validations
            //
            PerformDateValidations(journeyRequest);

            #endregion

            if (listErrors.Count == 0)
            {
                if (submitRequest)
                {
                    // All input journey parameters were correctly formed so invoke the CJP Manager
                    InvokeCJPManager(journeyRequest, language);
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
