using System;
using System.Diagnostics;
using System.Reflection;
using NUnit.Framework;
using NUnit.Isolation.Exceptions;

namespace NUnit.Isolation
{
    /// <summary> Helps to run a test in another process. </summary>
    public static class ProcessRunner
    {
        public static void Run(TestMethodInformation testMethodInformation)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath,
                Arguments =
                    $@" ""{testMethodInformation.AttachDebugger}"" ""{testMethodInformation.AssemblyName}"" ""{testMethodInformation
                        .TypeAssemblyQualifiedName}"" ""{testMethodInformation.TestMethodName}"" ",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                ErrorDialog = false,
                UseShellExecute = false
            };
            
            var process = new Process
            {
                StartInfo = startInfo
            };
            process.OutputDataReceived += (sender, eventArgs) => Console.WriteLine(eventArgs.Data);
            process.ErrorDataReceived += (sender, eventArgs) => Console.Error.WriteLine(eventArgs.Data);
            
            try
            {
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
            }
            finally
            {
                if (!process.HasExited)
                {
                    process.Kill();
                    throw new NotExitingProcessException();
                }
            }

            var exitCode = process.ExitCode;
            if (exitCode != 0)
                throw new TestRunInSubProcessFailedException($"Isolated test failed (ExitCode = {exitCode}). " +
                                                             $"See output for more information about the exception");

            // everything ok
            Assert.Pass();
        }
    }
}