using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.EventLogging;

namespace SJP.TestProject.EventLogging
{
    /// <summary>
    /// A console formatter for CustomEventOne
    /// </summary>
    public class MockCustomEventConsoleFormatter : IEventFormatter
    {
        public MockCustomEventConsoleFormatter()
        {
        }

        public string AsString(LogEvent logEvent)
        {
            string output = String.Empty;

            if (logEvent is CustomEventOne)
            {
                CustomEventOne customEventOne = (CustomEventOne)logEvent;

                output =
                    "SJP-CustomEventOne" + " " +
                    customEventOne.Time + " " +
                    customEventOne.Message + " " +
                    customEventOne.Category + " " +
                    customEventOne.Level + " " +
                    customEventOne.ReferenceNumber;
            }
            return output;
        }
    }
}
