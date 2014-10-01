// *********************************************** 
// NAME             : PropertyServiceFactory.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 16 Feb 2011
// DESCRIPTION  	: Factory class for the property service
// ************************************************


using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Timers;
using SJP.Common.ServiceDiscovery;


namespace SJP.Common.PropertyManager
{
    /// <summary>
	/// Factory class for the Property Service. Based on IServiceFactory
	/// </summary>
    public class PropertyServiceFactory : IServiceFactory, IDisposable
    {
        #region Private Fields

        private IPropertyProvider current = null;
		private Timer myTimer;

        #endregion
    
        #region Constructors
        /// <summary>
        /// PropetyServiceFactory constructor
        /// Initialises file/database property provider using reflection and calls the load
        /// method to load the properties in memory
        /// </summary>
        public PropertyServiceFactory()
		{		
			string strAssembly = ConfigurationManager.AppSettings["propertyservice.providerassembly"];
			string strClass = ConfigurationManager.AppSettings["propertyservice.providerclass"];

			Assembly aAssembly;
			Properties newInstance;
			try
			
			{
				// Instantiate it according to properties
				aAssembly = Assembly.Load(strAssembly);
				
			}
			catch( ArgumentException )
			{
				throw new SJPException(
					"Exception loading the assembly. Argument is null", 
					false, 
					SJPExceptionIdentifier.PSInvalidAssembly);
			}
			catch ( FileNotFoundException )
			{
				throw new SJPException(
					"Exception loading the assembly. File not Found",
					false, 
					SJPExceptionIdentifier.PSInvalidAssembly);
			}

			try
			{

				//Type[] temp = aAssembly.GetTypes(); //for Test
				Type tProvider = aAssembly.GetType(strClass);

				newInstance  = 
					(Properties)Activator.CreateInstance (tProvider);

			}
			catch ( ReflectionTypeLoadException)
			{
				throw new SJPException(
					"Exception Getting the type. Type not found in assembly", 
					false, 
					SJPExceptionIdentifier.PSInvalidAssemblyType);
			}
			catch (ArgumentException)
			{
				throw new SJPException(
					"Exception Getting the type. Type is invalid/ null", 
					false, 
					SJPExceptionIdentifier.PSInvalidAssemblyType);
			}
			
			
			// Call Load in new instantiated object
			IPropertyProvider newCurrent = newInstance.Load();

			// if the object has been successfully loaded, then 
			if (newCurrent!= null)
			{
				// Register as current
				Register(newCurrent);

				// Timer Init
				myTimer = new Timer();
				myTimer.Elapsed += new ElapsedEventHandler(TimerElapsed);

				// retrieve refresh rate
				RetrieveRefreshRate();

			}
		}
        #endregion

        #region Public Methods
        /// <summary>
        /// IServiceFactory interface method providing access to internal property provider object
        /// </summary>
        /// <returns></returns>
        public object Get()
        {
            return current;
        }

        /// <summary>
        /// Registers the property data store
        /// </summary>
        /// <param name="provider">Property store provider object</param>
        public void Register(IPropertyProvider provider)
        {
            current = provider;
        }

        /// <summary>
        /// Overloaded dispose method to clean up resources
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Code to dispose the managed resources of the class
                myTimer.Dispose();
            }
            // Code to dispose the un-managed resources of the class

        }

        /// <summary>
        /// Dispose method to clean up resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Retrieves refresh rate for property service to poll the property data store,
        /// sets the polling timer properties and starts the timer
        /// </summary>
        private void RetrieveRefreshRate()
		{
			int refreshRate = Convert.ToInt32
				(current["propertyservice.refreshrate"], CultureInfo.CurrentCulture.NumberFormat);

			Properties currentProperties = (Properties)current;

			myTimer.Enabled = false;
			myTimer.Interval = (double)refreshRate;
			myTimer.AutoReset = true;	
			
			myTimer.Start();

        }

        #endregion

        #region Event Handlers
        /// <summary>
        /// Handler for the property data store polling timer elapsed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">ElapsedEventArgs object</param>
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Load 
            IPropertyProvider returnedInstance = ((Properties)current).Load();



            // if object returned not null,
            if (returnedInstance != null)
            {
                Properties oldCurrent = (Properties)current;

                //register new instance as current
                Register(returnedInstance);

                // retrieve refresh rate
                RetrieveRefreshRate();

                // Supersede
                oldCurrent.Supersede();
            }



        }
        #endregion


    }
}
