using System;
using NUnit.Framework;
using NUnit.Isolation.Api;

namespace NUnit.Isolation
{
    public static class IsolationDispatcher
    {
        public static bool IsRootAppDomainOfIsolatedProcess { get; set; }
        public static bool IsInIsolatedAppDomain { get; set; }

        public static void IsolateTestRun(IsolationType isolationType, TestMethodInformation testMethodInformation)
        {
            if (IsInIsolatedAppDomain)
                return;

            switch (isolationType)
            {
                case IsolationType.AppDomain:
                    AppDomainRunner.Run(testMethodInformation, true);
                    break;
                case IsolationType.Process:
                    ProcessRunner.Run(testMethodInformation);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            Assert.Pass();
        }


    }
}
