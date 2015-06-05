namespace BaristaLabs.Espresso.Core
{
    using Common;
    using Nancy;
    using System;

    public class DefaultBarista : IBarista
    {
        private IJavaScriptEngine m_scriptEngine;

        public DefaultBarista(IJavaScriptEngine scriptEngine)
        {
            if (scriptEngine == null)
                throw new ArgumentNullException("scriptEngine");

            m_scriptEngine = scriptEngine;
        }

        public Response Brew(NancyContext ctx)
        {
            throw new NotImplementedException();
        }
    }
}
