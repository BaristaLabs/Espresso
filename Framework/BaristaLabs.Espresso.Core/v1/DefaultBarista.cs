namespace BaristaLabs.Espresso.Core.v1
{
    using BaristaLabs.Espresso.Core.Responses;
    using Common;
    using Nancy;
    using Nancy.Responses;
    using System;

    /// <summary>
    /// Represents a default barista implementation.
    /// </summary>
    public class DefaultBarista : IBarista
    {
        private IJavaScriptEngine m_scriptEngine;

        public DefaultBarista(IJavaScriptEngine scriptEngine)
        {
            if (scriptEngine == null)
                throw new ArgumentNullException("scriptEngine");

            m_scriptEngine = scriptEngine;
        }

        public Response Brew(NancyContext ctx, string virtualScriptFilePath)
        {
            //TODO: Somehow inject all packages that are in the /packages folder, and install them on the script engine.
            //TODO: Get the FileSystem implementation from the application.
            //TODO: Set the script engine limits based on the application.

            try
            {
                var result = m_scriptEngine.Evaluate(virtualScriptFilePath, "1+1");
                return new JsonResponse(result, EspressoJsonNetSerializer.Default.Value);
            }
            catch(Exception ex)
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
