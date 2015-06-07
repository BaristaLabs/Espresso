namespace BaristaLabs.Espresso.Core.v1
{
    using Common;
    using Extensions;
    using Responses;

    using Nancy.Bootstrapper;
    using Nancy.Routing;
    using Ninject;
    using System.Text.RegularExpressions;
    using System;
    using Nancy;

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
                var matchResult = rpm.Match(ctx.Request.Url.Path, @"/api/{virtualScriptFilePath?}", new[] { "api", "{virtualScriptFilePath?}" }, ctx);

                //If we didn't match the "API" route, let something else handle the request and get outta dodge.
                if (!matchResult.IsMatch)
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

                var barista = GetBaristaInstance(espressoApiVersionNumber);

                //If we were unable to obtain an IBarista instance, return 400 response.
                if (barista == null)
                    return new ErrorResponse("Unable to obtain an IBarista Instance. Ensure that a valid Espresso version specified in 'Accept' header: '" + espressoApiVersionNumber + "'");

                var request = GetBrewRequest(ctx, matchResult);

                if (request == null)
                    return new ErrorResponse("Unable to obtain an IBrewRequest.");

                var brewResponse = barista.Brew(request);

                var response = new Response();
                response.ContentType = brewResponse.ContentType;
                response.StatusCode = (HttpStatusCode)brewResponse.StatusCode;
                response.Contents = brewResponse.Contents;
                return response;
            });
        }

        protected virtual IBarista GetBaristaInstance(string espressoApiVersionNumber)
        {
            return m_kernel.TryGet<IBarista>(b => b.Get<string>("Espresso-Api-Version-Number") == espressoApiVersionNumber);
        }

        protected virtual IBrewRequest GetBrewRequest(NancyContext ctx, IRoutePatternMatchResult matchResult)
        {
            var request = new BrewRequest()
            {
                VirtualScriptFilePath = matchResult.Parameters["virtualScriptFilePath"]
            };
            request.PopulateFromNancyRequest(ctx.Request);

            return request;
        }
    }
}
