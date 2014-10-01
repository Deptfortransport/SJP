// *********************************************** 
// NAME             : EventPublisherGroup.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Factory class that creates a group of event publishers
// ************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Messaging;
using System.Net.Mail;
using SJP.Common.PropertyManager;

namespace SJP.Common.EventLogging
{
    /// <summary>
    /// Factory class that creates a group of event publishers.
    /// </summary>
    class EventPublisherGroup : PublisherGroup
    {

        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="properties">Properties used to create publishers.</param>
        public EventPublisherGroup(IPropertyProvider properties)
            : base(properties)
        { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Main factory method for creating publishers based on properties.
        /// </summary>
        /// <param name="errors">Stores errors occuring during validation and creation.</param>
        /// <exception cref="SJP.Common.SJPException">Unrecoverable error occured when creating publishing group.</exception>
        public override void CreatePublishers(List<string> errors)
        {
            int errorsBeforeValidation = errors.Count;

            validator.ValidateProperty(Keys.QueuePublishers, errors);
            validator.ValidateProperty(Keys.EmailPublishers, errors);
            validator.ValidateProperty(Keys.FilePublishers, errors);
            validator.ValidateProperty(Keys.EventLogPublishers, errors);
            validator.ValidateProperty(Keys.CustomPublishers, errors);
            validator.ValidateProperty(Keys.ConsolePublishers, errors);

            if (errors.Count == errorsBeforeValidation)
            {
                CreateEmailPublishers(errors);
                CreateQueuePublishers(errors);
                CreateFilePublishers(errors);
                CreateEventLogPublishers(errors);
                CreateConsolePublishers(errors);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates event log publishers to publish event to
        /// </summary>
        /// <param name="errors">List to be populated with error messages generated while creating publishers</param>
        private void CreateEventLogPublishers(List<string> errors)
        {
            string[] ids = null;
            string publisherids = properties[Keys.EventLogPublishers];
            if (publisherids.Length != 0) // valid to have no publishers
                ids = publisherids.Split(' ');
            else
                ids = new String[0];

            try
            {
                foreach (string id in ids)
                {
                    publishers.Add(
                        new EventLogPublisher(id,
                        properties[String.Format(Keys.EventLogPublisherName, id)],
                        properties[String.Format(Keys.EventLogPublisherSource, id)],
                        properties[String.Format(Keys.EventLogPublisherMachine, id)]
                                              )
                        );
                }
            }
            catch (SJPException sjpException)
            {
                errors.Add(sjpException.Message);
                throw;
            }

        }

        /// <summary>
        /// Creates console publishers to output events to console
        /// </summary>
        /// <param name="errors">List to be populated with error messages generated while creating publishers</param>
        private void CreateConsolePublishers(List<string> errors)
        {
            string[] ids = null;
            string publisherids = properties[Keys.ConsolePublishers];
            if (publisherids.Length != 0) // valid to have no publishers
                ids = publisherids.Split(' ');
            else
                ids = new String[0];

            try
            {
                foreach (string id in ids)
                {
                    publishers.Add(new ConsolePublisher(id, properties[String.Format(Keys.ConsolePublisherStream, id)]));
                }
            }
            catch (SJPException sjpException)
            {
                errors.Add(sjpException.Message);
                throw;
            }

        }

        /// <summary>
        /// Generates a unique filepath based on the filename 
        /// format {ApplicationId}-{YYYYMMDD}{Time}
        /// Filename does not include an extension.
        /// </summary>
        /// <param name="directoryPath">Path to directory that will hold file.</param>
        /// <returns>Filepath.</returns>
        private string GenerateFilepath(string directoryPath)
        {
            string date = DateTime.Now.ToString
                ("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            string time = DateTime.Now.ToString
                ("HHmmssfffff", System.Globalization.CultureInfo.InvariantCulture);
            string applicationId = this.properties.ApplicationID;

            return (String.Format("{0}\\{1}-{2}{3}", directoryPath, applicationId, date, time));
        }

        /// <summary>
        /// Creates file publishers to output events to file
        /// </summary>
        /// <param name="errors">List to be populated with error messages generated while creating publishers</param>
        private void CreateFilePublishers(List<string> errors)
        {
            string[] ids = null;
            string publisherids = properties[Keys.FilePublishers];
            if (publisherids.Length != 0) // valid to have no publishers
                ids = publisherids.Split(' ');
            else
                ids = new String[0];

            CultureInfo cultureInfo = new CultureInfo("en-US");

            try
            {
                foreach (string id in ids)
                {
                    int rotation = int.Parse(properties[String.Format(Keys.FilePublisherRotation, id)], cultureInfo);
                    string filePath = GenerateFilepath(properties[String.Format(Keys.FilePublisherDirectory, id)]);

                    publishers.Add(new FilePublisher(id, rotation, filePath));
                }
            }
            catch (SJPException sjpException)
            {
                errors.Add(sjpException.Message);
                throw;
            }

        }

        /// <summary>
        /// Creates email publishers
        /// </summary>
        /// <param name="errors">List to be populated with error messages generated while creating publishers</param>
        private void CreateEmailPublishers(List<string> errors)
        {
            string[] ids = null;
            string publisherids = properties[Keys.EmailPublishers];
            if (publisherids.Length != 0) // valid to have no publishers
                ids = publisherids.Split(' ');
            else
                ids = new String[0];

            try
            {
                foreach (string id in ids)
                {
                    publishers.Add(
                        new EmailPublisher(id,
                        properties[String.Format(Keys.EmailPublisherTo, id)],
                        properties[String.Format(Keys.EmailPublisherFrom, id)],
                        properties[String.Format(Keys.EmailPublisherSubject, id)],
                        (MailPriority)PropertyValidator.StringToEnum(typeof(MailPriority),
                                                             properties[String.Format(Keys.EmailPublisherPriority, id)]),
                        properties[Keys.EmailPublishersSmtpServer])
                                 );
                }
            }
            catch (SJPException sjpException)
            {
                errors.Add(sjpException.Message);
                throw;
            }
        }

        /// <summary>
        /// Creates queue publishers to output events to MSMQ
        /// </summary>
        /// <param name="errors">List to be populated with error messages generated while creating publishers</param>
        private void CreateQueuePublishers(List<string> errors)
        {
            bool recoverable;
            string[] ids = null;
            string publisherids = properties[Keys.QueuePublishers];
            if (publisherids.Length != 0) // valid to have no publishers
                ids = publisherids.Split(' ');
            else
                ids = new String[0];

            try
            {
                foreach (string id in ids)
                {
                    if (properties[String.Format(Keys.QueuePublisherDelivery, id)] == "Express")
                        recoverable = false;
                    else
                        recoverable = true;

                    publishers.Add(
                        new QueuePublisher(id,
                        (MessagePriority)PropertyValidator.StringToEnum(typeof(MessagePriority),
                        properties[String.Format(Keys.QueuePublisherPriority, id)]),
                        properties[String.Format(Keys.QueuePublisherPath, id)],
                        recoverable
                        )
                        );
                }
            }
            catch (SJPException sjpException)
            {
                errors.Add(sjpException.Message);
                throw;
            }

        }

        #endregion
    }
}
