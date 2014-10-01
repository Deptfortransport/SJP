// *********************************************** 
// NAME             : CustomEmailPublisher.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Custom publisher for sending emails (stored as CustomEmailEvent)
//                    with or without an attachment to one or more recipients.
// ************************************************                

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Mail;
using System.Security;
using SJP.Common.Extenders;

namespace SJP.Common.EventLogging
{
   /// <summary>
	/// Custom publisher for sending emails (stored as CustomEmailEvent)
	/// with or without an attachment to one or more recipients.
	/// </summary>
	public class CustomEmailPublisher : IEventPublisher
	{
        #region Private Fields
		private string identifier;
		private string from;
		private System.Net.Mail.MailPriority priority;
		private string smtpServer;
		private string workingDirectoryPath;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor for creating an Custom Email Publisher that sends event data via e-mail.
        /// Constructor will perform validation of parameters.
        /// </summary>
        /// <param name="identifier">Identifier associated with this publisher and that used in the configuration properties.</param>
        /// <param name="from">Default e-mail address of sendee.</param>
        /// <param name="priority">Priority of the e-mail message</param>
        /// <param name="smtpServer">Name of the SMTP Server in which to send messages from.</param>
        /// <param name="workingDirectoryPath">Path to a working directory in which publisher can temporarily construct attachment files.</param>
        /// <param name="errors">Used to store validation errors.</param>
        /// /// <exception cref="SJPException">
        /// Thrown if one or more of the constructor parameters are invalid.
        /// </exception>
        public CustomEmailPublisher(string identifier, string from, MailPriority priority, string smtpServer, string workingDirectoryPath, List<string> errors)
        {
            int errorCountStart = errors.Count;

            if (identifier.Length == 0)
                errors.Add(String.Format(Messages.CustomPublisherInvalidId, "EmailAttachmentPublisher", identifier));

            if (from.Length == 0)
                errors.Add(String.Format(Messages.CustomEmailPublisherPublisherFromAddressBad, from));
            else
            {

                if (!from.IsValidEmailAddress())
                    errors.Add(String.Format(Messages.CustomEmailPublisherPublisherFromAddressBad, from));
            }

            if (smtpServer.Length == 0)
                errors.Add(String.Format(Messages.CustomEmailPublisherPublisherSmtpServerBad, smtpServer));

            if (!Directory.Exists(workingDirectoryPath))
                errors.Add(String.Format(Messages.CustomEmailPublisherWorkingDirMissing, workingDirectoryPath));

            if (errors.Count == errorCountStart)
            {
                this.identifier = identifier;
                this.from = from;
                this.priority = priority;
                this.identifier = identifier;
                this.smtpServer = smtpServer;
                this.workingDirectoryPath = workingDirectoryPath;
            }
            else
            {
                throw new SJPException(Messages.CustomEmailPublisherConstructor, false, SJPExceptionIdentifier.ELSCustomEmailPublisherConstructor);
            }
        }
	
        #endregion

        #region Public Properties
        /// <summary>
		/// Gets the identifier associated with this publisher.
		/// This identifier is used to tie events with this publisher.
		/// This identifier must match with that used in the configuration properties
		/// </summary>
		public string Identifier
		{
			get {return identifier;}
		}
        #endregion

        #region Public Methods
        /// <summary>
        /// Publishes the event <c>LogEvent</c> passed.
        /// </summary>
        /// <remarks>
        /// This publisher is only able to publish custom events
        /// of type <c>EmailAttachmentPublisher</c>.
        /// </remarks>
        /// <exception cref="SJPException">
        /// Thrown if an unsupported <c>LogEvent</c> type is passed 
        /// or an error occurs publishing the event passed.
        /// </exception>
        /// <param name="logEvent">Event to be published.</param>
        public void WriteEvent(LogEvent logEvent)
        {
            if (logEvent is CustomEmailEvent)
            {
                CustomEmailEvent emailEvent = (CustomEmailEvent)logEvent;

                //If override of from address supplied by
                //CustomEmailEvent, then use that one instead

                string mailFrom = string.Empty;
                if (emailEvent.From == String.Empty)
                    mailFrom = this.from;
                else
                    mailFrom = emailEvent.From;

                emailEvent.To.Replace(' ', ';');

                string attachmentFilePath = string.Empty;

                using (MailMessage message = new MailMessage(mailFrom, emailEvent.To, emailEvent.Subject, emailEvent.BodyText))
                {
                    try
                    {

                        message.Priority = this.priority;
                        // Create attachment file

                        try
                        {

                            if (emailEvent.HasAttachmentStream)
                            {
                                CreateImageFileFromStream(emailEvent.AttachmentStream, emailEvent.AttachmentName, out attachmentFilePath);
                                // An attachment file has been created, attach it to the email
                                message.Attachments.Add(new Attachment(attachmentFilePath));
                            }
                            else if (emailEvent.HasAttachmentFilePath)
                            {
                                CopyImageFile(emailEvent.AttachmentFilePath, emailEvent.AttachmentName, out attachmentFilePath);
                                // An attachment file has been created, attach it to the email
                                message.Attachments.Add(new Attachment(attachmentFilePath));
                            }

                        }
                        catch (System.IO.FileNotFoundException)
                        {
                            // Fail quietly in this case. A user may have attempted to send email
                            // after GIS housekeeping has removed the image file (if they took too
                            // long to click send)
                        }
                        catch (Exception e) // exceptions not documented so catch all
                        {
                            // log error and throw exception
                            OperationalEvent opEvent =
                                new OperationalEvent(SJPEventCategory.Infrastructure,
                                SJPTraceLevel.Error,
                                String.Format(Messages.CustomEmailPublisherSaveFailed, attachmentFilePath),
                                e);

                            Trace.Write(opEvent);

                            throw new SJPException(String.Format(
                                Messages.CustomEmailPublisherSaveFailed,
                                attachmentFilePath),
                                true,
                                SJPExceptionIdentifier.ELSCustomEmailPublisherSavingStream);
                        }

                        SmtpClient client = new SmtpClient(this.smtpServer);
                        client.Send(message);



                    }
                    catch (Exception exception) // Catch ALL Exceptions due to lack of MS documentation on Smtp
                    {
                        string msg = String.Format(Messages.CustomEmailPublisherWriteEventFailed,
                                                   this.identifier,
                                                   emailEvent.To,
                                                   message.From,
                                                   emailEvent.Subject,
                                                   this.priority,
                                                   this.smtpServer,
                                                   emailEvent.BodyText);

                        throw new SJPException(msg, exception, false, SJPExceptionIdentifier.ELSCustomEmailPublisherWritingEvent);
                    }
                    finally
                    {

                        if (emailEvent.HasAttachment)
                        {
                            // delete file attachment and directory. Do not throw exception if delete fails since this signifies that event was not published - this may not be the case.
                            try
                            {
                                Directory.Delete(Path.GetDirectoryName(attachmentFilePath), true);
                            }
                            catch (SecurityException securityException)
                            {
                                OperationalEvent oe =
                                    new OperationalEvent(SJPEventCategory.Infrastructure,
                                    SJPTraceLevel.Error,
                                    String.Format(Messages.CustomEmailPublisherFileDeleteFailed, attachmentFilePath),
                                    securityException);

                                Trace.Write(oe);
                            }
                            catch (ArgumentException argumentException)
                            {
                                OperationalEvent oe =
                                    new OperationalEvent(SJPEventCategory.Infrastructure,
                                    SJPTraceLevel.Error,
                                    String.Format(Messages.CustomEmailPublisherFileDeleteFailed, attachmentFilePath),
                                    argumentException);

                                Trace.Write(oe);
                            }
                            catch (IOException ioException)
                            {

                                OperationalEvent oe =
                                    new OperationalEvent(SJPEventCategory.Infrastructure,
                                    SJPTraceLevel.Error,
                                    String.Format(Messages.CustomEmailPublisherFileDeleteFailed, attachmentFilePath),
                                    ioException);

                                Trace.Write(oe);
                            }
                        }


                    }
                }
            }
            else
            {
                throw new SJPException(Messages.CustomEmailPublisherUnsupportedEventType, false, SJPExceptionIdentifier.ELSCustomEmailPublisherUnsupportedEvent);
            }
        }

       
        #endregion

        #region Private Methods
        /// <summary>
        /// Copies the specified file to a new directory and gives it the supplied name.
        /// Deletion of the newly created directory and file is the responsibility of the
        /// caller.
        /// </summary>
        /// <param name="attachmentFilePath">Full path of file to copy</param>
        /// <param name="fileName">Filename to use for newly created file</param>
        /// <param name="destinationFilePath">Full path of destination file, returned 
        /// irrespective of whether file creation was successfull.</param>
        private void CopyImageFile(string attachmentFilePath, string fileName, out string destinationFilePath)
        {
            // generate unique dirpath using a GUID
            string dirPath = Path.Combine(workingDirectoryPath, Guid.NewGuid().ToString());
            destinationFilePath = Path.Combine(dirPath, fileName);

            Directory.CreateDirectory(dirPath);
            File.Copy(attachmentFilePath, destinationFilePath);
        }

        /// <summary>
        /// Creates a new directory and in it creates a new file from the supplied stream
        /// giving it the supplied name.
        /// Deletion of the newly created directory and file is the responsibility of the
        /// caller.
        /// </summary>
        /// <param name="attachmentStream">File source</param>
        /// <param name="fileName">Filename to use for newly created file</param>
        /// <param name="destinationFilePath">Full path of destination file, returned 
        /// irrespective of whether file creation was successfull.</param>
        private void CreateImageFileFromStream(Stream attachmentStream, string fileName, out string destinationFilePath)
        {
            // generate unique dirpath using a GUID
            string dirPath = Path.Combine(workingDirectoryPath, Guid.NewGuid().ToString());
            destinationFilePath = Path.Combine(dirPath, fileName);

            Directory.CreateDirectory(dirPath);
            Bitmap bitmap = new Bitmap(attachmentStream);
            bitmap.Save(destinationFilePath);
        }

        #endregion

		
    }
}
