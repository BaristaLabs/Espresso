namespace BaristaLabs.Espresso.Engine.V8.Tests
{
    using System;
    using System.ComponentModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BadV8DeploymentTest
    {
        #region test methods

        // ReSharper disable InconsistentNaming

        [TestMethod, TestCategory("BadV8Deployment")]
        [DeploymentItem("BaristaLabs.Espresso.Engine.V8.Native.dll", "BadV8Deployment_NoNativeLibrary")]
        public void BadV8Deployment_NoNativeLibrary()
        {
            V8Proxy.RunWithDeploymentDir("BadV8Deployment_NoNativeLibrary", () =>
            {
                var testException = new Win32Exception(126 /*ERROR_MOD_NOT_FOUND*/);
                TypeLoadException caughtException = null;

                try
                {
                    using (new V8ScriptEngine())
                    {
                    }
                }
                catch (TypeLoadException exception)
                {
                    caughtException = exception;
                }

                Assert.IsNotNull(caughtException);
                // ReSharper disable once PossibleNullReferenceException
                Assert.IsTrue(caughtException.Message.Contains(testException.Message));
            });
        }

        [TestMethod, TestCategory("BadV8Deployment")]
        [DeploymentItem("v8-x64.dll", "BadV8Deployment_NoManagedAssembly")]
        public void BadV8Deployment_NoManagedAssembly()
        {
            V8Proxy.RunWithDeploymentDir("BadV8Deployment_NoManagedAssembly", () =>
            {
                var testException = new Win32Exception(2 /*ERROR_FILE_NOT_FOUND*/);
                TypeLoadException caughtException = null;

                try
                {
                    using (new V8ScriptEngine())
                    {
                    }
                }
                catch (TypeLoadException exception)
                {
                    caughtException = exception;
                }

                Assert.IsNotNull(caughtException);
                // ReSharper disable once PossibleNullReferenceException
                Assert.IsTrue(caughtException.Message.Contains(testException.Message));
            });
        }

        // ReSharper restore InconsistentNaming

        #endregion
    }
}