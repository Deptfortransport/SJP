// *********************************************** 
// NAME             : OperationalEvent.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Defines an operational event
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace SJP.Common.EventLogging
{
    /// <summary>
    /// OperationalEvent class.
    /// Exhibits serializable attribute to allow publishing on MSMQ.
    /// </summary>
    [Serializable]
    public sealed class OperationalEvent : LogEvent
    {
        #region Constants
        private const string ContextUnassigned = "No Context";
        
        public const string SessionIdUnassigned = "No Session";
        // Prefix for forming the class name that is used by SJPTraceListener to associate operational event levels with publishers
        public const string ClassNamePrefix = "OperationalEvent";
        #endregion

        #region Private Static Fields
        private static Dictionary<SJPTraceLevelOverride,IEventFilter> filters = null; 
        #endregion

        #region Private Fields
        private SJPEventCategory category;
        private SJPTraceLevel level;
        private string message;
        private object target;
        private string typeName;
        private string methodName;
        private string assemblyName;
        private string machineName;
        private string sessionId;
        private SJPTraceLevelOverride overrideLevel;
        
        /// <summary>
        /// Number of stack frames to skip upwards to find the caller.
        /// This is set to 2 in order to find the method in which 
        /// the <c>OperationalEvent</c> was created.
        /// </summary>
        private const int StackFrameLevel = 2;
        #endregion

        #region Constructors

        /// <summary>
        /// Static constructor which initialises the Hashtable
        /// of OperationalEventFilter objects corresponding to
        /// each TDTraceLevelOverride value
        /// </summary>
        static OperationalEvent()
        {
            SJPTraceLevelOverride[] allValues = (SJPTraceLevelOverride[])Enum.GetValues(typeof(SJPTraceLevelOverride));
            filters = new Dictionary<SJPTraceLevelOverride, IEventFilter>(allValues.Length);
            foreach (SJPTraceLevelOverride curr in allValues)
                filters.Add(curr, new OperationalEventFilter(curr));
        }

        /// <summary>
        /// Constructor based on category, level and message
        /// </summary>
        /// <param name="category">Category of event.</param>
        /// <param name="level">Level of event.</param>
        /// <param name="message">Message associated with event.</param>
        public OperationalEvent(SJPEventCategory category,
                                SJPTraceLevel level,
                                string message)
            : base()
        {
            AssignProperties(category, level, message, null, SessionIdUnassigned, SJPTraceLevelOverride.None);
            SetContextProperties();
        }

        /// <summary>
        /// Constructor based on category, level and message
        /// </summary>
        /// <param name="category">Category of event.</param>
        /// <param name="level">Level of event.</param>
        /// <param name="message">Message associated with event.</param>
        /// <param name="overrideLevel">Override level to use</param>
        public OperationalEvent(SJPEventCategory category,
            SJPTraceLevel level,
            string message,
            SJPTraceLevelOverride overrideLevel)
            : base()
        {
            AssignProperties(category, level, message, null, SessionIdUnassigned, overrideLevel);
            SetContextProperties();
        }

        /// <summary>
        /// Constructor based on category, level, message and target
        /// </summary>
        /// <param name="category">Category of event.</param>
        /// <param name="level">Level of event.</param>
        /// <param name="message">Message associated with event.</param>
        /// <param name="target">Target object associated with event.</param>
        public OperationalEvent(SJPEventCategory category,
                                SJPTraceLevel level,
                                string message,
                                object target)
            : base()
        {
            AssignProperties(category, level, message, target, SessionIdUnassigned, SJPTraceLevelOverride.None);
            SetContextProperties();
        }

        /// <summary>
        /// Constructor based on category, level, message and target
        /// </summary>
        /// <param name="category">Category of event.</param>
        /// <param name="level">Level of event.</param>
        /// <param name="message">Message associated with event.</param>
        /// <param name="target">Target object associated with event.</param>
        /// <param name="overrideLevel">Override level to use</param>
        public OperationalEvent(SJPEventCategory category,
            SJPTraceLevel level,
            string message,
            object target,
            SJPTraceLevelOverride overrideLevel)
            : base()
        {
            AssignProperties(category, level, message, target, SessionIdUnassigned, overrideLevel);
            SetContextProperties();
        }

        /// <summary>
        /// Constructor based on category, level, message and session id
        /// </summary>
        /// <param name="category">Category of event.</param>
        /// <param name="level">Level of event.</param>
        /// <param name="message">Message associated with event.</param>
        /// <param name="sessionId">Identifies the ASP.NET session associated with event.</param>
        public OperationalEvent(SJPEventCategory category,
                                string sessionId,
                                SJPTraceLevel level,
                                string message)
            : base()
        {
            AssignProperties(category, level, message, null, sessionId, SJPTraceLevelOverride.None);
            SetContextProperties();
        }

        /// <summary>
        /// Constructor based on category, level, message and session id
        /// </summary>
        /// <param name="category">Category of event.</param>
        /// <param name="level">Level of event.</param>
        /// <param name="message">Message associated with event.</param>
        /// <param name="sessionId">Identifies the ASP.NET session associated with event.</param>
        /// <param name="overrideLevel">Override level to use</param>
        public OperationalEvent(SJPEventCategory category,
            string sessionId,
            SJPTraceLevel level,
            string message,
            SJPTraceLevelOverride overrideLevel)
            : base()
        {
            AssignProperties(category, level, message, null, sessionId, overrideLevel);
            SetContextProperties();
        }

        /// <summary>
        /// Constructor based on category, level, message, target and session identifier
        /// </summary>
        /// <param name="category">Category of event.</param>
        /// <param name="level">Level of event.</param>
        /// <param name="message">Message associated with event.</param>
        /// <param name="target">Target object associated with event.</param>
        /// <param name="sessionId">Identifies the ASP.NET session associated with event.</param>
        public OperationalEvent(SJPEventCategory category,
                                SJPTraceLevel level,
                                string message,
                                object target,
                                string sessionId)
            : base()
        {
            AssignProperties(category, level, message, target, sessionId, SJPTraceLevelOverride.None);
            SetContextProperties();
        }

        /// <summary>
        /// Constructor based on category, level, message, target and session identifier
        /// </summary>
        /// <param name="category">Category of event.</param>
        /// <param name="level">Level of event.</param>
        /// <param name="message">Message associated with event.</param>
        /// <param name="target">Target object associated with event.</param>
        /// <param name="sessionId">Identifies the ASP.NET session associated with event.</param>
        /// <param name="overrideLevel">Override level to use</param>
        public OperationalEvent(SJPEventCategory category,
            SJPTraceLevel level,
            string message,
            object target,
            string sessionId,
            SJPTraceLevelOverride overrideLevel)
            : base()
        {
            AssignProperties(category, level, message, target, sessionId, overrideLevel);
            SetContextProperties();
        }

        #endregion
       
        #region Public Properties

        /// <summary>
        /// Gets session id.
        /// </summary>
        public string SessionId
        {
            get { return sessionId; }
        }

        /// <summary>
        /// Gets category.
        /// </summary>
        public SJPEventCategory Category
        {
            get { return category; }
        }

        /// <summary>
        /// Gets level.
        /// </summary>
        public SJPTraceLevel Level
        {
            get { return level; }
        }

        /// <summary>
        /// Gets override level
        /// </summary>
        public SJPTraceLevelOverride OverrideLevel
        {
            get { return overrideLevel; }
        }

        /// <summary>
        /// Gets message.
        /// </summary>
        public string Message
        {
            get { return message; }
        }

        /// <summary>
        /// Gets target.
        /// </summary>
        public object Target
        {
            get { return target; }
        }

        /// <summary>
        /// Gets type name.
        /// </summary>
        public string TypeName
        {
            get { return typeName; }
        }

        /// <summary>
        /// Gets method name.
        /// </summary>
        public string MethodName
        {
            get { return methodName; }
        }

        /// <summary>
        /// Gets assembly name.
        /// </summary>
        public string AssemblyName
        {
            get { return assemblyName; }
        }

        /// <summary>
        /// Gets machine name.
        /// </summary>
        public string MachineName
        {
            get { return machineName; }
        }


        /// <summary>
        /// Gets file formatter.
        /// </summary>
        override public IEventFormatter FileFormatter
        {
            get { return OperationalEventFileFormatter.Instance; }
        }

        /// <summary>
        /// Gets email formatter.
        /// </summary>
        override public IEventFormatter EmailFormatter
        {
            get { return OperationalEventEmailFormatter.Instance; }
        }

        /// <summary>
        /// Gets event log formatter.
        /// </summary>
        override public IEventFormatter EventLogFormatter
        {
            get { return OperationalEventEventLogFormatter.Instance; }
        }

        /// <summary>
        /// Gets console formatter.
        /// </summary>
        override public IEventFormatter ConsoleFormatter
        {
            get { return OperationalEventConsoleFormatter.Instance; }
        }

        /// <summary>
        /// Read only property returting the filter asscociated with the override level
        /// </summary>
        public override IEventFilter Filter
        {
            get {
                if (filters == null || filters.Count == 0)
                {
                    InitialiseFilters();
                }

                return filters[overrideLevel];
                
            }
        }

       
        #endregion

        #region Public Methods

        /// <summary>
        /// Method to update the context properties
        /// </summary>
        public void UpdateContextProperties(string methodName, string typeName, string assemblyName, string machineName)
        {
            this.methodName = methodName;
            this.typeName = typeName;
            this.assemblyName = assemblyName;
            this.machineName = machineName;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Assigns properties of operational event.
        /// </summary>
        /// <remarks>
        /// The alternative to using this method would be to call a single constructor
        /// from each of the constructors. However, this means that the context properties
        /// do not get assigned the correct value, notably the Method property, since
        /// an extra call has been made.
        /// </remarks>
        private void AssignProperties(SJPEventCategory category,
            SJPTraceLevel level,
            string message,
            object target,
            string sessionId,
            SJPTraceLevelOverride overrideLevel)
        {
            if ((level == SJPTraceLevel.Off) || (level == SJPTraceLevel.Undefined))
                throw new SJPException(Messages.OperationalEventLevelInvalid, false, SJPExceptionIdentifier.ELSCustomOperationalConstructor);

            this.category = category;
            this.level = level;
            this.message = message;
            this.target = target;
            this.sessionId = sessionId;
            this.overrideLevel = overrideLevel;

            // used to determine publisher at publish time
            this.ClassName = ClassNamePrefix + level.ToString();
        }

        /// <summary>
        /// Sets the operational event properties infered from stack frame
        /// </summary>
        private void SetContextProperties()
        {
            try
            {
                StackFrame stackFrame = new StackFrame(StackFrameLevel, false);
                MethodBase methodBase = stackFrame.GetMethod();
                this.methodName = methodBase.Name;
                this.typeName = methodBase.DeclaringType.Name;

                string assemblyQualifiedName =
                    methodBase.DeclaringType.AssemblyQualifiedName;
                assemblyName = assemblyQualifiedName.Split(',')[1].Trim();

                this.machineName = System.Environment.MachineName;
            }
            catch
            {
                this.methodName = ContextUnassigned;
                this.typeName = ContextUnassigned;
                this.assemblyName = ContextUnassigned;
                this.machineName = ContextUnassigned;
            }
        }

        
        /// <summary>
        /// Initialises all the operational event filters
        /// </summary>
        private void InitialiseFilters()
        {
            SJPTraceLevelOverride[] allValues = (SJPTraceLevelOverride[])Enum.GetValues(typeof(SJPTraceLevelOverride));

            foreach (SJPTraceLevelOverride curr in allValues)
                filters.Add(curr, new OperationalEventFilter(curr));
        }
        #endregion
    }
}
