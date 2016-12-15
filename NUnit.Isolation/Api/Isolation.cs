using System.Diagnostics;

namespace NUnit.Isolation.Api
{
    public static class Isolation
    {
        public static void ReRun(IsolationType isolationType, bool attachDebugger = false)
        {
            var methodInfo = new StackTrace().GetFrame(1).GetMethod();
            var testMethodInformation = new TestMethodInformation(methodInfo, attachDebugger);
            IsolationDispatcher.IsolateTestRun(isolationType, testMethodInformation);
        }
    }
}