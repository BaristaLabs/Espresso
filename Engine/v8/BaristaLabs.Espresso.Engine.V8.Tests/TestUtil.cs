namespace BaristaLabs.Espresso.Engine.V8.Tests
{
    using Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;

    public static class TestUtil
    {
        //public static void Iterate(Array array, Action<int[]> action)
        //{
        //    array.Iterate(action);
        //}

        public static string FormatInvariant(string format, params object[] args)
        {
            return MiscHelpers.FormatInvariant(format, args);
        }

        public static double CalcTestValue(Guid callerGuid, params object[] args)
        {
            var hashCode = args.Aggregate(callerGuid.GetHashCode(), (currentHashCode, value) => unchecked((currentHashCode * 31) + ((value != null) ? value.GetHashCode() : 0)));
            return hashCode * Math.E / Math.PI;
        }

        public static void AssertException<T>(Action action, bool checkScriptStackTrace = true) where T : Exception
        {
            Exception caughtException = null;
            var gotExpectedException = false;

            try
            {
                action();
            }
            catch (T exception)
            {
                caughtException = exception;
                gotExpectedException = true;
                AssertValidExceptionChain(exception, checkScriptStackTrace);
            }
            catch (Exception exception)
            {
                caughtException = exception;
                gotExpectedException = exception.GetBaseException() is T;
                AssertValidExceptionChain(exception, checkScriptStackTrace);
            }

            var message = "Expected " + typeof(T).Name + " was not thrown.";
            if (caughtException != null)
            {
                message += " " + caughtException.GetType().Name + " was thrown instead.";
            }

            Assert.IsTrue(gotExpectedException, message);
        }

        public static void AssertValidException(Exception exception)
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
            Assert.IsFalse(exception.Message.Contains("COM"));
            Assert.IsFalse(exception.Message.Contains("HRESULT"));
            Assert.IsFalse(exception.Message.Contains("0x"));
            Assert.IsFalse(string.IsNullOrWhiteSpace(exception.StackTrace));
        }

        public static void AssertValidException(IScriptEngineException exception, bool checkScriptStackTrace = true)
        {
            AssertValidException((Exception)exception);
            if ((exception is ScriptEngineException) && !exception.IsFatal && (exception.HResult != RawCOMHelpers.HResult.CLEARSCRIPT_E_SCRIPTITEMEXCEPTION))
            {
                Assert.IsTrue(exception.ErrorDetails.StartsWith(exception.Message, StringComparison.Ordinal));
                if (checkScriptStackTrace)
                {
                    Assert.IsTrue(exception.ErrorDetails.Contains("\n    at "));
                }
            }
        }

        public static void AssertValidException(IJavaScriptEngine engine, IScriptEngineException exception, bool checkScriptStackTrace = true)
        {
            AssertValidException(exception, checkScriptStackTrace);
            Assert.AreEqual(engine.Name, exception.EngineName);
        }

        public static void AssertValidException(V8ScriptEngine engine, IScriptEngineException exception, bool checkScriptStackTrace = true)
        {
            AssertValidException(exception, checkScriptStackTrace);
            Assert.AreEqual(engine.Name, exception.EngineName);
        }

        private static void AssertValidExceptionChain(Exception exception, bool checkScriptStackTrace)
        {
            while (exception != null)
            {
                var scriptError = exception as IScriptEngineException;
                if (scriptError != null)
                {
                    AssertValidException(scriptError, checkScriptStackTrace);
                }
                else
                {
                    AssertValidException(exception);
                }

                exception = exception.InnerException;
            }
        }
    }
}
