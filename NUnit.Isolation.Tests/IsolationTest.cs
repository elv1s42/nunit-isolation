﻿using System;
using System.Configuration;
using DummyAssembly;
using NUnit.Framework;
using NUnit.Isolation.Api;

namespace NUnit.Isolation.Tests
{
    [TestFixture]
    public class IsolationTest
    {
        [Test]
        public void AppDomainTest()
        {
            Api.Isolation.ReRun(IsolationType.AppDomain);
            Assert.IsTrue(IsolationDispatcher.IsInIsolatedAppDomain);
            Assert.AreEqual(AppDomainRunner.ISOLATED_APP_DOMAIN_NAME, AppDomain.CurrentDomain.FriendlyName);
        }

        [Test]
        public void ProcessTest()
        {
            Api.Isolation.ReRun(IsolationType.Process);

            StringAssert.Contains("NUnit.Isolation.exe".ToLower(), Environment.CommandLine.ToLower());
            Assert.IsFalse(IsolationDispatcher.IsRootAppDomainOfIsolatedProcess, "test must run in a separate app domain within the separate process");
            Assert.IsTrue(IsolationDispatcher.IsInIsolatedAppDomain);
            Assert.AreEqual(AppDomainRunner.ISOLATED_APP_DOMAIN_NAME, AppDomain.CurrentDomain.FriendlyName);
        }

        [Test]
        public void ProcessTest_TestExpectedToThrowAnException()
        {
            Api.Isolation.ReRun(IsolationType.Process);
            Assert.Throws<ApplicationException>(() =>
            {
                throw new ApplicationException("voilà");
            });
        }

        [Test]
        public void ProcessTest_AppSettings()
        {
            Api.Isolation.ReRun(IsolationType.Process);
            Assert.AreEqual("testValue", ConfigurationManager.AppSettings["testKey"], "app.config expected to be read in separate process and separate appdomain");
        }

        [Test]
        public void ProcessTest_AppSettings_assemblyBinding()
        {
            Api.Isolation.ReRun(IsolationType.Process);

            // remark: DummyAssembly is not copied into target folder, but to targetfolder\libfolder
            // which has been added to app.config runtime assemblyBinding section
            // this test checks, that the app.config runtime assemblyBinding section is correctly taken into account
            var dummyMethod = DummyClass.DummyMethod();
        }
    }
}
