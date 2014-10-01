// *********************************************** 
// NAME             : SJPException.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Defines an SJP application exception
// ************************************************
// 
            
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace SJP.Common
{
    /// <summary>
    /// SJP application exception
    /// </summary>
    [Serializable]
    public class SJPException : ApplicationException
    {
        #region Private Fields
        private SJPExceptionIdentifier id;
		private bool logged;
		private object additionalInformation;
        #endregion

        #region Constructors
        /// <summary>
		/// Constructor used for deserialization
		/// </summary>
		public SJPException(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt)
		{
			id = (SJPExceptionIdentifier)info.GetValue("SJPExId", typeof(SJPExceptionIdentifier));
			logged = (bool)info.GetValue("SJPExLogged", typeof(bool));
			additionalInformation = (object)info.GetValue("SJPExAdditionalInformation", typeof(object));
		}

        /// <summary>
        /// Creates a new instance of SJPException, setting the identifier to -1.
        /// </summary>
        public SJPException()
            : base(String.Empty, null)
        {
            this.id = SJPExceptionIdentifier.Undefined;
            this.logged = false; 
        }

        /// <summary>
        /// Creates a new instance of SJPException with a specified message, identifier 
        /// and an indication of whether the exeption was logged.
        /// </summary>
        /// <param name="message">A message describing the exception, or an empty string.</param>
        /// <param name="logged">True if the exeception has been logged using the SJP Event Logging Service, otherwise false.</param>
        /// <param name="id">An identifier used to identify the exception.</param>
        public SJPException(string message, bool logged, SJPExceptionIdentifier id)
            : this(message, null, logged, id)
        {
        }

        /// <summary>
        /// Creates a new instance of SJPException with a specified message, identifier,
        /// inner exception and an indication of whether the exeption was logged.
        /// </summary>
        /// <param name="message">A message describing the exception, or an empty string.</param>
        /// <param name="innerException">A previously caught exception to store with exception.</param>
        /// <param name="logged">True if the exeception has been logged using the SJP Event Logging Service, otherwise false.</param>
        /// <param name="id">An identifier used to identify the exception.</param>
        public SJPException(string message, Exception innerException, bool logged, SJPExceptionIdentifier id)
            : base(message, innerException)
        {
            this.id = id;
            this.logged = logged;
        }

        /// <summary>
        /// Creates a new instance of SJPException with a specified message, identifier,
        /// inner exception and an indication of whether the exeption was logged.
        /// </summary>
        /// <param name="messages">An array of messages instead attached to this error</param>		
        /// <param name="logged">True if the exeception has been logged using the SJP Event Logging Service, otherwise false.</param>
        /// <param name="id">An identifier used to identify the exception.</param>
        /// <param name="additionalInformation">Additional information linked to this message</param>
        public SJPException(string message, bool logged, SJPExceptionIdentifier id, object additionalInformation)
            : this(message, null, logged, id)
        {
            this.additionalInformation = additionalInformation;
        }		

        #endregion 

        #region Public Properties
        /// <summary>
        /// Gets the exception identifier.
        /// </summary>
        public SJPExceptionIdentifier Identifier
        {
            get { return id; }
        }

        /// <summary>
        /// Gets the logged indicator.
        /// Has the value true if the exception has been logged, otherwise false.
        /// </summary>
        public bool Logged
        {
            get { return logged; }
        }

        /// <summary>
        /// read-only property to return an array of messages linked to this error.
        /// </summary>
        public object AdditionalInformation
        {
            get { return additionalInformation; }
        }
        #endregion


        #region Public Methods
        /// <summary>
		/// Serialization method.
		/// </summary>
        [SecurityPermission(SecurityAction.LinkDemand,Flags=SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
		{
			base.GetObjectData(info, ctxt);

			info.AddValue("SJPExId", id);
			info.AddValue("SJPExLogged", logged);
			info.AddValue("SJPExAdditionalInformation", additionalInformation);
		}

		/// <summary>
		/// Formats SJPException as a string.
		/// </summary>
		/// <returns>Formatted SJPException as a string.</returns>
		public override string ToString()
		{
			return (String.Format("{0} {1}Logged:{2},Id:{3}{4}",
					"SJP.Common.SJPException:",
					this.Message==String.Empty?String.Empty:String.Format("Message:{0},", this.Message),
					this.logged.ToString(),
					this.id.ToString("D"),
					this.InnerException==null?String.Empty:String.Format(",InnerException:{0}", this.InnerException.ToString())));
        }

        #endregion
    }
}
