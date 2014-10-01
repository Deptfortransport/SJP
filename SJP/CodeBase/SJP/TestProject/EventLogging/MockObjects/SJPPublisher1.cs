using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.EventLogging;

namespace SJP.TestProject.EventLogging.MockObjects
{
    /// <summary>
    /// Test custom event publisher 
    /// used in testing SJPTraceSwitch/listener
    /// </summary>
    class SJPPublisher1: IEventPublisher
	{
		private string identifier;
		
		public string Identifier
		{
			get {return identifier;}
		}

		public void WriteEvent(LogEvent logEvent)
		{
			
		}

        public SJPPublisher1(string identifier)
		{
			this.identifier = identifier;
		}
	}
}
