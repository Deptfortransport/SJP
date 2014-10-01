using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.UserPortal.SessionManager;
using System.Collections;
using SJP.UserPortal.StateServer;
using SJP.Common.ServiceDiscovery;
using SJP.Common;
using SJP.Common.EventLogging;
using Logger = System.Diagnostics.Trace;

namespace SJP.TestProject
{
    public class MockSJPSessionManager: ISJPSessionManager, IDisposable
    {
        #region Private variables

        /// <summary>
        /// The session factory used to create sessions
        /// </summary>
        private ISJPSessionFactory sessionFactory;

        /// <summary>
        /// Used to keep track when this session manager can safely be unloaded from the factory
        /// </summary>
        private int references;

        /// <summary>
        /// The ASP session wrapper "decorator"
        /// </summary>
        private MockSJPSession sjpSession = new MockSJPSession();

        /// <summary>
        /// Handles the deferable objects
        /// </summary>
        private Hashtable deferableObjects = new Hashtable();

        /// <summary>
        /// State Server used to defer data objects into storage and maintain locks on the data
        /// through the duration of a page life cycle
        /// </summary>
        private ISJPStateServer sjpStateServer = new SJPStateServer();

        /// <summary>
        /// Track whether Dispose has been called.
        /// </summary>
        private bool disposed = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for the MockSJPSessionManager that is used through the ServiceDiscovery
        /// </summary>
        /// <param name="sessFactory">The session factory to be used</param>
        public MockSJPSessionManager(ISJPSessionFactory sessFactory)
        {
            sessionFactory = sessFactory;
        }

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Read only property that gets the user's session manager instance. 
        /// This is a convenience method for getting it from the ServiceDiscovery.
        /// </summary>
        public static ISJPSessionManager Current
        {
            get
            {
                return SJPServiceDiscovery.Current.Get<ISJPSessionManager>(ServiceDiscoveryKey.SessionManager);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Read only property that gets the Session data for the current session manager.
        /// </summary>
        public ISJPSession Session
        {
            get
            {
                return sjpSession;
            }
        }

        #region Page state

        /// <summary>
        /// Get/Set the state of the input page
        /// </summary>
        public InputPageState PageState
        {
            get
            {
                InputPageState pageState = (InputPageState)GetData(SessionManagerKey.KeyPageState);
                if (pageState == null)
                {
                    pageState = PageState = new InputPageState();
                    
                }
                return pageState;
            }
            set
            {
                SetData(SessionManagerKey.KeyPageState, value);
            }
        }

        #endregion

        #region Journey View State

        /// <summary>
        /// Get/Set the object to enable sharing data between multiple outward and/or return journeys
        /// <remarks>
        /// The JourneyViewState object is intended for single page life cycle that's from page load to page unload.
        /// The object should never be persisted in deffered session
        /// </remarks>
        /// </summary>
        public JourneyViewState JourneyState
        {
            get
            {
                JourneyViewState journeyViewState = (JourneyViewState)GetData(SessionManagerKey.KeyJourneyViewState);
                if (journeyViewState == null)
                {
                    journeyViewState = JourneyState = new JourneyViewState();

                }
                return journeyViewState;
            }
            set
            {
                SetData(SessionManagerKey.KeyJourneyViewState, value);
            }
        }

        #endregion

        #region Journey managers

        /// <summary>
        /// Read/Write. SJPRequestManager containing journey requests for the session
        /// </summary>
        public SJPRequestManager RequestManager
        {
            get
            {
                SJPRequestManager requestManager = (SJPRequestManager)GetData(SessionManagerKey.KeyRequestManager);
                if (requestManager == null)
                {
                    requestManager = RequestManager = new SJPRequestManager();
                }
                return requestManager;
            }
            set
            {
                SetData(SessionManagerKey.KeyRequestManager, value);
            }
        }

        /// <summary>
        /// Read/Write. SJPResultManager containing journey results for the session
        /// </summary>
        /// <remarks>UI elements of the solution MUST NOT use the setter on this property (or change
        /// any objects within the SJPResultManager object). 
        /// Only the Journey Planner elements should update, this is to prevent losing 
        /// journey results in possible race conditions.</remarks>
        public SJPResultManager ResultManager
        {
            get
            {
                SJPResultManager resultManager = (SJPResultManager)GetData(SessionManagerKey.KeyResultManager);
                if (resultManager == null)
                {
                    resultManager = ResultManager = new SJPResultManager();
                }
                return resultManager;
            }
            set
            {
                SetData(SessionManagerKey.KeyResultManager, value);
            }
        }

        #endregion

        #region Stop Event managers

        /// <summary>
        /// Read/Write. SJPStopEventRequestManager containing stop event requests for the session
        /// </summary>
        public SJPStopEventRequestManager StopEventRequestManager
        {
            get
            {
                SJPStopEventRequestManager requestManager = (SJPStopEventRequestManager)GetData(SessionManagerKey.KeyStopEventRequestManager);
                if (requestManager == null)
                {
                    requestManager = StopEventRequestManager = new SJPStopEventRequestManager();
                }
                return requestManager;
            }
            set
            {
                SetData(SessionManagerKey.KeyStopEventRequestManager, value);
            }
        }

        /// <summary>
        /// Read/Write. SJPStopEventResultManager containing stop event journey results for the session
        /// </summary>
        /// <remarks>UI elements of the solution MUST NOT use the setter on this property (or change
        /// any objects within the SJPStopEventResultManager object). 
        /// Only the Journey Planner elements should update, this is to prevent losing 
        /// results in possible race conditions.</remarks>
        public SJPStopEventResultManager StopEventResultManager
        {
            get
            {
                SJPStopEventResultManager resultManager = (SJPStopEventResultManager)GetData(SessionManagerKey.KeyStopEventResultManager);
                if (resultManager == null)
                {
                    resultManager = StopEventResultManager = new SJPStopEventResultManager();
                }
                return resultManager;
            }
            set
            {
                SetData(SessionManagerKey.KeyStopEventResultManager, value);
            }
        }

        #endregion

        #region Travel News state

        /// <summary>
        /// Get/Set the state of the travel news page
        /// </summary>
        public TravelNewsPageState TravelNewsPageState
        {
            get
            {
                TravelNewsPageState travelNewsPageState = (TravelNewsPageState)GetData(SessionManagerKey.KeyTravelNewsPageState);
                if (travelNewsPageState == null)
                {
                    travelNewsPageState = TravelNewsPageState = new TravelNewsPageState();

                }
                return travelNewsPageState;
            }
            set
            {
                SetData(SessionManagerKey.KeyTravelNewsPageState, value);
            }
        }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Saves all data to the deferred storage.
        /// Note: This will only save data according to the rules (ISJPSessionAware).
        /// </summary>
        public void SaveData()
        {
            object ob = null;

            foreach (IKey key in deferableObjects.Keys)
            {
                try
                {
                    // Read object
                    ob = deferableObjects[key];

                    if (ob != null)
                    {
                        if (ob is ISJPSessionAware)
                        {
                            // Only save if object has changed
                            if (((ISJPSessionAware)ob).IsDirty)
                            {
                                // Reset dirty flag before saving to prevent further unnecessary saves
                                ((ISJPSessionAware)ob).IsDirty = false;

                                sjpStateServer.Save(this.Session.SessionID, key.ID, ob);
                            }
                        }
                    }
                    else
                    {
                        // Object has been set to null, delete it from deferred storage.
                        // Ensures no "old" object data is retained
                        sjpStateServer.Delete(this.Session.SessionID, key.ID);
                    }
                }
                catch (SJPException sjpEx)
                {
                    if (!sjpEx.Logged)
                    {
                        Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, sjpEx.Message));
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets data from the deferred storage and caches it.
        /// Further calls to this method will fetch it from the cache rather than
        /// the database.
        /// </summary>
        /// <param name="key">The key to fetch</param>
        /// <returns>The correct object or null if it is missing</returns>
        private object GetData(IKey key)
        {
            object val = deferableObjects[key];

            if (val == null)
            {
                try
                {
                    val = sjpStateServer.Read(this.Session.SessionID, key.ID);
                }
                catch (SJPException sjpEx)
                {
                    if (!sjpEx.Logged)
                    {
                        Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, sjpEx.Message));
                    }
                }

                if (val != null)
                    deferableObjects[key] = val;
            }

            return val;
        }

        /// <summary>
        /// Set correct data to be saved to deferred storage.
        /// This is set in the cache and not serialised until end of session or save-data is called.
        /// </summary>
        /// <param name="key">The key to use</param>
        /// <param name="ob">The object to store</param>
        private void SetData(IKey key, object ob)
        {
            if (ob == null)
            {
                deferableObjects[key] = null;
            }
            else
            {
                // Update flag indicating object has changed
                if (ob is ISJPSessionAware)
                    ((ISJPSessionAware)ob).IsDirty = true;

                deferableObjects[key] = ob;
            }
        }

        #endregion

        #region Lifecycle Event Methods

        /// <summary>
        /// OnLoad event executes the first time the SessionManager 
        /// is requested via the property 'Current'
        /// </summary>
        public void OnLoad()
        {
            this.references++;

            // Place locks on session data objects in the state server, these will be released when 
            // the object is saved/deleted or this session manager is removed from the session factory 
            // cache in the OnUnload event
            sjpStateServer.Lock(this.Session.SessionID, SessionManagerKey.AllSessionManagerKeys());
        }

        /// <summary>
        /// OnFormShift event executes when shift of a form has occurred
        /// but only after the new page's OnLoad event has executed.
        /// </summary>
        public void OnFormShift()
        {
            // Current implementation does not need to do anything here
        }

        /// <summary>
        /// OnPreUnload event executes when the page renders.
        /// </summary>
        public void OnPreUnload()
        {
            // Save data to deferred storage
            SaveData();
        }

        /// <summary>
        /// OnUnload is the last event to occur and any outside access 
        /// should be avoided at this point.
        /// </summary>
        public void OnUnload()
        {
            this.references--;
            if (references < 1)
            {
                // Safely dispose resources in the state server as no longer being used
                sjpStateServer.Dispose();

                sessionFactory.Remove();
            }
        }

        #endregion

        #region IDisposable methods

        ~MockSJPSessionManager()
        {
            //calls a protected method 
            //the false tells this method
            //not to bother with managed
            //resources
            this.Dispose(false);
        }

        public void Dispose()
        {
            //calls the same method
            //passed true to tell it to
            //clean up managed and unmanaged 
            this.Dispose(true);

            //as dispose has been correctly
            //called we don't need the 
            //'backup' finaliser
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            //check this hasn't been called already
            //remember that Dispose can be called again
            if (!disposed)
            {
                //this is passed true in the regular Dispose
                if (disposing)
                {
                    // Dispose managed resources here.

                    #region Dispose managed resources

                    sjpStateServer.Dispose();

                    #endregion
                }

                //both regular Dispose and the finaliser
                //will hit this code
                // Dispose unmanaged resources here.
            }

            disposed = true;
        }

        #endregion
    }
}

