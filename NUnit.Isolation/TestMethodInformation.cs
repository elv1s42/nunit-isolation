using System.Reflection;
using System;

namespace NUnit.Isolation
{
    [Serializable]
    public class TestMethodInformation
    {
        #region private fields
        private readonly string mAssemblyName;
        private readonly string mTypeAssemblyQualifiedName;
        private readonly string mTestMethodName;
        private readonly bool mAttachDebugger;
        #endregion

        #region constructor
        public TestMethodInformation(MethodBase method, bool attachDebugger)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));

            if (method.DeclaringType == null)
                throw new ArgumentException("method expected to have a valid DecalringType but it is null");

            mAssemblyName = method.DeclaringType.Assembly.GetName().Name;
            mTypeAssemblyQualifiedName = method.DeclaringType.AssemblyQualifiedName;
            mTestMethodName = method.Name;
            mAttachDebugger = attachDebugger;
        }

        public TestMethodInformation(string assemblyName, string typeAssemblyQualifiedName, string testMethodName, bool attachDebugger)
        {
            mAssemblyName = assemblyName;
            mTypeAssemblyQualifiedName = typeAssemblyQualifiedName;
            mTestMethodName = testMethodName;
            mAttachDebugger = attachDebugger;
        }
        #endregion

        #region public properties
        public string AssemblyName => mAssemblyName;

        // TODO: case unit test are in exe
        public string ConfigurationFile => mAssemblyName + ".dll.config";

        public string TypeAssemblyQualifiedName => mTypeAssemblyQualifiedName;

        public string TestMethodName => mTestMethodName;

        public bool AttachDebugger => mAttachDebugger;

        #endregion
    }
}