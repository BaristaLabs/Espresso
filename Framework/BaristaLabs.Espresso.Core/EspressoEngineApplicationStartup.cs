namespace BaristaLabs.Espresso.Core.v1
{
    using Common;
    using Responses;
    using Nancy.Bootstrapper;
    using Nancy.Routing;
    using Ninject;
    using Ninject.Parameters;
    using System.Text.RegularExpressions;
    using System.Linq;
    using System;

    public class EspressoEngineApplicationStartup : IApplicationStartup
    {
        Regex espressoMediaTypeRegex = new Regex(@"application/vnd.espresso(\.(?<versionNumber>[0-9]*))?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private readonly IKernel m_kernel;

        public EspressoEngineApplicationStartup(IKernel kernel)
        {
            if (kernel == null)
                throw new ArgumentNullException("kernel");

            m_kernel = kernel;
        }

        public void Initialize(IPipelines pipelines)
        {
            //Add the generic API route that handles any requests to the pipeline.
            pipelines.BeforeRequest.AddItemToStartOfPipeline(ctx =>
            {
                var rpm = new DefaultRoutePatternMatcher();
                var results = rpm.Match(ctx.Request.Url.Path, @"/api/{virtualScriptFilePath?}", new[] { "api", "{virtualScriptFilePath?}" }, ctx);

                //If we didn't match the "API" route, let something else handle the request and get outta dodge.
                if (!results.IsMatch)
                    return null;

                //use espresso media type to determine API version (forward compatibility)
                var espressoApiVersionNumber = "1"; //TODO: Get this default value from config.
                foreach (var acceptTuple in ctx.Request.Headers.Accept)
                {
                    var match = espressoMediaTypeRegex.Match(acceptTuple.Item1);

                    if (match.Success)
                    {
                        var versionNumberGroup = match.Groups["versionNumber"];
                        if (versionNumberGroup != null && versionNumberGroup.Success && !string.IsNullOrWhiteSpace(versionNumberGroup.Value))
                            espressoApiVersionNumber = versionNumberGroup.Value;
                    }
                }

                //If there exists an x-espresso-debug header value, use debug (future functionality to use the value to do... something...)
                bool isDebug = false;
                var debugKey = ctx.Request.Headers.Keys.LastOrDefault(k => StringComparer.InvariantCultureIgnoreCase.Compare(k, "x-espresso-debug") == 0);
                if (!string.IsNullOrWhiteSpace(debugKey))
                    isDebug = true;

                //If there exists an x-espresso-script-engine-type header value, use the value as the type of script engine to get (Right now only V8, which is the default.)
                string scriptEngineType = "V8";
                var scriptEngineTypeKey = ctx.Request.Headers.Keys.LastOrDefault(k => StringComparer.InvariantCultureIgnoreCase.Compare(k, "x-espresso-script-engine-type") == 0);
                if (!string.IsNullOrWhiteSpace(scriptEngineTypeKey))
                    scriptEngineType = ctx.Request.Headers[scriptEngineTypeKey].First();

                IScriptEngine scriptEngine;
                if (isDebug)
                {
                    var debugScriptEngineFactory = m_kernel.TryGet<IDebugScriptEngineFactory>(b => b.Get<string>("Espresso-Script-Engine-Type") == scriptEngineType);
                    if (debugScriptEngineFactory == null)
                        return new ErrorResponse("A Debug ScriptEngineFactory could not be located for the specified script engine type: '" + scriptEngineType + "'");

                    //TODO: Get a random unassigned port.
                    scriptEngine = debugScriptEngineFactory.GetDebugScriptEngine(5678);
                }
                else
                {
                    var scriptEngineFactory = m_kernel.TryGet<IScriptEngineFactory>(b => b.Get<string>("Espresso-Script-Engine-Type") == scriptEngineType);
                    if (scriptEngineFactory == null)
                        return new ErrorResponse("A ScriptEngineFactory could not be located for the specified script engine type: '" + scriptEngineType + "'");

                    scriptEngine = scriptEngineFactory.GetScriptEngine();
                }

                if (scriptEngine == null)
                    return new ErrorResponse("A IScriptEngine instance was not returned by the script engine factory associated with the request.");

                var barista = m_kernel.TryGet<IBarista>( b => b.Get<string>("Espresso-Api-Version-Number") == espressoApiVersionNumber, new IParameter[] { new ConstructorArgument("scriptEngine", scriptEngine) });
                //If we were unable to obtain an IBarista instance, return 400 response.
                if (barista == null)
                    return new ErrorResponse("Invalid Espresso version specified in 'Accept' header: '" + espressoApiVersionNumber + "'");

                return barista.Brew(ctx, results.Parameters["virtualScriptFilePath"]);
            });
        }
    }
}
