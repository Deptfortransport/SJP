// *********************************************** 
// NAME             : EmailPublisher.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Publishes events via SMTP mail
// ************************************************           
                
using System;
using System.Net.Mail;

namespace SJP.Common.EventLogging
{
    /// <summary>
    /// Publishes events via SMTP mail.
    /// </summary>
    public class EmailPublisher : IEventPublisher
    {
        #region Private Fields
        private string to;
        private string from;
        private string subject;
        private MailPriority priority;
        private string identifier;
        private string smtpServer;
        #endregion

        #region Constructors
        /// <summary>
        /// Create a publisher that sends event details via e-mail.
        /// It is assumed that all parameters have been pre-validated.
        /// </summary>
        /// <param name="identifier">Identifer</param>
        /// <param name="destination">E-mail address to send to.</param>
        /// <param name="from">E-mail address of sendee.</param>
        /// <param name="subject">Subject of the e-mail message.</param>
        /// <param name="priority">Prioirty of the e-mail message</param>
        /// <param name="smtpServer">Name of the SMTP Server in which to send messages from.</param>
        public EmailPublisher(string identifier, string destination, string from, string subject, MailPriority priority, string smtpServer)
        {
            this.to = destination;
            this.from = from;
            this.subject = subject;
            this.priority = priority;
            this.identifier = identifier;
            this.smtpServer = smtpServer;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public string Identifier
        {
            get { return identifier; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Sends an email containing details of the given log event.
        /// </summary>
        /// <param name="logEvent"><c>LogEvent</c> to send details for.</param>
        /// <exception cref="SJP.Common.SJPException">Log Event was not successfully emailed.</exception>
        public void WriteEvent(LogEvent logEvent)
        {
            // set-up the e-mail message
            string formatString = String.Empty;
            formatString = logEvent.EmailFormatter.AsString(logEvent);
            
            using (MailMessage message = new MailMessage(from, to, subject, formatString))
            {
                try
                {
                
                        message.Priority = priority;
                        // send the message            
                        SmtpClient client = new SmtpClient(smtpServer);
                        client.Send(message);
                
                }
                catch (Exception e) // Catch Exception due to lack of MS documentation
                {
                    string msg =
                        String.Format(Messages.EmailPublisherWriteEvent,
                        "to:" + to + "," + "from:" +
                        from + "," + "subject:" + subject + "," + "priority:" + priority +
                        "," + "identifier:" + identifier + "," + "smtpServer:" +
                        smtpServer + "," + "MessageBody:" + message.Body
                        + "," + "LogEventString:" + formatString);

                    throw new SJPException(msg, e, false, SJPExceptionIdentifier.ELSEmailPublisherWritingEvent);
                }
            }
            
        }
        #endregion

        
    }
}
