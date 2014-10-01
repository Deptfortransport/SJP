using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.EventLogging;

namespace SJP.TestProject.EventLogging.MockObjects
{
    [Serializable]
    class SJPEvent4: CustomEvent
	{
		
		public SJPEvent4() : base()
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

