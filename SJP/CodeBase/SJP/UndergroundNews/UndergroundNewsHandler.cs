// *********************************************** 
// NAME             : UndergroundNewsHandler.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 05 Mar 2012
// DESCRIPTION  	: UndergroundNewsHandler class that makes underground news available to clients
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.DatabaseInfrastructure;
using Logger = System.Diagnostics.Trace;
using SJP.Common.EventLogging;
using System.Data.SqlClient;

namespace SJP.UserPortal.UndergroundNews
{
    /// <summary>
    /// UndergroundNewsHandler class that makes underground news available to clients
    /// </summary>
    public class UndergroundNewsHandler : IUndergroundNewsHandler
    {
        #region Private members

        private bool undergroundNewsAvailable = false;

        private DateTime undergroundStatusLastUpdated = DateTime.MinValue;
        private List<UndergroundStatusItem> undergroundStatusList = new List<UndergroundStatusItem>();
        
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public UndergroundNewsHandler()
        {
            LoadUndergroundStatusData();
        }

        #endregion

        #region IUndergroundNewsHandler methods

        /// <summary>
        /// Returns if underground news is available
        /// </summary>
        public bool IsUndergroundNewsAvaliable
        {
            get { return undergroundNewsAvailable; }
        }

        /// <summary>
        /// Returns the underground status last updated date time 
        /// </summary>
        public DateTime UndergroundStatusLastUpdated 
        {
            get { return undergroundStatusLastUpdated; }
        }

        /// <summary>
        /// Returns underground status data
        /// </summary>
        public List<UndergroundStatusItem> GetUndergroundStatusItems()
        {
            return undergroundStatusList;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Loads UndergroundStatus data from the database
        /// </summary>
        private void LoadUndergroundStatusData()
        {
            #region Load from database

            using (SqlHelper sqlHelper = new SqlHelper())
            {
                List<UndergroundStatusItem> tmpUndergroundStatusList = new List<UndergroundStatusItem>();

                UndergroundStatusItem usi = null;

                try
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose,
                        "UndergroundNews - Loading underground status data"));
                    
                    List<SqlParameter> paramList = new List<SqlParameter>();

                    sqlHelper.ConnOpen(SqlHelperDatabase.TransientPortalDB);

                    // Read and populate the underground status
                    using (SqlDataReader listDR = sqlHelper.GetReader("GetUndergroundStatus", paramList))
                    {
                        while (listDR.Read())
                        {
                            // Create underground status item
                            usi = new UndergroundStatusItem();

                            usi.LineStatusID = listDR["LineStatusId"].ToString();
                            usi.LineStatusDetails = listDR["LineStatusDetails"].ToString();
                            usi.LineId = listDR["LineId"].ToString();
                            usi.LineName = listDR["LineName"].ToString();
                            usi.StatusId = listDR["StatusId"].ToString();
                            usi.StatusDescription = listDR["StatusDescription"].ToString();
                            usi.StatusIsActive = Convert.ToBoolean(listDR["StatusIsActive"].ToString());
                            usi.StatusCssClass = listDR["StatusCssClass"].ToString();
                            
                            // Add to temp underground status list
                            tmpUndergroundStatusList.Add(usi);
                        }
                    }
                    
                    // Assign to class list
                    undergroundStatusList = tmpUndergroundStatusList;

                    // Update the last updated date time
                    undergroundStatusLastUpdated = DateTime.Now;

                    // Let callers know that data was loaded successfully
                    if (undergroundStatusList.Count > 0)
                    {
                        undergroundNewsAvailable = true;
                    }

                }
                catch (Exception ex)
                {
                    string message = string.Format("Error occurred attempting to load Underground status data: {0}", ex.Message);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, ex));

                    undergroundNewsAvailable = false;
                }
            }

            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose,
                string.Format("UndergroundNews - Load completed with UndergroundStatusItems[{0}].", undergroundStatusList.Count)));

            #endregion
        }

        #endregion
    }
}
