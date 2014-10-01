using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.EventLogging;

namespace SJP.TestProject.EventLogging
{
    /// <summary>
    /// An email formatter for CustomEventOne
    /// </summary>
    public class MockCustomEventEmailFormatter : IEventFormatter
    {
        public MockCustomEventEmailFormatter()
        {
        }

        public string AsString(LogEvent logEvent)
        {
            string output = String.Empty;

            if (logEvent is CustomEventOne)
            {
                CustomEventOne customEventOne =
                    (CustomEventOne)logEvent;

                output =
                    "SJP-CustomEventOne\n\n" +
                    "Time: " + customEventOne.Time + "\n" +
                    "Message: " + customEventOne.Message + "\n" +
                    "Category: " + customEventOne.Category + "\n" +
                    "Level: " + customEventOne.Level + "\n" +
                    "Reference: " + customEventOne.ReferenceNumber;

            }

            return output;

        }
    }
}
