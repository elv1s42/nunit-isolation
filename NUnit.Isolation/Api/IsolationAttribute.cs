using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace NUnit.Isolation.Api
{
    public enum IsolationType
    {
        AppDomain,
        Process
    }
 
    [AttributeUsage(AttributeTargets.Method)]
    public class IsolationAttribute : Attribute, ITestAction
    {
        private readonly IsolationType mIsolationType;
        private readonly bool mAttachDebugger;

        public IsolationAttribute(IsolationType isolationType, bool attachDebugger = false)
        {
            mIsolationType = isolationType;
            mAttachDebugger = attachDebugger;
        }

        public void BeforeTest(ITest test)
        {
            var testMethodInformation = new TestMethodInformation(test.Method.MethodInfo, mAttachDebugger);
            IsolationDispatcher.IsolateTestRun(mIsolationType, testMethodInformation);
        }

        public void AfterTest(ITest test)
        {
        }

        public ActionTargets Targets => ActionTargets.Test;
    }
}