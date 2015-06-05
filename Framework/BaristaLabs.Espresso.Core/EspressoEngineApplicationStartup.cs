namespace BaristaLabs.Espresso.Core.v1
{
    using BaristaLabs.Espresso.Core.Responses;
    using Nancy.Bootstrapper;
    using Nancy.Routing;
    using System.Text.RegularExpressions;

    public class EspressoEngineApplicationStartup : IApplicationStartup
    {
        Regex espressoMediaTypeRegex = new Regex(@"application/vnd.espresso(\.(?<versionNumber>[0-9]*))?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private IBaristaFactory m_baristaFactory;
        public EspressoEngineApplicationStartup(IBaristaFactory baristaFactory)
        {
            m_baristaFactory = baristaFactory;
        }

        public void Initialize(IPipelines pipelines)
        {
            //Add the generic API route that handles any requests to the pipeline.
            pipelines.BeforeRequest.AddItemToStartOfPipeline(ctx =>
            {
                var rpm = new DefaultRoutePatternMatcher();
                var results = rpm.Match(ctx.Request.Url.Path, @"/api/{virtualScriptFilePath?}", new[] { "api", "{virtualScriptFilePath?}" }, ctx);

                //If we didn't match the "API" route, let something else handle the request.
                if (!results.IsMatch)
                    return null;

                //use espresso media type.
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

                var barista = m_baristaFactory.AssignBarista(ctx, "v" + espressoApiVersionNumber);

                //If we were unable to obtain an IBarista instance, return 400 response.
                if (barista == null)
                {
                    return new ErrorResponse("Invalid Espresso version specified in 'Accept' header: '" + espressoApiVersionNumber + "'");
                }

                return barista.Brew(ctx, results.Parameters["virtualScriptFilePath"]);
            });
        }
    }
}
