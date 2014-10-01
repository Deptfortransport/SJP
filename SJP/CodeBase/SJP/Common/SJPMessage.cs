// *********************************************** 
// NAME             : SJPMessage.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 25 Mar 2011
// DESCRIPTION  	: SJPMessage class to hold messages returned by the journey planner engine
// ************************************************
// 

using System;
using System.Collections.Generic;

namespace SJP.Common
{
    /// <summary>
    /// SJPMessage class to hold messages returned by the journey planner engine
    /// </summary>
    [Serializable()]
    public class SJPMessage
    {
        #region Private members

        private string messageText;
        private string messageResourceId;
        private string messageResourceCollection;
        private string messageResourceGroup;
        private List<string> messageArgs;
        private int minorMessageNumber;
        private int majorMessageNumber;
        private SJPMessageType type;

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SJPMessage()
        {
            messageText = string.Empty;
            messageResourceId = string.Empty;
            messageResourceCollection = string.Empty;
            messageResourceGroup = string.Empty;
            messageArgs = new List<string>();
            minorMessageNumber = 0;
            majorMessageNumber = 0;
            type = SJPMessageType.Info;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SJPMessage(string messageResourceId, SJPMessageType type):
            this(string.Empty,messageResourceId,0,0,type)
        {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public SJPMessage(string messageText, string messageResourceId, int majorNumber, int minorNumber, SJPMessageType type):
            this(messageText,messageResourceId,string.Empty,string.Empty,new List<string>(),majorNumber,minorNumber,type)
        {
           
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SJPMessage(string messageText, string messageResourceId, string messageResourceCollection, 
            string messageResourceGroup, List<string> messageArgs,
            int majorNumber, int minorNumber, SJPMessageType type)
        {
            this.messageText = messageText;
            this.messageResourceId = messageResourceId;
            this.messageResourceCollection = messageResourceCollection;
            this.messageResourceGroup = messageResourceGroup;
            this.messageArgs = messageArgs;
            this.majorMessageNumber = majorNumber;
            this.minorMessageNumber = minorNumber;
            this.type = type;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read/Write. Message text
        /// </summary>
        public string MessageText
        {
            get { return messageText; }
            set { messageText = value; }
        }

        /// <summary>
        /// Read/Write. Major message number
        /// </summary>
        public int MajorMessageNumber
        {
            get { return majorMessageNumber; }
            set { majorMessageNumber = value; }
        }

        /// <summary>
        /// Read/Write. Minor message number
        /// </summary>
        public int MinorMessageNumber
        {
            get { return minorMessageNumber; }
            set { minorMessageNumber = value; }
        }

        /// <summary>
        /// Read/Write. SJP Message type
        /// </summary>
        public SJPMessageType Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Read/Write. Message resource id
        /// </summary>
        public string MessageResourceId
        {
            get { return messageResourceId; }
            set { messageResourceId = value; }
        }

        /// <summary>
        /// Read/Write. Resource collection name for the message represented by messageResourceId
        /// </summary>
        public string MessageResourceCollection
        {
            get { return messageResourceCollection; }
            set { messageResourceCollection = value; }
        }

        /// <summary>
        /// Read/Write. Resource collection group name for the message represented by messageResourceId
        /// </summary>
        public string MessageResourceGroup 
        {
            get { return messageResourceGroup; }
            set { messageResourceGroup = value; } 
        }

        /// <summary>
        /// Read/Write. Message argument values to be substituted into message
        /// </summary>
        public List<string> MessageArgs
        {
            get { return messageArgs; }
            set { messageArgs = value; }
        }

        #endregion

        /// <summary>
        /// Creates a deep clone of this object
        /// </summary>
        /// <returns></returns>
        public SJPMessage Clone()
        {
            SJPMessage clone = new SJPMessage(
                this.messageText, this.messageResourceId,
                this.messageResourceCollection, this.messageResourceGroup,
                this.messageArgs, this.majorMessageNumber,
                this.minorMessageNumber, this.type);

            return clone;
        }
       
    }
}
