namespace BaristaLabs.Espresso.Core
{
    using Common;
    using Common.BrewResponses;
    using Utilities;
    using System;
    using Ninject;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a default barista implementation.
    /// </summary>
    [EspressoApi("1")]
    public class BaristaV1 : IBarista
    {
        private IKernel m_kernel;

        public BaristaV1(IKernel kernel)
        {
            if (kernel == null)
                throw new ArgumentNullException("kernel");

            m_kernel = kernel;
            JsonSettings.DefaultJsonSerializer = new JsonNetSerializer();
        }

        public IBrewResponse Brew(IBrewRequest request)
        {
            //TODO: Somehow inject all packages that are in the /packages folder, and install them on the script engine.
            //TODO: Get the FileSystem implementation from the application.
            //TODO: Set the script engine limits based on the application.
            
            //If there exists an x-espresso-debug header value, use debug (future functionality to use the value to do... something...)
            bool isDebug = false;
            var debugHeader = request.Headers.LastOrDefault(k => StringComparer.InvariantCultureIgnoreCase.Compare(k.Key, "x-espresso-debug") == 0);
            if (!debugHeader.Equals(default(KeyValuePair<string, IEnumerable<string>>)))
                isDebug = true;

            IBrewResponse getScriptEngineErrorResponse;
            var scriptEngine = GetScriptEngine(request, isDebug, out getScriptEngineErrorResponse);

            if (getScriptEngineErrorResponse != null)
                return getScriptEngineErrorResponse;

            if (scriptEngine == null)
                return new BrewErrorResponse("A IScriptEngine instance was not returned by the script engine factory associated with the request.");

            if (isDebug)
                return EvalDebug(request, scriptEngine);

            return Eval(request, scriptEngine);
        }

        private IScriptEngine GetScriptEngine(IBrewRequest request, bool isDebug, out IBrewResponse errorResponse)
        {
            //If there exists an x-espresso-script-engine-type header value, use the value as the type of script engine to get (Right now only V8, which is the default.)
            string scriptEngineType = "V8";
            var scriptEngineTypeHeader = request.Headers.LastOrDefault(k => StringComparer.InvariantCultureIgnoreCase.Compare(k.Key, "x-espresso-script-engine-type") == 0);
            if (!scriptEngineTypeHeader.Equals(default(KeyValuePair<string, IEnumerable<string>>)))
                scriptEngineType = scriptEngineTypeHeader.Value.FirstOrDefault();

            IScriptEngine scriptEngine;
            if (isDebug)
            {
                var debugScriptEngineFactory = m_kernel.TryGet<IDebugScriptEngineFactory>(b => b.Get<string>("Espresso-Script-Engine-Type") == scriptEngineType);
                if (debugScriptEngineFactory == null)
                {
                    errorResponse = new BrewErrorResponse("A Debug ScriptEngineFactory could not be located for the specified script engine type: '" + scriptEngineType + "'");
                    return null;
                }

                var port = PortUtilities.FindFreePort();
                scriptEngine = debugScriptEngineFactory.GetDebugScriptEngine(port);
            }
            else
            {
                var scriptEngineFactory = m_kernel.TryGet<IScriptEngineFactory>(b => b.Get<string>("Espresso-Script-Engine-Type") == scriptEngineType);
                if (scriptEngineFactory == null)
                {
                    errorResponse = new BrewErrorResponse("A ScriptEngineFactory could not be located for the specified script engine type: '" + scriptEngineType + "'");
                    return null;
                }

                scriptEngine = scriptEngineFactory.GetScriptEngine();
            }

            errorResponse = null;
            return scriptEngine;
        }

        private IBrewResponse Eval(IBrewRequest request, IScriptEngine scriptEngine)
        {
            try
            {
                var result = scriptEngine.Evaluate(request.VirtualScriptFilePath, "1+1");
                return new JsonBrewResponse(result);
            }
            catch (Exception ex)
            {
                if (ex is IScriptEngineException)
                {
                    return new ScriptEngineExceptionResponse(ex as IScriptEngineException);
                }
                else
                {
                    throw;
                }
            }
        }

        private IBrewResponse EvalDebug(IBrewRequest request, IScriptEngine scriptEngine)
        {
            var script = "1+1";

            //Only in debug mode allow non-get requests to specify code in the request body.
            if (request.Method != "GET" && string.IsNullOrWhiteSpace(request.VirtualScriptFilePath))
            {
                using (var ms = new MemoryStream())
                {
                    request.Body.CopyTo(ms);
                    ms.Seek(0, SeekOrigin.Begin);

                    using (var sr = new StreamReader(ms))
                    {
                        script = sr.ReadToEnd();
                    }
                }
            }

            try
            {
                var result = scriptEngine.Evaluate(request.VirtualScriptFilePath, script);
                return new JsonBrewResponse(result);
            }
            catch (Exception ex)
            {
                if (ex is IScriptEngineException)
                {
                    return new ScriptEngineExceptionResponse(ex as IScriptEngineException);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
