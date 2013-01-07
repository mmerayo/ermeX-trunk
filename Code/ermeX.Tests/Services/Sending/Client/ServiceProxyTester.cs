// /*---------------------------------------------------------------------------------------*/
// If you viewing this code.....
// The current code is under construction.
// The reason you see this text is that lot of refactors/improvements have been identified and they will be implemented over the next iterations versions. 
// This is not a final product yet.
// /*---------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using NUnit.Framework;
using ermeX.Common.Caching;
using ermeX.ConfigurationManagement.Settings;
using ermeX.ConfigurationManagement.Settings.Data;
using ermeX.ConfigurationManagement.Settings.Data.DbEngines;
using ermeX.ConfigurationManagement.Settings.Data.Schemas;
using ermeX.DAL.DataAccess.DataSources;
using ermeX.DAL.DataAccess.Helpers;

using ermeX.Entities.Entities;
using ermeX.LayerMessages;
using ermeX.Tests.Common.DataAccess;
using ermeX.Tests.Common.SettingsProviders;
using ermeX.Tests.Services.Mock;

using ermeX.Transport.Interfaces.Messages;
using ermeX.Transport.Publish;

namespace ermeX.Tests.Services.Sending.Client
{
    //[TestFixture]
    internal sealed class ServiceProxyTester :DataAccessTestBase
    {
        private ICacheProvider GetCacheProvider()
        {
            return new MemoryCacheStore(300);//TODO: REMOVE AS THE CACHEING WILL BE RE-ADDED IN THE FUTURE
        }

        private ServiceProxy GetTarget(DbEngineType engineType, out MockConnectivityProvider provider,
                                       bool withEndPoints = true)
        {
            provider = GetDummyConnectivityProvider(withEndPoints);
            IDalSettings dataAccessSettingsSource = GetDataAccessSettingsSource(engineType);
            var dataAccessExecutor = new DataAccessExecutor(dataAccessSettingsSource);
            var connectivityDetailsDataSource = new ConnectivityDetailsDataSource(dataAccessSettingsSource, LocalComponentId,dataAccessExecutor);
            return
                new ServiceProxy(
                    connectivityDetailsDataSource,
                    GetCacheProvider(), provider, TestSettingsProvider.GetClientConfigurationSettingsSource());
        }

        private ServiceProxy GetTarget(DbEngineType engineType, bool withEndPoints = true)
        {
            MockConnectivityProvider none;
            return GetTarget(engineType, out none, withEndPoints);
        }

        private MockConnectivityProvider GetDummyConnectivityProvider(bool withEndPoints)
        {
            return new MockConnectivityProvider(withEndPoints);
        }


        private IDalSettings GetDataAccessSettingsSource(DbEngineType engineType)
        {
            DataAccessTestHelper dataAccessTestHelper = GetDataHelper(engineType);
            return dataAccessTestHelper.DataAccessSettings;
        }


        private TransportMessage GetTransportMessage<TData>(TData data)
        {
            var bizMessage = new BizMessage(data);
            var busMessage = new BusMessage(LocalComponentId, bizMessage);
            var transportMessage = new TransportMessage(RemoteComponentId, busMessage);
            return transportMessage;
        }


        [Test, TestCaseSource(typeof(TestCaseSources), "InMemoryDb")]
        public void CanInvokeSend(DbEngineType engineType)
        {
            using (ServiceProxy target = GetTarget(engineType))
            {
                const int nothing = 100;

                var transportMessage = GetTransportMessage(nothing);
              
                ServiceResult actual = target.Send(transportMessage);
                Assert.IsTrue(actual.Ok);
            }
        }

      

        [Test, TestCaseSource(typeof(TestCaseSources), "InMemoryDb")]
        public void WhenAllEndPointsFailed_CanBeReInvoked(DbEngineType engineType)
        {
            MockConnectivityProvider connMgr;
            using (ServiceProxy target = GetTarget(engineType, out connMgr))
            {
                const int nothing = 150;
                var expected = GetTransportMessage(nothing);
                connMgr.ClientProxiesForComponent.Clear();
                const int itemsNum = 10;
                for (int i = 0; i < itemsNum; i++)
                {
                    connMgr.ClientProxiesForComponent.Add(new MockEndPoint() {Fails = true});
                }

                ServiceResult actual = target.Send(expected);

                Assert.IsFalse(actual.Ok);

                ((MockEndPoint) connMgr.ClientProxiesForComponent[itemsNum - 1]).Fails = false;

                actual = target.Send(expected);

                Assert.IsTrue(actual.Ok);
            }
        }

        [Test, TestCaseSource(typeof(TestCaseSources), "InMemoryDb")]
        public void WhenAllEndPointsFailed__ServiceResultError(DbEngineType engineType)
        {
            MockConnectivityProvider connMgr;
            using (ServiceProxy target = GetTarget(engineType, out connMgr))
            {
                const int nothing = 2100;
                var expected = GetTransportMessage(nothing);
                connMgr.ClientProxiesForComponent.Clear();
                const int itemsNum = 10;
                for (int i = 0; i < itemsNum; i++)
                {
                    connMgr.ClientProxiesForComponent.Add(new MockEndPoint() {Fails = true});
                }

                ServiceResult actual = target.Send(expected);

                Assert.IsFalse(actual.Ok);
            }
        }

        [Test, TestCaseSource(typeof(TestCaseSources), "InMemoryDb")]
        public void WhenEndpointFailedTry_WithAllTheRestOfEndpoints(DbEngineType engineType)
        {
            MockConnectivityProvider connMgr;
            using (ServiceProxy target = GetTarget(engineType, out connMgr))
            {
                const int nothing = 155;
                var expected = GetTransportMessage(nothing);
                connMgr.ClientProxiesForComponent.Clear();
                const int itemsNum = 10;
                for (int i = 0; i < itemsNum; i++)
                {
                    connMgr.ClientProxiesForComponent.Add(new MockEndPoint() {Fails = true});
                }

                ((MockEndPoint) connMgr.ClientProxiesForComponent[itemsNum - 1]).Fails = false;

                ServiceResult actual = target.Send(expected);

                Assert.IsTrue(actual.Ok);
            }
        }

        [Test, TestCaseSource(typeof(TestCaseSources), "InMemoryDb")]
        public void WhenEndpointRaisesExceptionTry_WithAllTheRestOfEndpoints(DbEngineType engineType)
        {
            MockConnectivityProvider connMgr;
            using (ServiceProxy target = GetTarget(engineType, out connMgr))
            {
                const int nothing = 10055;
                var expected = GetTransportMessage(nothing);
                ((MockEndPoint) connMgr.ClientProxiesForComponent[0]).RaisesException = true;
                ServiceResult actual = target.Send(expected);
                Assert.IsTrue(actual.Ok);
            }
        }

        [Test, TestCaseSource(typeof(TestCaseSources), "InMemoryDb")]
        public void WhenThereAreNotEndpointsItReturns_ServiceResultError(DbEngineType engineType)
        {
            using (ServiceProxy target = GetTarget(engineType, false))
            {
                const int nothing = 5100;
                var expected = GetTransportMessage(nothing);
                ServiceResult actual = target.Send(expected);
                Assert.IsFalse(actual.Ok);
            }
        }
    }
}