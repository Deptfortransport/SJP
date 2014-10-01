// *********************************************** 
// NAME             : CJPCustomEventPublisher.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Jun 2011
// DESCRIPTION  	: CJPCustomEventPublisher class to publish TDP CJPCustomEvents to the database
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using SJP.Common;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common.PropertyManager;
using SJP.Reporting.Events;
using Logger = System.Diagnostics.Trace;
using SJPEL = SJP.Common.EventLogging;

namespace SJP.Reporting.EventPublishers
{
    /// <summary>
    /// CJPCustomEventPublisher class to publish TDP CJPCustomEvents to the database
    /// </summary>
    public class CJPCustomEventPublisher : SJPEL.IEventPublisher
    {
        #region Private members

        private string identifier;
        private SqlHelperDatabase targetDatabase;

        #endregion

        #region Constructor

        /// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="identifier">Identifier for publisher. Must match identifier defined in logging service property key.</param>
		/// <param name="targetDatabase">Identifier of target database to which events are published.</param>
        public CJPCustomEventPublisher(string identifier, SqlHelperDatabase targetDatabase)
		{
			this.identifier = identifier;
			this.targetDatabase = targetDatabase;

            DatabasePublisherPropertyValidator validator = new DatabasePublisherPropertyValidator(Properties.Current);
            List<string> errors = new List<string>();
			validator.ValidateProperty(targetDatabase.ToString(), errors);
            validator.ValidateProperty(String.Format(SJP.Common.EventLogging.Keys.CustomPublisherName, identifier), errors);

			if (errors.Count > 0)
			{
				StringBuilder message = new StringBuilder(100);

				foreach( string error in errors )
				{
					message.Append(error);
					message.Append(" ");	
				}

				throw new SJPException(String.Format(Messages.ConstructorFailed, message.ToString()), false,
                    SJPExceptionIdentifier.RDPCJPCustomEventPublisherConstruction);
			}
		}

        #endregion

        #region Publict properties

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public string Identifier
        {
            get { return identifier; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Publisher for Events that inherit from TDCustomEvent
        /// </summary>
        /// <param name="logEvent">Event that inherits from TDPCustomEvent</param>
        /// <exception cref="SJPException">Thrown when the call to WriteToDB method fails</exception>
        /// <exception cref="SJPException">Thrown if the Event passed in is not a TDPCustomEvent</exception>
        public void WriteEvent(SJPEL.LogEvent logEvent)
        {
            List<SqlParameter> parameters = null;

            if(logEvent is JourneyWebRequestEvent)
			{
                #region JourneyWebRequestEvent

				JourneyWebRequestEvent customEvent = (JourneyWebRequestEvent)logEvent;

                parameters = new List<SqlParameter>();
                				
				parameters.Add(new SqlParameter("@JourneyWebRequestId", customEvent.JourneyWebRequestId ));
				parameters.Add(new SqlParameter("@SessionId", customEvent.SessionId ));

				if(!SqlTypeHelper.IsSqlDateTimeCompatible( customEvent.Submitted))
				{
					Logger.Write(new SJPEL.OperationalEvent(SJPEL.SJPEventCategory.Database, SJPEL.SJPTraceLevel.Error, 
                        string.Format(Messages.SqlDateTimeOverflow, "AddJourneyWebRequestEvent", customEvent.FileFormatter.AsString(customEvent))));
					return;
				}
				parameters.Add(new SqlParameter("@Submitted", customEvent.Submitted ));
				parameters.Add(new SqlParameter("@RequestType", customEvent.RequestType.ToString() ));
				parameters.Add(new SqlParameter("@RegionCode", customEvent.RegionCode ));
				parameters.Add(new SqlParameter("@Success", customEvent.Success ));
				parameters.Add(new SqlParameter("@RefTransaction", customEvent.RefTransaction ));
				
				if(!SqlTypeHelper.IsSqlDateTimeCompatible( customEvent.Time))
				{
					Logger.Write(new SJPEL.OperationalEvent(SJPEL.SJPEventCategory.Database, SJPEL.SJPTraceLevel.Error, 
                        string.Format(Messages.SqlDateTimeOverflow, "AddJourneyWebRequestEvent", customEvent.FileFormatter.AsString(customEvent))));

					return;

				}
				parameters.Add(new SqlParameter("@TimeLogged", customEvent.Time ));
				
				try
				{	
					WriteToDB("AddJourneyWebRequestEvent", parameters);
				}
				catch (SJPException sjpEx)
                {
                    throw new SJPException(String.Format(Messages.EventPublishFailure, "JourneyWebRequestEvent", sjpEx.Message), 
                        sjpEx, false, SJPExceptionIdentifier.RDPFailedPublishingCJPJourneyWebRequestEvent);
                }

                #endregion
			}
			else if(logEvent is LocationRequestEvent)
			{
                #region LocationRequestEvent

				LocationRequestEvent customEvent = (LocationRequestEvent)logEvent;
				
                parameters = new List<SqlParameter>();

				parameters.Add(new SqlParameter("@JourneyPlanRequestId", customEvent.JourneyPlanRequestId ));
				parameters.Add(new SqlParameter("@PrepositionCategory", customEvent.PrepositionCategory.ToString() ));
				parameters.Add(new SqlParameter("@AdminAreaCode", customEvent.AdminAreaCode ));
				parameters.Add(new SqlParameter("@RegionCode", customEvent.RegionCode ));
				if(!SqlTypeHelper.IsSqlDateTimeCompatible( customEvent.Time))
				{
					Logger.Write(new SJPEL.OperationalEvent(SJPEL.SJPEventCategory.Database, SJPEL.SJPTraceLevel.Error, 
                        string.Format(Messages.SqlDateTimeOverflow, "AddLocationRequestEvent", customEvent.FileFormatter.AsString(customEvent))));
					return;
				}

				parameters.Add(new SqlParameter("@TimeLogged", customEvent.Time ));
				
				try
				{
					WriteToDB("AddLocationRequestEvent", parameters);
				}
				catch (SJPException sjpEx)
                {
                    throw new SJPException(String.Format(Messages.EventPublishFailure, "LocationRequestEvent", sjpEx.Message), 
                        sjpEx, false, SJPExceptionIdentifier.RDPFailedPublishingCJPLocationRequestEvent);
                }

                #endregion
			}
			else if (logEvent is InternalRequestEvent)
			{
                #region InternalRequestEvent

				InternalRequestEvent customEvent = (InternalRequestEvent)logEvent;
				
                parameters = new List<SqlParameter>();
                
				// Add all the required parameters for the Stored Procedure to the hashtable
				parameters.Add(new SqlParameter("@InternalRequestId", customEvent.InternalRequestId ));
				parameters.Add(new SqlParameter("@SessionId", customEvent.SessionId ));
				if(!SqlTypeHelper.IsSqlDateTimeCompatible( customEvent.Submitted))
				{
                    Logger.Write(new SJPEL.OperationalEvent(SJPEL.SJPEventCategory.Database, SJPEL.SJPTraceLevel.Error, 
                        string.Format(Messages.SqlDateTimeOverflow, "AddInternalRequestEvent", customEvent.FileFormatter.AsString(customEvent))));
					return;
				}
				parameters.Add(new SqlParameter("@Submitted", customEvent.Submitted ));
				parameters.Add(new SqlParameter("@InternalRequestType", customEvent.RequestType.ToString() ));
				parameters.Add(new SqlParameter("@FunctionType", customEvent.FunctionType ));
				parameters.Add(new SqlParameter("@Success", customEvent.Success ));
				parameters.Add(new SqlParameter("@RefTransaction", customEvent.RefTransaction ));
				
				if(!SqlTypeHelper.IsSqlDateTimeCompatible( customEvent.Time))
				{
                    Logger.Write(new SJPEL.OperationalEvent(SJPEL.SJPEventCategory.Database, SJPEL.SJPTraceLevel.Error, 
                        string.Format(Messages.SqlDateTimeOverflow, "AddInternalRequestEvent", customEvent.FileFormatter.AsString(customEvent))));
					return;
				}
				parameters.Add(new SqlParameter("@TimeLogged", customEvent.Time ));
								
				try
				{	
					WriteToDB("AddInternalRequestEvent", parameters);
				}
				catch (SJPException sjpEx)
                {
                    throw new SJPException(String.Format(Messages.EventPublishFailure, "InternalRequestEvent", sjpEx.Message),
                        sjpEx, false, SJPExceptionIdentifier.RDPFailedPublishingCJPInternalRequestEvent);
                }

                #endregion
			}
            else
            {
                Logger.Write(new SJPEL.OperationalEvent(SJPEL.SJPEventCategory.Database, SJPEL.SJPTraceLevel.Error, String.Format(Messages.UnsupportedEventType, logEvent.ClassName, "CJPCustomEventPublisher")));
                throw new SJPException(String.Format(Messages.UnsupportedEventType, logEvent.ClassName, "CJPCustomEventPublisher"),
                    true, SJPExceptionIdentifier.RDPUnsupportedCJPCustomEventPublisherEvent);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Executes the requested Stored Procedure with the parameters supplied.
        /// </summary>
        /// <param name="storedProcName">string containing the Stored Procedure Name</param>
        /// <param name="parameters">Hashtable containg the Stored Proc parameters</param>
        /// <exception cref="SJPException">Thrown when the stored Procedure return code is incorrect</exception>
        /// <exception cref="SJPException">Thrown when a SqlException is caught</exception>
        private void WriteToDB(string storedProcName, List<SqlParameter> sqlParameters)
        {
            try
            {
                using (SqlHelper sqlHelper = new SqlHelper())
                {
                    sqlHelper.ConnOpen(this.targetDatabase);

                    int returnCode = sqlHelper.Execute(storedProcName, sqlParameters);

                    if (returnCode != 1)
                    {
                        throw new SJPException(String.Format(Messages.SQLHelperInsertFailed, 1, storedProcName, returnCode),
                                              false,
                                              SJPExceptionIdentifier.RDPSQLHelperInsertFailed);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // SQLHelper does not catch SqlException so catch here.
                throw new SJPException(String.Format(Messages.SQLHelperError, storedProcName, sqlEx.Message), sqlEx, false, 
                    SJPExceptionIdentifier.RDPSQLHelperStoredProcedureFailure);
            }
            catch (SqlTypeException ste)
            {
                throw new SJPException(String.Format(Messages.SQLHelperTypeError, storedProcName, ste.Message), ste, false, 
                    SJPExceptionIdentifier.RDPSQLHelperStoredProcedureFailure);
            }
        }

        #endregion
    }
}
