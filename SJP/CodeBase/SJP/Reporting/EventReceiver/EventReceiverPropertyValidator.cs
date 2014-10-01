// *********************************************** 
// NAME             : EventReceiverPropertyValidator.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Apr 2011
// DESCRIPTION  	: EventReceiverPropertyValidator class to validate properties required by service
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Messaging;
using SJP.Common;
using SJP.Common.PropertyManager;

namespace SJP.Reporting.EventReceiver
{
    /// <summary>
    /// EventReceiverPropertyValidator class to validate properties required by service
    /// </summary>
    public class EventReceiverPropertyValidator : PropertyValidator
    {
        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="properties">Properties.</param>
        public EventReceiverPropertyValidator(IPropertyProvider properties)
            : base(properties)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Validates the given property.
        /// </summary>
        /// <param name="key">Property name to validate.</param>
        /// <param name="errors">List to append any errors.</param>
        /// <returns>true on success, false on validation failure.</returns>
        public override bool ValidateProperty(string key, List<string> errors)
        {
            if (key == Keys.ReceiverQueue)
                return ValidateReceiverQueue(errors);
            else
            {
                throw new SJPException(String.Format(Messages.Init_UnknownPropertyKey, key), false, SJPExceptionIdentifier.RDPEventReceiverUnknownPropertyKey);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Checks that for every space delimited 'ID' entry in the Receiver.Queue property,
        /// there is a corresponding property of type Receiver.Queue.'ID'.Path in the property list.
        /// </summary>
        /// <param name="errors">Error list.</param>
        /// <returns>true on success, false on validation failure.</returns>
        private bool ValidateReceiverQueue(List<string> errors)
        {
            int errorsBefore = errors.Count;

            if (ValidateExistence(Keys.ReceiverQueue, Optionality.Mandatory, errors))
            {

                string[] idList = properties[Keys.ReceiverQueue].Split(' ');

                if (!(idList.Length == 1 && idList[0].Length == 0))
                {

                    string path = null;

                    foreach (string id in idList)
                    {
                        if (id != " ")
                        {
                            path = string.Format(Keys.ReceiverQueuePath, id);

                            // Go through and validate each property, and build up the error list.
                            if (ValidateExistence(path, Optionality.Mandatory, errors))
                            {
                                ValidateReceiverQueueCanRead(path, errors);
                            }
                        }
                    }
                }
                else
                {
                    errors.Add(String.Format(Messages.Validation_NoQueues, properties[Keys.ReceiverQueue], Keys.ReceiverQueue));
                }
            }

            return (errorsBefore == errors.Count);
        }

        /// <summary>
        /// Checks that queue exists in MSMQ.
        /// </summary>
        /// <param name="key">Property key.</param>
        /// <param name="errors">Error list.</param>
        /// <returns>true on success, false on validation failure.</returns>
        /// <remarks>
        /// This method should **not** be used to validate queues on remote computers
        /// when running in workgroup mode.
        /// </remarks>
        private bool ValidateReceiverQueuePath(string key, List<string> errors)
        {
            int errorsBefore = errors.Count;

            string queue = properties[key];

            if (!MessageQueue.Exists(queue))
            {
                errors.Add(String.Format(Messages.Validation_QueueMissing, properties[key], key));
            }

            return (errorsBefore == errors.Count);
        }


        /// <summary>
        /// Checks that the queue can be read.
        /// </summary>		
        /// <param name="key">Property key.</param>
        /// <param name="errors">Error list.</param>
        /// <returns>true on success, false on validation failure.</returns>
        /// <remarks>
        /// When validating queues on remote computers in workgroup mode,
        /// the queue path must be provided as a Direct Format Name.
        /// </remarks>
        private bool ValidateReceiverQueueCanRead(string key, List<string> errors)
        {
            int errorsBefore = errors.Count;
            string queue = properties[key];

            try
            {

                using (MessageQueue mq = new MessageQueue(queue))
                {
                    if (!mq.CanRead)
                    {
                        errors.Add(String.Format(Messages.Validation_QueueNotReadable, properties[key], key));
                    }
                }
            }
            catch (Exception e)
            {
                errors.Add(String.Format(Messages.Validation_TestQueueNotReadable, properties[key], key, e.Message));
            }

            return (errorsBefore == errors.Count);
        }

        #endregion
    }
}
