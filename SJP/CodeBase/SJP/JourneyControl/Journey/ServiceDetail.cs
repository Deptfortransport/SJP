// *********************************************** 
// NAME             : ServiceDetail.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 25 Mar 2011
// DESCRIPTION  	: ServiceDetail class to hold information about a service
// ************************************************
// 
                
using System;
using System.Text;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// ServiceDetail class to hold information about a service
    /// </summary>
    [Serializable()]
    public class ServiceDetail
    {
        #region Private members

        private string operatorCode;
        private string operatorName;
        private string serviceNumber;
        private string destinationBoard;
        private string direction;
        private string privateId;
        private string retailId;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ServiceDetail()
        { 
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ServiceDetail(string operatorCode,
                                string operatorName,
                                string serviceNumber,
                                string destinationBoard,
                                string direction,
                                string privateId,
                                string retailId)
        {
            this.operatorCode = (!string.IsNullOrEmpty(operatorCode)) ? operatorCode : string.Empty;
            this.operatorName = (!string.IsNullOrEmpty(operatorName)) ? operatorName : string.Empty;
            this.serviceNumber = (!string.IsNullOrEmpty(serviceNumber)) ? serviceNumber : string.Empty;
            this.destinationBoard = (!string.IsNullOrEmpty(destinationBoard)) ? destinationBoard : string.Empty;
            this.direction = (!string.IsNullOrEmpty(direction)) ? direction : string.Empty;
            this.privateId = (!string.IsNullOrEmpty(privateId)) ? privateId : string.Empty;
            this.retailId = (!string.IsNullOrEmpty(retailId)) ? retailId : string.Empty;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read/Write. OperatorCode
        /// </summary>
        public string OperatorCode
        {
            get { return operatorCode; }
            set { operatorCode = value; }
        }

        /// <summary>
        /// Read/Write. OperatorName
        /// </summary>
        public string OperatorName
        {
            get { return operatorName; }
            set { operatorName = value; }
        }

        /// <summary>
        /// Read/Write. ServiceNumber
        /// </summary>
        public string ServiceNumber
        {
            get { return serviceNumber; }
            set { serviceNumber = value; }
        }

        /// <summary>
        /// Read/Write. DestinationBoard
        /// </summary>
        public string DestinationBoard
        {
            get { return destinationBoard; }
            set { destinationBoard = value; }
        }

        /// <summary>
        /// Read/Write. Direction
        /// </summary>
        public string Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        /// <summary>
        /// Read/Write. PrivateId
        /// </summary>
        public string PrivateId
        {
            get { return privateId; }
            set { privateId = value; }
        }

        /// <summary>
        /// Read/Write. RetailId
        /// </summary>
        public string RetailId
        {
            get { return retailId; }
            set { retailId = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a string of this ServiceDetail
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("opCode[{0}] ", operatorCode));
            sb.AppendLine(string.Format("opName[{0}] ", operatorName));
            sb.AppendLine(string.Format("serNum[{0}] ", serviceNumber));
            sb.AppendLine(string.Format("direction[{0}] ", direction));
            sb.AppendLine(string.Format("privId[{0}] ", privateId));
            sb.AppendLine(string.Format("retId[{0}]", retailId));

            return sb.ToString();            
        }

        #endregion
    }
}
