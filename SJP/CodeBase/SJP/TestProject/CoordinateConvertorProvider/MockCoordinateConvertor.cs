// *********************************************** 
// NAME             : MockCoordinateConvertor.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 Apr 2011
// DESCRIPTION  	: MockCoordinateConvertor class
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.UserPortal.CoordinateConvertorProvider;
using SJP.Common.LocationService;

namespace SJP.TestProject
{
    /// <summary>
    /// MockCoordinateConvertor class
    /// </summary>
    public class MockCoordinateConvertor : ICoordinateConvertor
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MockCoordinateConvertor()
        {
        }

        #endregion

        #region ICoordinateConvertor methods

        public LatitudeLongitude GetLatitudeLongitude(OSGridReference osgr)
        {
            return new LatitudeLongitude();
        }

        public LatitudeLongitude[] GetLatitudeLongitude(OSGridReference[] osgrs)
        {
            return new LatitudeLongitude[0];
        }

        public OSGridReference GetOSGridReference(LatitudeLongitude latlong)
        {
            return new OSGridReference();
        }

        public OSGridReference[] GetOSGridReference(LatitudeLongitude[] latlongs)
        {
            return new OSGridReference[0];
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // NOTE: Leave out the finalizer altogether if this class doesn't 
        // own unmanaged resources itself, but leave the other methods
        // exactly as they are. 
        ~MockCoordinateConvertor()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
               
            }
        }

        #endregion
    }
}
