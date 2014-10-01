// *********************************************** 
// NAME             : Keys.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Apr 2011
// DESCRIPTION  	: Keys class for properties used by the service
// ************************************************
// 

namespace SJP.Reporting.EventReceiver
{
    /// <summary>
    /// Keys class for properties used by the service
    /// </summary>
    public class Keys
    {
        public const string ReceiverQueue = "EventReceiver.Queue";
        public const string ReceiverQueuePath = ReceiverQueue + ".{0}.Path";
        public const string TimeBeforeRecovery = "EventReceiver.TimeBeforeRecovery.Millisecs";
    }
}
