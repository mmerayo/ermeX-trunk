// /*---------------------------------------------------------------------------------------*/
// If you viewing this code.....
// The current code is under construction.
// The reason you see this text is that lot of refactors/improvements have been identified and they will be implemented over the next iterations versions. 
// This is not a final product yet.
// /*---------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using ermeX.Common;
using ermeX.ConfigurationManagement.Settings.Data.DbEngines;
using ermeX.Exceptions;
using ermeX.Tests.Acceptance.Dummy;
using ermeX.Tests.Common.Networking;
using ermeX.Tests.Common.SettingsProviders;


namespace ermeX.Tests.Acceptance
{
    [TestFixture]
    internal sealed class ServicesAcceptanceTester:AcceptanceTester
    {
        //TODO: implement async calls

        #region Local

        [Test,TestCaseSource(typeof(TestCaseSources), "InMemoryDb")]
        public void CanInvoke_LocalCustomService_EmptyMethod(DbEngineType engineType)
        {
            string dbConnString = TestSettingsProvider.GetConnString(engineType);

            const ushort c1Port = 10001;
            
            using (var component1 = TestComponent.GetComponent(true))
            {
                InitializeLonelyComponent(engineType, c1Port, component1, dbConnString);

                var finishedEvent = new AutoResetEvent(false);
                var registeredEvent = new AutoResetEvent(false);
                component1.RegisterService(1, registeredEvent, finishedEvent);
                registeredEvent.WaitOne(TimeSpan.FromSeconds(10));

                var target = component1.GetServiceProxy<ITestService1>();
                Assert.DoesNotThrow(target.EmptyMethod);
                finishedEvent.WaitOne(TimeSpan.FromSeconds(5));

                Assert.IsTrue(component1.Tracker.EmptyMethodCalled == 1);
                Assert.IsTrue(component1.Tracker.ParametersLastCall.Count == 0);
            }
        }

        [Test,TestCaseSource(typeof(TestCaseSources), "InMemoryDb")]
        public void CanInvoke_LocalCustomService_OneParamMethod(DbEngineType engineType)
        {
            string dbConnString = TestSettingsProvider.GetConnString(engineType);

            const ushort c1Port = 10001;
            
            using (var component1 = TestComponent.GetComponent(true))
            {
                InitializeLonelyComponent(engineType, c1Port, component1, dbConnString);

                var finishedEvent = new AutoResetEvent(false);
                var registeredEvent = new AutoResetEvent(false);
                component1.RegisterService(1, registeredEvent, finishedEvent);
                registeredEvent.WaitOne(TimeSpan.FromSeconds(10));

                var target = component1.GetServiceProxy<ITestService1>();
                var expected = new AcceptanceMessageType1();

                Assert.DoesNotThrow(() => target.EmptyMethodWithOneParameter(expected));
                finishedEvent.WaitOne(TimeSpan.FromSeconds(5));

                Assert.IsTrue(component1.Tracker.EmptyMethodWithOneParameterCalled == 1);
                Assert.IsTrue(component1.Tracker.ParametersLastCall.Count == 1);
                Assert.AreEqual(expected, component1.Tracker.ParametersLastCall[0]);
            }
        }

        [Test,TestCaseSource(typeof(TestCaseSources), "InMemoryDb")]
        public void CanInvoke_LocalCustomService_SeveralParamsMethod(DbEngineType engineType)
        {
            string dbConnString = TestSettingsProvider.GetConnString(engineType);

            const ushort c1Port = 10001;
            
            using (var component1 = TestComponent.GetComponent(true))
            {
                InitializeLonelyComponent(engineType, c1Port, component1, dbConnString);

                var finishedEvent = new AutoResetEvent(false);
                var registeredEvent = new AutoResetEvent(false);
                component1.RegisterService(1, registeredEvent, finishedEvent);
                registeredEvent.WaitOne(TimeSpan.FromSeconds(10));

                var target = component1.GetServiceProxy<ITestService1>();
                var expected1 = new AcceptanceMessageType1();
                var expected2 = new AcceptanceMessageType2();

                Assert.DoesNotThrow(() => target.EmptyMethodWithSeveralParameters(expected1, expected2));
                finishedEvent.WaitOne(TimeSpan.FromSeconds(5));

                Assert.IsTrue(component1.Tracker.EmptyMethodWithSeveralParametersCalled == 1);
                Assert.IsTrue(component1.Tracker.ParametersLastCall.Count == 2);
                Assert.AreEqual(expected1, component1.Tracker.ParametersLastCall[0]);
                Assert.AreEqual(expected2, component1.Tracker.ParametersLastCall[1]);
            }
        }


        [Test,TestCaseSource(typeof(TestCaseSources), "InMemoryDb")]
        public void CanInvoke_LocalCustomService_EmptyMethod_ReturnValue(DbEngineType engineType)
        {
            string dbConnString = TestSettingsProvider.GetConnString(engineType);

            const ushort c1Port = 10001;
            
            using (var component1 = TestComponent.GetComponent(true))
            {
                InitializeLonelyComponent(engineType, c1Port, component1, dbConnString);

                var finishedEvent = new AutoResetEvent(false);
                var registeredEvent = new AutoResetEvent(false);
                component1.RegisterService(1, registeredEvent, finishedEvent);
                registeredEvent.WaitOne(TimeSpan.FromSeconds(10));

                var target = component1.GetServiceProxy<ITestService1>();
                var actual = target.ReturnMethod();
                finishedEvent.WaitOne(TimeSpan.FromSeconds(5));

                Assert.IsTrue(component1.Tracker.ReturnMethodCalled == 1);
                Assert.IsTrue(component1.Tracker.ParametersLastCall.Count == 0);
                Assert.AreEqual(component1.Tracker.ResultLastCall, actual);
            }
        }

        [Test,TestCaseSource(typeof(TestCaseSources), "InMemoryDb")]
        public void CanInvoke_LocalCustomService_OneParamMethod_ReturnValue(DbEngineType engineType)
        {
            string dbConnString = TestSettingsProvider.GetConnString(engineType);

            const ushort c1Port = 10001;
            
            using (var component1 = TestComponent.GetComponent(true))
            {
                InitializeLonelyComponent(engineType, c1Port, component1, dbConnString);

                var finishedEvent = new AutoResetEvent(false);
                var registeredEvent = new AutoResetEvent(false);
                component1.RegisterService(1, registeredEvent, finishedEvent);
                registeredEvent.WaitOne(TimeSpan.FromSeconds(10));

                var target = component1.GetServiceProxy<ITestService1>();
                var expected = new AcceptanceMessageType1();

                var actual = target.ReturnMethodWithOneParameter(expected);
                finishedEvent.WaitOne(TimeSpan.FromSeconds(5));

                Assert.IsTrue(component1.Tracker.ReturnMethodWithOneParameterCalled == 1);
                Assert.IsTrue(component1.Tracker.ParametersLastCall.Count == 1);
                Assert.AreEqual(expected, component1.Tracker.ParametersLastCall[0]);
                Assert.AreEqual(component1.Tracker.ResultLastCall, actual);
            }
        }

        [Test,TestCaseSource(typeof(TestCaseSources), "InMemoryDb")]
        public void CanInvoke_LocalCustomService_SeveralParamsMethod_ReturnValue(DbEngineType engineType)
        {
            string dbConnString = TestSettingsProvider.GetConnString(engineType);

            const ushort c1Port = 10001;
            
            using (var component1 = TestComponent.GetComponent(true))
            {
                InitializeLonelyComponent(engineType, c1Port, component1, dbConnString);

                var finishedEvent = new AutoResetEvent(false);
                var registeredEvent = new AutoResetEvent(false);
                component1.RegisterService(1, registeredEvent, finishedEvent);
                registeredEvent.WaitOne(TimeSpan.FromSeconds(10));

                var target = component1.GetServiceProxy<ITestService1>();
                var expected1 = new AcceptanceMessageType1();
                var expected2 = new AcceptanceMessageType2();

                var actual = target.ReturnMethodWithSeveralParameters(expected1, expected2);
                finishedEvent.WaitOne(TimeSpan.FromSeconds(5));

                Assert.IsTrue(component1.Tracker.ReturnMethodWithSeveralParametersCalled == 1);
                Assert.IsTrue(component1.Tracker.ParametersLastCall.Count == 2);
                Assert.AreEqual(expected1, component1.Tracker.ParametersLastCall[0]);
                Assert.AreEqual(expected2, component1.Tracker.ParametersLastCall[1]);
                Assert.AreEqual(component1.Tracker.ResultLastCall, actual);
            }
        }

        
        [Test,TestCaseSource(typeof(TestCaseSources), "InMemoryDb")]
        public void CanInvoke_LocalCustomService_ArrayParamsMethod_Returns(DbEngineType engineType)
        {
            string dbConnString = TestSettingsProvider.GetConnString(engineType);

            const ushort c1Port = 10001;
            
            using (var component1 = TestComponent.GetComponent(true))
            {
                InitializeLonelyComponent(engineType, c1Port, component1, dbConnString);

                var finishedEvent = new AutoResetEvent(false);
                var registeredEvent = new AutoResetEvent(false);
                component1.RegisterService(1, registeredEvent, finishedEvent);
                registeredEvent.WaitOne(TimeSpan.FromSeconds(10));

                var target = component1.GetServiceProxy<ITestService1>();
                var expected1 = new AcceptanceMessageType1();
                var expected2 = new AcceptanceMessageType1();

                var actual = target.ReturnMethodWithSeveralParametersParams(expected1, expected2);
                finishedEvent.WaitOne(TimeSpan.FromSeconds(5));

                Assert.IsTrue(component1.Tracker.ReturnMethodWithArrayParametersCalled == 1);
                Assert.IsTrue(component1.Tracker.ParametersLastCall.Count == 2);
                Assert.AreEqual(expected1, component1.Tracker.ParametersLastCall[0]);
                Assert.AreEqual(expected2, component1.Tracker.ParametersLastCall[1]);
                Assert.AreEqual(component1.Tracker.ResultLastCall, actual);
            }
        }
       
        [Test,TestCaseSource(typeof(TestCaseSources), "InMemoryDb")]
        public void CanInvoke_LocalCustomService_ReturnsArray(DbEngineType engineType)
        {
            string dbConnString = TestSettingsProvider.GetConnString(engineType);

            const ushort c1Port = 10001;
            
            using (var component1 = TestComponent.GetComponent(true))
            {
                InitializeLonelyComponent(engineType, c1Port, component1, dbConnString);

                var finishedEvent = new AutoResetEvent(false);
                var registeredEvent = new AutoResetEvent(false);
                component1.RegisterService(1, registeredEvent, finishedEvent);
                registeredEvent.WaitOne(TimeSpan.FromSeconds(10));

                var target = component1.GetServiceProxy<ITestService1>();
                var actual = target.ReturnArrayMethod();
                finishedEvent.WaitOne(TimeSpan.FromSeconds(5));

                Assert.IsTrue(component1.Tracker.ReturnArrayMethodCalled == 1);
                Assert.IsTrue(component1.Tracker.ParametersLastCall.Count == 0);
                CollectionAssert.AreEqual((AcceptanceMessageType1[])component1.Tracker.ResultLastCall, actual);
            }
        }

        #endregion

        #region remote

        [Test,TestCaseSource(typeof(TestCaseSources), "InMemoryDb")]
        public void CanInvoke_RemoteCustomService_EmptyMethod(DbEngineType engineType)
        {
            string dbConnString = TestSettingsProvider.GetConnString(engineType);

            const ushort c1Port = 10001;
            const ushort c2Port = 10002;
            
            using (var component1 = TestComponent.GetComponent())
            using (var component2 = TestComponent.GetComponent(true))
            {
                InitializeLonelyComponent(engineType,c1Port, component1, dbConnString);
                InitializeConnectedComponent(engineType, c1Port, c2Port, component1, dbConnString, component2);

                var finishedEvent = new AutoResetEvent(false);
                var registeredEvent = new AutoResetEvent(false);
                component1.RegisterService(1, registeredEvent, finishedEvent);
                registeredEvent.WaitOne(TimeSpan.FromSeconds(10));

                var target = component2.GetServiceProxy<ITestService1>();
                Assert.DoesNotThrow(target.EmptyMethod);
                finishedEvent.WaitOne(TimeSpan.FromSeconds(5));

                Assert.IsTrue(component2.Tracker.EmptyMethodCalled == 1);
                Assert.IsTrue(component2.Tracker.ParametersLastCall.Count == 0);
            }
        }

        [Test,TestCaseSource(typeof(TestCaseSources), "InMemoryDb")]
        public void CanInvoke_RemoteCustomService_OneParamMethod(DbEngineType engineType)
        {
            string dbConnString = TestSettingsProvider.GetConnString(engineType);

            const ushort c1Port = 10001;
            const ushort c2Port = 10002;
            
            using (var component1 = TestComponent.GetComponent())
            using (var component2 = TestComponent.GetComponent(true))
            {
                InitializeLonelyComponent(engineType, c1Port, component1, dbConnString);
                InitializeConnectedComponent(engineType, c1Port, c2Port, component1, dbConnString, component2);

                var finishedEvent = new AutoResetEvent(false);
                var registeredEvent = new AutoResetEvent(false);
                component1.RegisterService(1, registeredEvent, finishedEvent);
                registeredEvent.WaitOne(TimeSpan.FromSeconds(10));

                var target = component2.GetServiceProxy<ITestService1>();
                var expected = new AcceptanceMessageType1();

                Assert.DoesNotThrow(() => target.EmptyMethodWithOneParameter(expected));
                finishedEvent.WaitOne(TimeSpan.FromSeconds(5));

                Assert.IsTrue(component2.Tracker.EmptyMethodWithOneParameterCalled == 1);
                Assert.IsTrue(component2.Tracker.ParametersLastCall.Count == 1);
                Assert.AreEqual(expected, component2.Tracker.ParametersLastCall[0]);
            }
        }

        [Test,TestCaseSource(typeof(TestCaseSources), "InMemoryDb")]
        public void CanInvoke_RemoteCustomService_SeveralParamsMethod(DbEngineType engineType)
        {
            string dbConnString = TestSettingsProvider.GetConnString(engineType);

            const ushort c1Port = 10001;
            const ushort c2Port = 10002;
            
            using (var component1 = TestComponent.GetComponent())
            using (var component2 = TestComponent.GetComponent(true))
            {
                InitializeLonelyComponent(engineType, c1Port, component1, dbConnString);
                InitializeConnectedComponent(engineType, c1Port, c2Port, component1, dbConnString, component2);

                var finishedEvent = new AutoResetEvent(false);
                var registeredEvent = new AutoResetEvent(false);
                component1.RegisterService(1, registeredEvent, finishedEvent);
                registeredEvent.WaitOne(TimeSpan.FromSeconds(10));

                var target = component2.GetServiceProxy<ITestService1>();
                var expected1 = new AcceptanceMessageType1();
                var expected2 = new AcceptanceMessageType2();

                Assert.DoesNotThrow(() => target.EmptyMethodWithSeveralParameters(expected1, expected2));
                finishedEvent.WaitOne(TimeSpan.FromSeconds(5));

                Assert.IsTrue(component2.Tracker.EmptyMethodWithSeveralParametersCalled == 1);
                Assert.IsTrue(component2.Tracker.ParametersLastCall.Count == 2);
                Assert.AreEqual(expected1, component2.Tracker.ParametersLastCall[0]);
                Assert.AreEqual(expected2, component2.Tracker.ParametersLastCall[1]);
            }
        }


        [Test,TestCaseSource(typeof(TestCaseSources), "InMemoryDb")]
        public void CanInvoke_RemoteCustomService_EmptyMethod_ReturnValue(DbEngineType engineType)
        {
            string dbConnString = TestSettingsProvider.GetConnString(engineType);

            const ushort c1Port = 10001;
            const ushort c2Port = 10002;
            
            using (var component1 = TestComponent.GetComponent())
            using (var component2 = TestComponent.GetComponent(true))
            {
                InitializeLonelyComponent(engineType, c1Port, component1, dbConnString);
                InitializeConnectedComponent(engineType, c1Port, c2Port, component1, dbConnString, component2);

                var finishedEvent = new AutoResetEvent(false);
                var registeredEvent = new AutoResetEvent(false);
                component1.RegisterService(1, registeredEvent, finishedEvent);
                registeredEvent.WaitOne(TimeSpan.FromSeconds(10));

                var target = component2.GetServiceProxy<ITestService1>();
                var actual = target.ReturnMethod();
                finishedEvent.WaitOne(TimeSpan.FromSeconds(5));

                Assert.IsTrue(component2.Tracker.ReturnMethodCalled == 1);
                Assert.IsTrue(component2.Tracker.ParametersLastCall.Count == 0);
                Assert.AreEqual(component2.Tracker.ResultLastCall, actual);
            }
        }

        [Test,TestCaseSource(typeof(TestCaseSources), "InMemoryDb")]
        public void CanInvoke_RemoteCustomService_OneParamMethod_ReturnValue(DbEngineType engineType)
        {
            string dbConnString = TestSettingsProvider.GetConnString(engineType);

            const ushort c1Port = 10001;
            const ushort c2Port = 10002;
            
            using (var component1 = TestComponent.GetComponent())
            using (var component2 = TestComponent.GetComponent(true))
            {
                InitializeLonelyComponent(engineType, c1Port, component1, dbConnString);
                InitializeConnectedComponent(engineType, c1Port, c2Port, component1, dbConnString, component2);

                var finishedEvent = new AutoResetEvent(false);
                var registeredEvent = new AutoResetEvent(false);
                component1.RegisterService(1, registeredEvent, finishedEvent);
                registeredEvent.WaitOne(TimeSpan.FromSeconds(10));

                var target = component2.GetServiceProxy<ITestService1>();
                var expected = new AcceptanceMessageType1();

                var actual = target.ReturnMethodWithOneParameter(expected);
                finishedEvent.WaitOne(TimeSpan.FromSeconds(5));

                Assert.IsTrue(component2.Tracker.ReturnMethodWithOneParameterCalled == 1);
                Assert.IsTrue(component2.Tracker.ParametersLastCall.Count == 1);
                Assert.AreEqual(expected, component2.Tracker.ParametersLastCall[0]);
                Assert.AreEqual(component2.Tracker.ResultLastCall, actual);
            }
        }

        [Test,TestCaseSource(typeof(TestCaseSources), "InMemoryDb")]
        public void CanInvoke_RemoteCustomService_SeveralParamsMethod_ReturnValue(DbEngineType engineType)
        {
            string dbConnString = TestSettingsProvider.GetConnString(engineType);

            const ushort c1Port = 10001;
            const ushort c2Port = 10002;
            
            using (var component1 = TestComponent.GetComponent())
            using (var component2 = TestComponent.GetComponent(true))
            {
                InitializeLonelyComponent(engineType, c1Port, component1, dbConnString);
                InitializeConnectedComponent(engineType, c1Port, c2Port, component1, dbConnString, component2);

                var finishedEvent = new AutoResetEvent(false);
                var registeredEvent = new AutoResetEvent(false);
                component1.RegisterService(1, registeredEvent, finishedEvent);
                registeredEvent.WaitOne(TimeSpan.FromSeconds(10));

                var target = component2.GetServiceProxy<ITestService1>();
                var expected1 = new AcceptanceMessageType1();
                var expected2 = new AcceptanceMessageType2();

                var actual = target.ReturnMethodWithSeveralParameters(expected1, expected2);
                finishedEvent.WaitOne(TimeSpan.FromSeconds(5));

                Assert.IsTrue(component2.Tracker.ReturnMethodWithSeveralParametersCalled == 1);
                Assert.IsTrue(component2.Tracker.ParametersLastCall.Count == 2);
                Assert.AreEqual(expected1, component2.Tracker.ParametersLastCall[0]);
                Assert.AreEqual(expected2, component2.Tracker.ParametersLastCall[1]);
                Assert.AreEqual(component2.Tracker.ResultLastCall, actual);
            }
        }

        [Test,TestCaseSource(typeof(TestCaseSources), "InMemoryDb")]
        public void CanInvoke_RemoteCustomService_ArrayParamsMethod_Returns(DbEngineType engineType)
        {
            string dbConnString = TestSettingsProvider.GetConnString(engineType);

            const ushort c1Port = 10001;
            const ushort c2Port = 10002;

            using (var component1 = TestComponent.GetComponent())
            using (var component2 = TestComponent.GetComponent(true))
            {
                InitializeLonelyComponent(engineType, c1Port, component1, dbConnString);
                InitializeConnectedComponent(engineType, c1Port, c2Port, component1, dbConnString, component2);

                var finishedEvent = new AutoResetEvent(false);
                var registeredEvent = new AutoResetEvent(false);
                component1.RegisterService(1, registeredEvent, finishedEvent);
                registeredEvent.WaitOne(TimeSpan.FromSeconds(10));

                var target = component2.GetServiceProxy<ITestService1>();
                var expected1 = new AcceptanceMessageType1();
                var expected2 = new AcceptanceMessageType1();

                var actual = target.ReturnMethodWithSeveralParametersParams(expected1, expected2);
                finishedEvent.WaitOne(TimeSpan.FromSeconds(5));

                Assert.IsTrue(component2.Tracker.ReturnMethodWithArrayParametersCalled == 1);
                Assert.IsTrue(component2.Tracker.ParametersLastCall.Count == 2);
                Assert.AreEqual(expected1, component2.Tracker.ParametersLastCall[0]);
                Assert.AreEqual(expected2, component2.Tracker.ParametersLastCall[1]);
                Assert.AreEqual(component2.Tracker.ResultLastCall, actual);
            }
        }

        [Test,TestCaseSource(typeof(TestCaseSources), "InMemoryDb")]
        public void CanInvoke_RemoteCustomService_ReturnsArray(DbEngineType engineType)
        {
            string dbConnString = TestSettingsProvider.GetConnString(engineType);

            const ushort c1Port = 10001;
            const ushort c2Port = 10002;

            using (var component1 = TestComponent.GetComponent()) 
            using (var component2 = TestComponent.GetComponent(true))
            {
                InitializeLonelyComponent(engineType, c1Port, component1, dbConnString);
                InitializeConnectedComponent(engineType, c1Port, c2Port, component1, dbConnString, component2);

                var finishedEvent = new AutoResetEvent(false);
                var registeredEvent = new AutoResetEvent(false);
                component1.RegisterService(1, registeredEvent, finishedEvent);
                registeredEvent.WaitOne(TimeSpan.FromSeconds(10));

                var target = component2.GetServiceProxy<ITestService1>();
                var actual = target.ReturnArrayMethod();
                finishedEvent.WaitOne(TimeSpan.FromSeconds(5));

                Assert.IsTrue(component2.Tracker.ReturnArrayMethodCalled == 1);
                Assert.IsTrue(component2.Tracker.ParametersLastCall.Count == 0);
                CollectionAssert.AreEqual((AcceptanceMessageType1[])component2.Tracker.ResultLastCall, actual);
            }
        }

        
        #endregion


        [Test,TestCaseSource(typeof(TestCaseSources), "InMemoryDb")]
        public void When_Service_NotDefined_Returns_Null(DbEngineType engineType)
        {
            string dbConnString = TestSettingsProvider.GetConnString(engineType);

            ushort c1Port = new TestPort(10000,10100);

            using (var component1 = TestComponent.GetComponent(true))            
            {
                InitializeLonelyComponent(engineType, c1Port, component1, dbConnString);
                
                var target = component1.GetServiceProxy<ITestService1>();
                Assert.IsNull(target);
            }
        }

        [Test,TestCaseSource(typeof(TestCaseSources), "InMemoryDb")]
        public void When_CannotInvoke_ThrowsException(DbEngineType engineType)
        {
            string dbConnString = TestSettingsProvider.GetConnString(engineType);

            const ushort c1Port = 10001;
            const ushort c2Port = 10002;

            using (var component2 = TestComponent.GetComponent(true))
            {
                var finishedEvent = new AutoResetEvent(false);
                using (var component1 = TestComponent.GetComponent())
                {
                    InitializeLonelyComponent(engineType, c1Port, component1, dbConnString);
                    InitializeConnectedComponent(engineType, c1Port, c2Port, component1, dbConnString, component2);

                    var registeredEvent = new AutoResetEvent(false);
                    component1.RegisterService(1, registeredEvent, finishedEvent);
                    registeredEvent.WaitOne(TimeSpan.FromSeconds(10));
                }
                var target = component2.GetServiceProxy<ITestService1>();
                Assert.Throws<ermeXComponentNotAvailableException>(target.EmptyMethod);

                Assert.IsTrue(component2.Tracker.EmptyMethodCalled == 0);
                Assert.IsTrue(component2.Tracker.ParametersLastCall.Count == 0);
            }

        }

        [Test,TestCaseSource(typeof(TestCaseSources), "InMemoryDb")]
        public void TwoComponents_CannotRegister_TheSameService_IfReturnValues(DbEngineType engineType)
        {
            string dbConnString = TestSettingsProvider.GetConnString(engineType);

            const ushort c1Port = 10001;
            const ushort c2Port = 10002;

            using (var component1 = TestComponent.GetComponent())
            using (var component2 = TestComponent.GetComponent(true))
            {
                InitializeLonelyComponent(engineType, c1Port, component1, dbConnString);
                InitializeConnectedComponent(engineType, c1Port, c2Port, component1, dbConnString, component2);

                var finishedEvent1 = new AutoResetEvent(false);
                var registeredEvent1 = new AutoResetEvent(false);
                var finishedEvent2 = new AutoResetEvent(false);
                var registeredEvent2 = new AutoResetEvent(false);
                
                component1.RegisterService<ITestService1>(1, registeredEvent1, finishedEvent1);
                Assert.Throws<InvalidOperationException>(()=>component2.RegisterService<ITestService1>(0, registeredEvent2, finishedEvent2));
            }
        }

        [Test,TestCaseSource(typeof(TestCaseSources), "InMemoryDb")]
        public void TwoComponents_CanRegister_TheSameService_If_Dont_ReturnValues(DbEngineType engineType)
        {
            string dbConnString = TestSettingsProvider.GetConnString(engineType);

            const ushort c1Port = 10001;
            const ushort c2Port = 10002;

            using (var component1 = TestComponent.GetComponent())
            using (var component2 = TestComponent.GetComponent(true))
            {
                InitializeLonelyComponent(engineType, c1Port, component1, dbConnString);
                InitializeConnectedComponent(engineType, c1Port, c2Port, component1, dbConnString, component2);

                var finishedEvent1 = new AutoResetEvent(false);
                var registeredEvent1 = new AutoResetEvent(false);
                var finishedEvent2 = new AutoResetEvent(false);
                var registeredEvent2 = new AutoResetEvent(false);

                Assert.DoesNotThrow(()=>component1.RegisterService<ITestService3>(0, registeredEvent1, finishedEvent1));
                Assert.DoesNotThrow(() => component2.RegisterService<ITestService3>(0, registeredEvent2, finishedEvent2));
            }
        }
      
        [Test,TestCaseSource(typeof(TestCaseSources), "InMemoryDb")]
        public void Components_CanPublish_Several_Services(DbEngineType engineType)
        {
            string dbConnString = TestSettingsProvider.GetConnString(engineType);

            const ushort c1Port = 10001;
            const ushort c2Port = 10002;

            using (var component1 = TestComponent.GetComponent())
            using (var component2 = TestComponent.GetComponent(true))
            {
                InitializeLonelyComponent(engineType, c1Port, component1, dbConnString);
                InitializeConnectedComponent(engineType, c1Port, c2Port, component1, dbConnString, component2);

                var finishedEvent1 = new AutoResetEvent(false);
                var registeredEvent1 = new AutoResetEvent(false);
                component1.RegisterService<ITestService1>(1, registeredEvent1, finishedEvent1);

                var finishedEvent2 = new AutoResetEvent(false);
                var registeredEvent2 = new AutoResetEvent(false);
                component1.RegisterService<ITestService2>(1, registeredEvent2, finishedEvent2);

                WaitHandle.WaitAll(new []{registeredEvent1,registeredEvent2},TimeSpan.FromSeconds(10));

                var target1 = component2.GetServiceProxy<ITestService1>();
                var target2 = component2.GetServiceProxy<ITestService2>();

                Assert.DoesNotThrow(target1.EmptyMethod);

                Assert.IsTrue(component2.Tracker.EmptyMethodCalled == 1);
                Assert.IsTrue(component2.Tracker.ParametersLastCall.Count == 0);


                Assert.DoesNotThrow(target2.EmptyMethod);

                WaitHandle.WaitAll(new[] { finishedEvent1, finishedEvent2 }, TimeSpan.FromSeconds(10));

                Assert.IsTrue(component2.Tracker.EmptyMethodCalled == 2, string.Format("component2.Tracker.EmptyMethodCalled: {0}", component2.Tracker.EmptyMethodCalled));
                Assert.IsTrue(component2.Tracker.ParametersLastCall.Count == 0);

               
            }
        }

        [Ignore("FIX")]
        [Test,TestCaseSource(typeof(TestCaseSources), "InMemoryDb")]
        public void SeveralComponents_Can_Serve_Same_Service_NoReturnValues(DbEngineType engineType)
        {
            string dbConnString = TestSettingsProvider.GetConnString(engineType);

            const ushort c1Port = 10001;
            const ushort c2Port = 10002;
            const ushort c3Port = 10003;

            using (var component1 = TestComponent.GetComponent())
            using (var component2 = TestComponent.GetComponent())
            using (var component3 = TestComponent.GetComponent())
            {
                InitializeLonelyComponent(engineType, c1Port, component1, dbConnString);
                InitializeConnectedComponent(engineType, c1Port, c2Port, component1, dbConnString, component2);
                InitializeConnectedComponent(engineType, c1Port, c3Port, component1, dbConnString, component3);

                var finishedEvent1 = new AutoResetEvent(false);
                var registeredEvent1 = new AutoResetEvent(false);
                var finishedEvent2 = new AutoResetEvent(false);
                var registeredEvent2 = new AutoResetEvent(false);

                component2.RegisterService<ITestService3>(1, registeredEvent1, finishedEvent1);
                component3.RegisterService<ITestService3>(1, registeredEvent2, finishedEvent2);
                WaitHandle.WaitAll(new[] { registeredEvent1, registeredEvent2}, TimeSpan.FromSeconds(20));
               
                var serviceProxy = component1.GetServiceProxy<ITestService3>();

                serviceProxy.EmptyMethod();

                WaitHandle.WaitAll(new[] { finishedEvent1, finishedEvent2 }, TimeSpan.FromSeconds(20));

                Assert.IsTrue(component2.Tracker.EmptyMethodCalled == 1, string.Format("component2.Tracker.EmptyMethodCalled: {0}", component2.Tracker.EmptyMethodCalled));
                Assert.IsTrue(component2.Tracker.ParametersLastCall.Count == 0);
                
                Assert.IsTrue(component3.Tracker.EmptyMethodCalled == 1, string.Format("component3.Tracker.EmptyMethodCalled: {0}", component3.Tracker.EmptyMethodCalled));
                Assert.IsTrue(component3.Tracker.ParametersLastCall.Count == 0);
            }
        }

   }
}
