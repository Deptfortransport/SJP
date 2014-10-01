using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.EventLogging;

namespace SJP.TestProject.EventLogging
{
    /// <summary>
    /// A custom event used for NUnit testing
    /// </summary>
    [Serializable]
    public class CustomEventOne : CustomEvent
    {
        private SJPEventCategory category;
        private SJPTraceLevel level;
        private string message;
        private string user;
        private long referenceNumber;

        private static IEventFormatter fileFormatter = new MockCustomEventFileFormatter();
        private static IEventFormatter emailFormatter = new MockCustomEventEmailFormatter();
        private static IEventFormatter eventLogFormatter = new MockCustomEventEventLogFormatter();
        private static IEventFormatter consoleFormatter = new MockCustomEventConsoleFormatter();

        private static IEventFilter filter = new OperationalEventFilter();

        override public IEventFormatter FileFormatter
        {
            get { return fileFormatter; }
        }

        override public IEventFormatter EmailFormatter
        {
            get { return emailFormatter; }
        }

        override public IEventFormatter EventLogFormatter
        {
            get { return eventLogFormatter; }
        }

        override public IEventFormatter ConsoleFormatter
        {
            get { return consoleFormatter; }
        }

        override public IEventFilter Filter
        {
            get { return filter; }
        }

        public SJPTraceLevel Level
        {
            get { return level; }
        }

        public SJPEventCategory Category
        {
            get { return category; }
        }

        public string Message
        {
            get { return message; }
        }

        public long ReferenceNumber
        {
            get { return referenceNumber; }
        }

        public string User
        {
            get { return user; }
        }

        public CustomEventOne(SJPEventCategory category, SJPTraceLevel level, string message,
            string user, long referenceNumber)
            : base()
        {
            this.category = category;
            this.level = level;
            this.message = message;
            this.referenceNumber = referenceNumber;
            this.user = user;
        }
    }
}
