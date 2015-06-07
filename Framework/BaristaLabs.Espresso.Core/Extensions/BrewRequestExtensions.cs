namespace BaristaLabs.Espresso.Core.Extensions
{
    using Common;
    using System;
    using Nancy;

    public static class BrewRequestExtensions
    {
        public static void PopulateFromNancyRequest(this BrewRequest brewRequest, Request request)
        {
            if (brewRequest == null)
                throw new ArgumentNullException("brewRequest");

            if (request == null)
                throw new ArgumentNullException("request");

            brewRequest.Headers = request.Headers;
            brewRequest.Method = request.Method;
            brewRequest.Url = request.Url;
            brewRequest.Body = request.Body;
        }
    }
}
