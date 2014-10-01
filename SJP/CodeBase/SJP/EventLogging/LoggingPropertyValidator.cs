// *********************************************** 
// NAME             : LoggingPropertyProvider.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Utility class used to validate logging specific properties
// ************************************************


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Messaging;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;

namespace SJP.Common.EventLogging
{
    /// <summary>
    /// Utility class used to validate logging specific properties.
    /// Derives from PropertyValidator which provides general
    /// validation methods.
    /// </summary>
    public class LoggingPropertyValidator : PropertyValidator
    {
        #region Constants
        /// <summary>
        /// Maximum character length of email subject lines
        /// </summary>
        public const int MaxSubjectLength = 35;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor used to create class based on properties.
        /// </summary>
        /// <param name="properties">Properties that are used to perform validation against.</param>
        public LoggingPropertyValidator(IPropertyProvider properties) :
            base(properties)
        { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Used to validate a property with a given <c>key</c>.
        /// </summary>
        /// <param name="key">Key of property to validate.</param>
        /// <param name="errors">Holds validation errors if any.</param>
        /// <returns><c>true</c> if property is valid otherwise <c>false</c>.</returns>
        public override bool ValidateProperty(string key, List<string> errors)
        {
            // A validation method is defined for every key that may be passed.

            if (key == Keys.QueuePublishers)
                return ValidateQueuePublishers(errors);
            else if (key == Keys.EmailPublishers)
                return ValidateEmailPublishers(errors);
            else if (key == Keys.FilePublishers)
                return ValidateFilePublishers(errors);
            else if (key == Keys.EventLogPublishers)
                return ValidateEventLogPublishers(errors);
            else if (key == Keys.OperationalTraceLevel)
                return ValidateOperationalEventTraceLevel(errors);
            else if (key == Keys.CustomPublishers)
                return ValidateCustomPublishers(errors);
            else if (key == Keys.DefaultPublisher)
                return ValidateDefaultPublisher(errors);
            else if (key == Keys.CustomEventsLevel)
                return ValidateCustomEventsLevel(errors);
            else if (key == Keys.CustomEvents)
                return ValidateCustomEvents(errors);
            else if (key == Keys.ConsolePublishers)
                return ValidateConsolePublishers(errors);
            else
            {
                string message = String.Format(Messages.LoggingPropertyValidatorKeyBad, key);
                throw new SJPException(message, false, SJPExceptionIdentifier.ELSLoggingPropertyValidatorUnknownKey);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Validates SJPTraceLevel properties set correctly
        /// </summary>
        /// <param name="errors">List keeping track of errors generated while validating</param>
        /// <returns>True if the SJPTraceLevel properties are set correctly in property store</returns>
        private bool ValidateOperationalEventTraceLevel(List<string> errors)
        {
            int errorsBefore = errors.Count;

            // NB Can take any value in SJPTraceLevel except Undefined
            ValidateEnumProperty(Keys.OperationalTraceLevel,
                                 typeof(SJPTraceLevel), Optionality.Mandatory, errors);

            if (String.Compare(properties[Keys.OperationalTraceLevel], "Undefined") == 0)
            {
                errors.Add(String.Format(SJP.Common.Messages.PropertyValueBad,
                                         Keys.OperationalTraceLevel,
                                         properties[Keys.OperationalTraceLevel],
                                         "Value cannot take value Undefined."));

            }

            return (errorsBefore == errors.Count);
        }

        /// <summary>
        /// Checks if writable Message queue exists at the specified path
        /// </summary>
        /// <param name="path">Message queue path</param>
        /// <returns>True if Message queue exists, false otherwise</returns>
        private bool QueueExists(string path)
        {
            // NB. Not possible to use the Exists method since will not work for remote private queues.
            // Instead use CanWrite after creating a queue object based on the path.
            bool valid = true;
            MessageQueue queue = null;

            try
            {
                using (queue = new MessageQueue(path))
                {
                    valid = queue.CanWrite;
                }
            }
            catch (Exception)
            {
                valid = false;
                queue = null;
            }
            
            return valid;
        }

        /// <summary>
        /// Validates queue publisher properties set correctly in property store
        /// </summary>
        /// <param name="errors">List of error messages</param>
        /// <returns>True if the queue publisher properties set correctly, false otherwise</returns>
        private bool ValidateQueuePublishers(List<string> errors)
        {
            if (!ValidateExistence(Keys.QueuePublishers, Optionality.Mandatory, errors))
                return false;

            string[] publisherIDs = null;
            string publishers = properties[Keys.QueuePublishers];
            if (publishers.Length != 0) // valid to have no publishers
                publisherIDs = publishers.Split(' ');
            else
                publisherIDs = new String[0];
            StringBuilder key = new StringBuilder(50);
            int errorsBefore = errors.Count;

            foreach (string publisherID in publisherIDs)
            {
                key.Length = 0;
                key.Append(String.Format(Keys.QueuePublisherPath, publisherID));

                if (ValidateExistence(key.ToString(), Optionality.Mandatory, errors))
                {
                    if (!QueueExists(properties[key.ToString()]))
                    {
                        errors.Add(String.Format(Messages.QueuePublisherMSMQBad,
                                                 properties[key.ToString()],
                                                 key.ToString()));
                    }
                }

                key.Length = 0;
                key.Append(String.Format(Keys.QueuePublisherPriority, publisherID));

                if (ValidateExistence(key.ToString(), Optionality.Mandatory, errors))
                    ValidateEnumProperty(key.ToString(),
                                         typeof(MessagePriority),
                                         Optionality.Mandatory, errors);




                key.Length = 0;
                key.Append(String.Format(Keys.QueuePublisherDelivery, publisherID));

                if (ValidateExistence(key.ToString(), Optionality.Mandatory, errors))
                {
                    if ((String.Compare(properties[key.ToString()], "Express") != 0) &&
                         (String.Compare(properties[key.ToString()], "Recoverable") != 0))
                    {
                        errors.Add(String.Format(Messages.QueuePublisherDeliveryBad,
                                                           properties[key.ToString()],
                                                           key.ToString()));
                    }
                }


            }

            return (errorsBefore == errors.Count);
        }

        /// <summary>
        /// Validates if the Email publisher properties are set correctly in property store
        /// </summary>
        /// <param name="errors">List keeping track of errors generated while validating</param>
        /// <returns>True if email publisher properties are set correctly, false otherwise</returns>
        private bool ValidateEmailPublishers(List<string> errors)
        {
            if (!ValidateExistence(Keys.EmailPublishers, Optionality.Mandatory, errors))
                return false;

            string[] publisherIDs = null;
            string publishers = properties[Keys.EmailPublishers];
            if (publishers.Length != 0) // valid to have no publishers
                publisherIDs = publishers.Split(' ');
            else
                publisherIDs = new String[0];
            StringBuilder key = new StringBuilder(50);
            
           int errorsBefore = errors.Count;

            foreach (string publisherID in publisherIDs)
            {
                key.Length = 0;
                key.Append(String.Format(Keys.EmailPublisherTo, publisherID));

                if (ValidateExistence(key.ToString(), PropertyValidator.Optionality.Mandatory, errors))
                {
                    if (!properties[key.ToString()].IsValidEmailAddress())
                        errors.Add(String.Format(Messages.EmailPublisherAddressBad,
                                                 properties[key.ToString()],
                                                 key.ToString()));

                }


                key.Length = 0;
                key.Append(String.Format(Keys.EmailPublisherFrom, publisherID));

                if (ValidateExistence(key.ToString(), PropertyValidator.Optionality.Mandatory, errors))
                {
                    if (!properties[key.ToString()].IsValidEmailAddress())
                        errors.Add(String.Format(Messages.EmailPublisherAddressBad,
                                                 properties[key.ToString()],
                                                 key.ToString()));

                }

                key.Length = 0;
                key.Append(String.Format(Keys.EmailPublisherSubject, publisherID));

                if (ValidateExistence(key.ToString(), PropertyValidator.Optionality.Mandatory, errors))
                    ValidateLength(key.ToString(), 1, MaxSubjectLength, errors);

                key.Length = 0;
                key.Append(String.Format(Keys.EmailPublisherPriority, publisherID));

                if (ValidateExistence(key.ToString(), PropertyValidator.Optionality.Mandatory, errors))
                    ValidateEnumProperty(key.ToString(), typeof(MailPriority), Optionality.Mandatory, errors);
            }

            if (publisherIDs.Length > 0)
            {
                key.Length = 0;
                key.Append(String.Format(Keys.EmailPublishersSmtpServer));

                if (ValidateExistence(key.ToString(), Optionality.Mandatory, errors))
                    ValidateLength(key.ToString(), 1, errors);
            }

            return (errorsBefore == errors.Count);
        }

        /// <summary>
        /// Validates custom publisher properties
        /// </summary>
        /// <param name="errors">List keeping track of errors generated while validating</param>
        /// <returns>True if the custom publisher properties are set correctly, false otherwise</returns>
        private bool ValidateCustomPublishers(List<string> errors)
        {
            if (!ValidateExistence(Keys.CustomPublishers, Optionality.Mandatory, errors))
                return false;

            string[] publisherIDs = null;
            string publishers = properties[Keys.CustomPublishers];
            if (publishers.Length != 0) // valid to have no publishers
                publisherIDs = publishers.Split(' ');
            else
                publisherIDs = new String[0];


            StringBuilder key = new StringBuilder(50);

            int errorsBefore = errors.Count;

            foreach (string publisherID in publisherIDs)
            {
                key.Length = 0;
                key.Append(String.Format(Keys.CustomPublisherName, publisherID));

                if (ValidateExistence(key.ToString(), Optionality.Mandatory, errors))
                    ValidateLength(key.ToString(), 1, errors);


            }

            return (errorsBefore == errors.Count);
        }

        /// <summary>
        ///  Validates file publisher properties
        /// </summary>
        /// <param name="errors">List keeping track of errors generated while validating</param>
        /// <returns>True if file publisher properties set correctly, false otherwise</returns>
        private bool ValidateFilePublishers(List<string> errors)
        {
            if (!ValidateExistence(Keys.FilePublishers, Optionality.Mandatory, errors))
                return false;

            string[] publisherIDs = null;
            string publishers = properties[Keys.FilePublishers];
            if (publishers.Length != 0) // valid to have no publishers
                publisherIDs = publishers.Split(' ');
            else
                publisherIDs = new String[0];

            StringBuilder key = new StringBuilder(50);
            int errorsBefore = errors.Count;

            foreach (string publisherID in publisherIDs)
            {
                key.Length = 0;
                key.Append(String.Format(Keys.FilePublisherDirectory, publisherID));

                if (ValidateExistence(key.ToString(), Optionality.Mandatory, errors))
                {
                    if (!Directory.Exists(properties[key.ToString()]))
                        errors.Add(String.Format(Messages.FilePublisherDirectoryBad, properties[key.ToString()], key.ToString()));
                }

                key.Length = 0;
                key.Append(String.Format(Keys.FilePublisherRotation, publisherID));

                if (ValidateExistence(key.ToString(), Optionality.Mandatory, errors))
                {
                    if (ValidateLength(key.ToString(), 1, errors))
                    {
                        CultureInfo cultureInfo = new CultureInfo("en-US");
                        string property = properties[key.ToString()];

                        int rotation = int.Parse(property, cultureInfo);

                        if (rotation <= 0)
                            errors.Add(String.Format(Messages.FilePublisherRotationBad, properties[key.ToString()], key.ToString()));

                    }
                }

            }

            return (errorsBefore == errors.Count);
        }


        /// <summary>
        /// Validates event log publisher properties
        /// </summary>
        /// <param name="errors">List keeping track of errors generated while validating</param>
        /// <returns>True if event log publisher proeprties are set correctly, false otherwise</returns>
        private bool ValidateEventLogPublishers(List<string> errors)
        {
            if (!ValidateExistence(Keys.EventLogPublishers, Optionality.Mandatory, errors))
                return false;

            string[] publisherIDs = null;
            string publishers = properties[Keys.EventLogPublishers];
            if (publishers.Length != 0) // valid to have no publishers
                publisherIDs = publishers.Split(' ');
            else
                publisherIDs = new String[0];
            StringBuilder key1 = new StringBuilder(50);
            StringBuilder key2 = new StringBuilder(50);
            bool nameValid = false;
            bool machineValid = false;
            int errorsBefore = errors.Count;

            foreach (string publisherID in publisherIDs)
            {
                nameValid = false;
                machineValid = false;

                key1.Length = 0;
                key1.Append(String.Format(Keys.EventLogPublisherName, publisherID));

                if (ValidateExistence(key1.ToString(), Optionality.Mandatory, errors))
                {
                    if (properties[key1.ToString()].Equals("Application"))
                    {
                        nameValid = true; // configured to use standard Application Event Log
                    }
                    else
                    {
                        if (ValidateLength(key1.ToString(), 1, 8, errors))
                            nameValid = true; // configured to use a custom event log
                    }
                }

                key2.Length = 0;
                key2.Append(String.Format(Keys.EventLogPublisherMachine, publisherID));

                if (ValidateExistence(key2.ToString(), Optionality.Mandatory, errors))
                {
                    if (ValidateLength(key2.ToString(), 1, errors))
                        machineValid = true;
                }

                if (machineValid && nameValid)
                {
                    try
                    {
                        if (!EventLog.Exists(properties[key1.ToString()], properties[key2.ToString()]))
                        {
                            errors.Add(String.Format(Messages.EventLogPublisherEventLogBad,
                                properties[key1.ToString()],
                                properties[key2.ToString()],
                                key1.ToString(), key2.ToString()));

                        }
                    }
                    catch (Exception exception)
                    {
                        // MS have not documented all possible exceptions so catch all	
                        errors.Add(String.Format(Messages.EventLogPublisherEventLogNotFound,
                                   exception.Message,
                                   key1.ToString(), key2.ToString()));
                    }
                }


                key1.Length = 0;
                key1.Append(String.Format(Keys.EventLogPublisherSource, publisherID));
                ValidateExistence(key1.ToString(), Optionality.Mandatory, errors);

            }

            return (errorsBefore == errors.Count);
        }

        /// <summary>
        /// Validates default publisher properties
        /// </summary>
        /// <param name="errors">List keeping track of errors generated while validating</param>
        /// <returns>True when default publisher properties are set correctly.</returns>
        private bool ValidateDefaultPublisher(List<string> errors)
        {
            int errorsBefore = errors.Count;

            if (ValidateExistence(Keys.DefaultPublisher, Optionality.Mandatory, errors))
            {
                ValidateLength(Keys.DefaultPublisher, 1, errors);
            }


            return (errorsBefore == errors.Count);
        }

        /// <summary>
        /// Validates custom event level property
        /// </summary>
        /// <param name="errors">List keeping track of errors generated while validating</param>
        /// <returns>True if the custom event level property validated otherwiser returnes false</returns>
        private bool ValidateCustomEventsLevel(List<string> errors)
        {
            int errorsBefore = errors.Count;

            if (ValidateExistence(Keys.CustomEventsLevel, Optionality.Mandatory, errors))
            {
                ValidateEnumProperty(Keys.CustomEventsLevel,
                                     typeof(CustomEventLevel),
                                     Optionality.Mandatory, errors);

                string property = properties[Keys.CustomEventsLevel];

                if (String.Compare(property, "Undefined") == 0)
                {
                    errors.Add(String.Format(SJP.Common.Messages.PropertyValueBad,
                                             Keys.CustomEventsLevel, property,
                                             "Value cannot take value Undefined."));

                }
            }

            return (errorsBefore == errors.Count);
        }

        /// <summary>
        /// Validates that the base class of a type matches a given type.
        /// </summary>
        /// <param name="key">Property key whose value identifies the type</param>
        /// <param name="requiredSubclass">Perent class to check inheritence for</param>
        /// <param name="assembly">Assembly which consists the class represented by property</param>
        /// <param name="errors">List keeping track of errors generated while validating</param>
        private void ValidateBaseType(string key, Type requiredSubclass,
                                      Assembly assembly, List<string> errors)
        {
            string classTypeName = properties[key];
            Type[] types = assembly.GetTypes();

            foreach (Type type in types)
            {
                if (type.Name == classTypeName)
                {
                    if (!type.IsSubclassOf(requiredSubclass))
                    {
                        errors.Add(String.Format(Messages.IncorrectClassType,
                                   classTypeName,
                                   key,
                                   requiredSubclass.FullName));
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Validates proeprties of custom event identified using eventId
        /// </summary>
        /// <param name="eventId">Id of custom event</param>
        /// <param name="errors">List keeping track of errors generated while validating</param>
        private void ValidateCustomEvent(string eventId, List<string> errors)
        {
            string nameKey = String.Format(Keys.CustomEventName, eventId);
            string assemblyKey = String.Format(Keys.CustomEventAssembly, eventId);
            string traceKey = String.Format(Keys.CustomEventLevel, eventId);
            bool nameValid = false;
            bool assemblyNameValid = false;

            if (ValidateExistence(nameKey, Optionality.Mandatory, errors))
            {
                if (ValidateLength(nameKey, 1, errors))
                {
                    nameValid = true;
                }
            }

            if (ValidateExistence(assemblyKey, Optionality.Mandatory, errors))
            {
                if (ValidateLength(assemblyKey, 1, errors))
                {
                    assemblyNameValid = true;
                }
            }

            if (nameValid && assemblyNameValid)
            {
                Assembly assembly = ValidateAssembly(assemblyKey, errors);

                if (assembly != null)
                {
                    if (ValidateClassExists(nameKey, assembly, errors))
                        ValidateBaseType(nameKey, typeof(CustomEvent), assembly, errors);
                }
            }

            if (ValidateExistence(traceKey, Optionality.Mandatory, errors))
            {
                string property = properties[traceKey];

                ValidateEnumProperty(traceKey,
                                     typeof(CustomEventLevel),
                                     Optionality.Mandatory, errors);

                if (String.Compare(property, "Undefined") == 0)
                {
                    errors.Add(String.Format(SJP.Common.Messages.PropertyValueBad,
                                             traceKey, property,
                                             "Value cannot take value Undefined."));

                }
            }

        }

        /// <summary>
        /// Validates custom event properties 
        /// </summary>
        /// <param name="errors">List keeping track of errors generated while validating</param>
        /// <returns>True if the properties are valid</returns>
        private bool ValidateCustomEvents(List<string> errors)
        {
            int errorsBefore = errors.Count;

            if (ValidateExistence(Keys.CustomEvents, Optionality.Mandatory, errors))
            {
                string[] eventIds = null;
                string events = properties[Keys.CustomEvents];
                if (events.Length != 0) // valid to have no custom events
                    eventIds = events.Split(' ');
                else
                    eventIds = new String[0];

                foreach (string eventId in eventIds)
                    ValidateCustomEvent(eventId, errors);

            }

            return (errorsBefore == errors.Count);
        }

        /// <summary>
        /// Validates console publisher properties
        /// </summary>
        /// <param name="errors">List keeping track of errors generated while validating</param>
        /// <returns>True if the console publisher properties are valid</returns>
        private bool ValidateConsolePublishers(List<string> errors)
        {
            if (!ValidateExistence(Keys.ConsolePublishers, Optionality.Mandatory, errors))
                return false;

            string[] publisherIDs = null;
            string publishers = properties[Keys.ConsolePublishers];

            if (publishers.Length != 0) // valid to have no publishers
                publisherIDs = publishers.Split(' ');
            else
                publisherIDs = new String[0];
            StringBuilder key = new StringBuilder(50);
            int errorsBefore = errors.Count;

            foreach (string publisherID in publisherIDs)
            {
                key.Length = 0;
                key.Append(String.Format(Keys.ConsolePublisherStream, publisherID));

                string property = properties[key.ToString()];

                if (ValidateExistence(key.ToString(), Optionality.Mandatory, errors))
                {
                    if ((String.Compare(property, Keys.ConsolePublisherErrorStream) != 0) &&
                         (String.Compare(property, Keys.ConsolePublisherOutputStream) != 0))
                    {
                        string usage = Keys.ConsolePublisherErrorStream + "|" + Keys.ConsolePublisherOutputStream;

                        errors.Add(String.Format(SJP.Common.Messages.PropertyValueBad,
                                                  key.ToString(), property, usage));

                    }
                }

            }

            return (errorsBefore == errors.Count);
        }

        #endregion

    }
}
