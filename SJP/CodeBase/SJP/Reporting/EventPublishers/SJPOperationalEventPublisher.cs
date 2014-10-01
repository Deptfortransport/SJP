// *********************************************** 
// NAME             : OperationalEventPublisher.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Apr 2011
// DESCRIPTION  	: Publisher that publishes SJP Operational Events to the report staging database
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common.EventLogging;
using SJP.Common.PropertyManager;
using SJP.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Logger = System.Diagnostics.Trace;
using SJP.Reporting.Events;

namespace SJP.Reporting.EventPublishers
{
    /// <summary>
    /// Publisher that publishes SJP Operational Events to the report staging database
    /// </summary>
    public class SJPOperationalEventPublisher: IEventPublisher
    {
        #region Private members

        private string identifier;
        private SqlHelperDatabase targetDatabase;

        private Dictionary<string, int> fieldLengths = null;

        #endregion

        #region Constructor

        /// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="identifier">Identifier for publisher. Must match identifier defined in logging service property key.</param>
		/// <param name="targetDatabase">Identifier of target database to which events are published.</param>
        public SJPOperationalEventPublisher(string identifier, SqlHelperDatabase targetDatabase)
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
                    SJPExceptionIdentifier.RDPSJPOperationalEventPublisherConstruction);
			}

            LoadFieldLengths(false);
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
        /// <exception cref="TDException">Thrown when the call to WriteToDB method fails</exception>
        /// <exception cref="TDException">Thrown if the Event passed in is not a TDPCustomEvent</exception>
        public void WriteEvent(LogEvent logEvent)
        {
            List<SqlParameter> parameters = null;

            if ((logEvent is OperationalEvent) || (logEvent is ReceivedOperationalEvent))
            {
                #region OperationalEvent, ReceivedOperationalEvent

                OperationalEvent operationalEvent = null;

                if (logEvent is ReceivedOperationalEvent)
                {
                    ReceivedOperationalEvent receivedOperationalEvent = (ReceivedOperationalEvent)logEvent;
                    operationalEvent = receivedOperationalEvent.WrappedOperationalEvent;
                }
                else
                {
                    operationalEvent = (OperationalEvent)logEvent;
                }

                List<string> truncatedFields = new List<string>(9);

                string sessionId = TruncateField("SessionId", operationalEvent.SessionId, truncatedFields);
                string message = TruncateField("Message", operationalEvent.Message, truncatedFields);
                string machineName = TruncateField("MachineName", operationalEvent.MachineName, truncatedFields);
                string assemblyName = TruncateField("AssemblyName", operationalEvent.AssemblyName, truncatedFields);
                string methodName = TruncateField("MethodName", operationalEvent.MethodName, truncatedFields);
                string typeName = TruncateField("TypeName", operationalEvent.TypeName, truncatedFields);
                string level = TruncateField("Level", operationalEvent.Level.ToString(), truncatedFields);
                string category = TruncateField("Category", operationalEvent.Category.ToString(), truncatedFields);
                string target = string.Empty;
                DateTime timeLogged = operationalEvent.Time;

                if (operationalEvent.Target != null)
                {
                    target = TruncateField("Target", operationalEvent.Target.ToString(), truncatedFields);
                }

                if (!SqlTypeHelper.IsSqlDateTimeCompatible(operationalEvent.Time))
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error,
                        string.Format(Messages.SqlDateTimeOverflow, "AddOperationalEvent",
                        operationalEvent.FileFormatter.AsString(operationalEvent))));
                    return;
                }

                if (truncatedFields.Count > 0)
                {
                    // At least one field has been truncated. Write a warning operational event
                    string[] fields = truncatedFields.ToArray();
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Warning, 
                        string.Format(Messages.TruncatedField, string.Join(", ", fields), operationalEvent.FileFormatter.AsString(operationalEvent))));
                }

                
                // Build SqlParameters
                parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter("@SessionId", sessionId));
                parameters.Add(new SqlParameter("@Message", message));
                parameters.Add(new SqlParameter("@MachineName", machineName));
                parameters.Add(new SqlParameter("@AssemblyName", assemblyName));
                parameters.Add(new SqlParameter("@MethodName", methodName));
                parameters.Add(new SqlParameter("@TypeName", typeName));
                parameters.Add(new SqlParameter("@Level", level));
                parameters.Add(new SqlParameter("@Category", category));
                parameters.Add(new SqlParameter("@Target", target));
                parameters.Add(new SqlParameter("@TimeLogged", timeLogged));
                                
                try
                {
                    WriteToDB("AddOperationalEvent", parameters);
                }
                catch (SJPException sjpEx)
                {
                    throw new SJPException(String.Format(Messages.EventPublishFailure, logEvent.ClassName, sjpEx.Message),
                        sjpEx, false, SJPExceptionIdentifier.RDPFailedPublishingOperationalEvent);
                }
                #endregion
            }
            else
            {
                Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, String.Format(Messages.UnsupportedEventType, logEvent.ClassName, "SJPOperationalEventPublisher")));
                throw new SJPException(String.Format(Messages.UnsupportedEventType, logEvent.ClassName, "OperationalEventPublisher"),
                    true, SJPExceptionIdentifier.RDPUnsupportedSJPOperationalEventPublisherEvent);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Executes the requested Stored Procedure with the parameters supplied.
        /// </summary>
        /// <param name="storedProcName">string containing the Stored Procedure Name</param>
        /// <param name="parameters">Hashtable containg the Stored Proc parameters</param>
        /// <exception cref="TDException">Thrown when the stored Procedure return code is incorrect</exception>
        /// <exception cref="TDException">Thrown when a SqlException is caught</exception>
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

        /// <summary>
        /// Attempts to retrieve the lengths of the text fields in the operational events
        /// table in the staging database. This information can then be used to trim the data.
        /// </summary>
        private bool LoadFieldLengths(bool forceReload)
        {
            if ((fieldLengths != null) && !forceReload)
                return true;

            lock (this)
            {
                SqlDataReader reader = null;

                try
                {
                    using (SqlHelper sqlHelper = new SqlHelper())
                    {
                        sqlHelper.ConnOpen(this.targetDatabase);

                        List<SqlParameter> parameters = new List<SqlParameter>();
                        parameters.Add(new SqlParameter("@TableName", "OperationalEvent"));

                        reader = sqlHelper.GetReader("GetTableColumnLengths", parameters);

                        fieldLengths = new Dictionary<string, int>();

                        while (reader.Read())
                            fieldLengths.Add(reader.GetString(0).ToLower(), reader.GetInt32(1));
                    }
                }
                catch (Exception ex)
                {
                    // Write this as a warning then continue
                    Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Warning, 
                        string.Format(Messages.FailedRetrievingFieldLengths, ex.Message), ex, string.Empty));
                    
                    fieldLengths = null;
                    
                    return false;
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// If the contents of fieldValue are greater than the maximum length for the field,
        /// as retrieved using GetFieldLength, a truncated copy of fieldValue will be returned,
        /// and fieldName will be added to fieldList. Otherwise fieldValue will be returned 
        /// unaltered.
        /// </summary>
        /// <param name="fieldName">Name of the field being processed</param>
        /// <param name="fieldValue">Value of the field being processed</param>
        /// <param name="fieldList">Array list to which the name of the field should be added if it is truncated</param>
        /// <returns></returns>
        private string TruncateField(string fieldName, string fieldValue, List<string> fieldList)
        {
            int fieldLength = GetFieldLength(fieldName);
            if ((fieldLength != 0) && (fieldLength < fieldValue.Length))
            {
                fieldList.Add(fieldName);
                return fieldValue.Substring(0, fieldLength - 3) + "...";
            }
            else
                return fieldValue;
        }

        /// <summary>
        /// If there is a maximum length for the field specified in the hashtable, then
        /// this value is returned, otherwise 0 is returned
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private int GetFieldLength(string fieldName)
        {
            string lowerFieldName = fieldName.ToLower();
            if (fieldLengths == null)
                return 0;
            if (fieldLengths.ContainsKey(lowerFieldName))
                return (int)fieldLengths[lowerFieldName];
            else
                return 0;
        }

        #endregion
    }
}
