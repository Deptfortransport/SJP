// *********************************************** 
// NAME             : PublisherGroup.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Abstract factory class for creating publishing groups
// ************************************************


using System.Collections.Generic;
using SJP.Common.PropertyManager;

namespace SJP.Common.EventLogging
{
    /// <summary>
    /// Abstract factory class for creating publishing groups.
    /// </summary>
    abstract class PublisherGroup
    {
        #region Protected Fields
        protected List<IEventPublisher> publishers;
        protected IPropertyProvider properties;
        protected LoggingPropertyValidator validator;
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor based on a property provider.</c>
        /// </summary>
        /// <param name="properties">Properties to use to create publisher group.</param>
        public PublisherGroup(IPropertyProvider properties)
        {
            this.properties = properties;
            this.validator = new LoggingPropertyValidator(properties);
            publishers = new List<IEventPublisher>();
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the array list of publishers.
        /// </summary>
        public List<IEventPublisher> Publishers
        {
            get { return publishers; }
        }

        #endregion

        #region Abstract Properties

        /// <summary>
        /// Creates the publisher group.
        /// </summary>
        /// <param name="errors">Holds errors occurring during publisher creation.</param>
        abstract public void CreatePublishers(List<string> errors);

        #endregion

    }
}
