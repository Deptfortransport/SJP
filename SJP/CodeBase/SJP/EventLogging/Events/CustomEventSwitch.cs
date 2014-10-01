// *********************************************** 
// NAME             : CustomEventSwitch.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 11 Feb 2011
// DESCRIPTION  	: Static class used to determine
//                    the logging level of custom events.
// ************************************************
                 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.Common.EventLogging
{
    /// <summary>
    /// Static class used to determine the level of <c>CustomEvent</c>s.
    /// </summary>
    public class CustomEventSwitch
    {
        #region Private Fields
        /// <summary>
        /// Contains a level setting for each configured <c>CustomEvent</c>.
        /// </summary>
        private static Dictionary<string, CustomEventLevel> individualLevels;

        /// <summary>
        /// Constains the level setting across all CustomEvents,
        /// indexed by <c>CustomEvent</c> class names.
        /// </summary>
        private static CustomEventLevel globalLevel;

        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor.
        /// </summary>
        static CustomEventSwitch()
        {
            // Set global level to undefined until properties service notifies 
            // this class of the value read from the configuration settings.
            globalLevel = CustomEventLevel.Undefined;

            // Note that until the properties service notifies this class 
            // of the real values, this hash table will not contain any keys.
            individualLevels = new Dictionary<string,CustomEventLevel>();

            // Register listener for changes in levels including the actual initialisation of levels
            SJPTraceListener.CustomLevelChange += new CustomLevelChangeEventHandler(CustomLevelChangeEventHandler);
            SJPTraceListener.CustomLevelsChange += new CustomLevelsChangeEventHandler(CustomLevelsChangeEventHandler);
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the global on event level indicator. <c>true</c> if event level across all CustomEvents is 'On'.
        /// </summary>
        /// <returns><c>true</c> if global level is 'On', otherwise <c>false</c>.</returns> 
        public static bool GlobalOn
        {
            get { return CheckLevel(); }
        }
        #endregion


        #region Event Handlers

        /// <summary>
        /// Event handler for CustomLevelChange event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="customLevelEventArgs"></param>
        private static void CustomLevelChangeEventHandler(object sender, CustomLevelEventArgs customLevelEventArgs)
        {
            globalLevel = customLevelEventArgs.CustomLevel;
        }

        /// <summary>
        /// Event handler for CustomLevelsChange event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="customLevelsEventArgs"></param>
        private static void CustomLevelsChangeEventHandler(object sender, CustomLevelsEventArgs customLevelsEventArgs)
        {

            individualLevels = customLevelsEventArgs.CustomLevels;

        }

        #endregion


        #region Public Methods

        /// <summary>
        /// Indicates whether a <c>CustomEvent</c>s level is 'On'.
        /// </summary>
        /// <param name="eventClassName">Class name of <c>CustomEvent</c> not including path.</param>
        /// <returns><c>true</c> if <c>CustomEvent</c> has level of 'On', otherwise <c>false</c>.</returns> 
        public static bool On(string eventClassName)
        {
            return CheckLevel(CustomEventLevel.On, eventClassName);
        }

        /// <summary>
        /// Indicates whether a <c>CustomEvent</c>s level is 'Off'.
        /// </summary>
        /// <param name="eventClassName">Class name of <c>CustomEvent</c> not including path.</param>
        /// <returns><c>true</c> if <c>CustomEvent</c> has level of 'Off', otherwise <c>false</c>.</returns> 
        public static bool Off(string eventClassName)
        {
            return CheckLevel(CustomEventLevel.Off, eventClassName);
        }

        #endregion


        #region Private Methods

        /// <summary>
        /// Returns the global level across all Custom Events.
        /// </summary>
        /// <returns><c>true</c> is 'on' otherwise <c>false</c></returns>
        /// <exception cref="SJP.Common.SJPException">Level is undefined.</exception>"
        private static bool CheckLevel()
        {
            if (globalLevel == CustomEventLevel.Undefined)
            {
                // NB this test is performed to catch cases where level is changed at runtime to an undefined value. 
                throw new SJPException(Messages.CustomEventSwitchNotInitialised, false, SJPExceptionIdentifier.ELSGlobalTraceLevelUndefined);
            }
            else
            {
                return (globalLevel == CustomEventLevel.On);
            }
        }

        /// <summary>
        /// Indicates the value of an individual <c>CustomEvent</c> level.
        /// This method takes into account the global trace level for custom events
        /// which overrides individual levels if it is set to off.
        /// </summary>
        /// <param name="level">The level to which the check is being made.</param>
        /// <param name="eventClassName">The class name (not including path) of the <c>CustomEvent</c></param>
        /// <returns><c>true</c> if the level passed in <c>level</c> matches 
        /// the current level for <c>CustomEvent</c> with class name <c>eventClassName</c>.</returns>
        private static bool CheckLevel(CustomEventLevel level, string eventClassName)
        {
            if (CheckLevel()) // check global level
            {
                try
                {
                    if (individualLevels.ContainsKey(eventClassName))
                        return (individualLevels[eventClassName] == level);
                    else
                    {
                        throw new SJPException(String.Format(Messages.UnknownCustomEventClassName, eventClassName), false, SJPExceptionIdentifier.ELSSwitchOnUnknownEvent);
                    }
                }
                catch (Exception exception)  // catch undocumented exceptions
                {
                    throw new SJPException(String.Format(Messages.UnknownCustomEventClassName, eventClassName), exception, false, SJPExceptionIdentifier.ELSSwitchOnUnknownEvent);
                }
            }
            else
            {
                return false; // global level set to off
            }
        }

        #endregion

        

    }
}
