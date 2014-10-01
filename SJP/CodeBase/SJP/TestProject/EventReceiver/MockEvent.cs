using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.EventLogging;
using SJP.TestProject.EventLogging.MockObjects;

namespace SJP.TestProject.EventReceiver
{
    [Serializable]
    public class MockEvent: CustomEvent
	{
		
		public MockEvent() : base()
		{
			
		}

		override public IEventFormatter FileFormatter
		{
			get {return SJP.Common.EventLogging.DefaultFormatter.Instance;}
		}

		override public IEventFormatter EmailFormatter
		{
			get {return SJP.Common.EventLogging.DefaultFormatter.Instance;}
		}

		override public IEventFormatter EventLogFormatter
		{
			get {return SJP.Common.EventLogging.DefaultFormatter.Instance;}
		}


		override public IEventFormatter ConsoleFormatter
		{
			get {return SJP.Common.EventLogging.DefaultFormatter.Instance;}
		}

	}
}
