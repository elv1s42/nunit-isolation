using System;
using System.Diagnostics;

namespace NUnit.Isolation
{
    public class InProcessRunner
    {
        public static void Main(string[] args)
        {
            IsolationDispatcher.IsRootAppDomainOfIsolatedProcess = true;
            
            var attachDebugger = bool.Parse(args[0]);
            var testAssemblyFullName = args[1];
            var typeAssemblyQualifiedName = args[2];
            var testMethodName = args[3];
            
            var testMethodInformation = new TestMethodInformation(testAssemblyFullName, typeAssemblyQualifiedName, testMethodName, attachDebugger);

            if (testMethodInformation.AttachDebugger)
                Debugger.Launch();

            // remark: no need to close the appdomain, as we close the process
            AppDomainRunner.Run(testMethodInformation, false);
        }
    }
}
