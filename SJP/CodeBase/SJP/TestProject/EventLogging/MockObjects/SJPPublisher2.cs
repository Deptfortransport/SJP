using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.EventLogging;
using SJP.Common;

namespace SJP.TestProject.EventLogging.MockObjects
{
    /// <summary>
    /// Test custom event publisher 
    /// used in testing SJPTraceSwitch/listener
    /// </summary>
    class SJPPublisher2: IEventPublisher
	{
		private string identifier;
		
		public string Identifier
		{
			get {return identifier;}
		}

		public void WriteEvent(LogEvent logEvent)
		{
			throw new SJPException("mock write error", false, SJPExceptionIdentifier.ELSCustomEmailPublisherWritingEvent);
		}

        public SJPPublisher2(string identifier)
		{
			this.identifier = identifier;
		}
	}
}
