using System;
using NUnit.Framework;
using NUnit.Isolation.Api;

namespace NUnit.Isolation.Tests
{
    [TestFixture]
    public class IsolationAttributeTest
    {
        [Test, Isolation(IsolationType.AppDomain)]
        public void AppDomainTest()
        {
            Assert.IsTrue(IsolationDispatcher.IsInIsolatedAppDomain);
            Assert.AreEqual(AppDomainRunner.ISOLATED_APP_DOMAIN_NAME, AppDomain.CurrentDomain.FriendlyName);
        }

        [Test, Isolation(IsolationType.Process, true)]
        public void ProcessTest()
        {
            Assert.IsTrue(IsolationDispatcher.IsRootAppDomainOfIsolatedProcess);
            Assert.IsTrue(IsolationDispatcher.IsInIsolatedAppDomain);
            Assert.AreEqual(AppDomainRunner.ISOLATED_APP_DOMAIN_NAME, AppDomain.CurrentDomain.FriendlyName);
        }
    }
}
