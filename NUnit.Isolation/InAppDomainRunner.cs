using System.Linq;
using System;
using NUnit.Framework;

namespace NUnit.Isolation
{
    public sealed class InAppDomainRunner : MarshalByRefObject
    {
        public void Execute(TestMethodInformation testMethodInformation)
        {
            IsolationDispatcher.IsInIsolatedAppDomain = true;
            
            // TODO: re-use Nunit material instead of doing this ourself

            //Assembly assembly = Assembly.Load(testMethodInformation.AssemblyName);
            //List<Type> types = assembly.GetTypes().ToList();

            var typeUnderTest = Type.GetType(testMethodInformation.TypeAssemblyQualifiedName);

            if (typeUnderTest == null)
                throw new TypeLoadException(testMethodInformation.TypeAssemblyQualifiedName);

            var instance = Activator.CreateInstance(typeUnderTest);

            typeUnderTest.GetMethods()
                .Where(m => m.GetCustomAttributes(typeof(SetUpFixtureAttribute), false).Length > 0)
                .ToList()
                .ForEach(m => m.Invoke(instance, null));

            typeUnderTest.GetMethods()
                .Where(m => m.GetCustomAttributes(typeof(OneTimeTearDownAttribute), false).Length > 0)
                .ToList()
                .ForEach(m => m.Invoke(instance, null));

            typeUnderTest.GetMethods()
                .Where(m => m.GetCustomAttributes(typeof(SetUpAttribute), false).Length > 0)
                .ToList()
                .ForEach(m => m.Invoke(instance, null));

            typeUnderTest.GetMethods()
                .Where(m => m.GetCustomAttributes(typeof(TearDownAttribute), false).Length > 0)
                .ToList()
                .ForEach(m => m.Invoke(instance, null));

            typeUnderTest.GetMethod(testMethodInformation.TestMethodName)
                .Invoke(instance, null);
        }
    }
}