using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.ServiceDiscovery;

namespace SJP.TestProject.EventLogging.MockObjects
{
    class MockPropertiesGoodServiceFactory : IServiceFactory
    {
        #region Private Fields
        private MockPropertiesGood current;

        #endregion

        #region Constructors
        public MockPropertiesGoodServiceFactory()
        {
            current = new MockPropertiesGood();
        }

        public MockPropertiesGoodServiceFactory(MockPropertiesGood goodProps)
        {
            current = goodProps;
        }

        #endregion

        #region Public Methods
        public object Get()
        {
            return current;
            
        }

        #endregion

        internal void MockLevelChange1()
        {
            current.MockLevelChange1();
        }

        internal void MockLevelChange2()
        {
            current.MockLevelChange2();
        }

        internal void MockLevelChange3()
        {
            current.MockLevelChange3();
        }

        internal void MockInvalidPropertyChange()
        {
            current.MockInvalidPropertyChange();
        }
    }
}
