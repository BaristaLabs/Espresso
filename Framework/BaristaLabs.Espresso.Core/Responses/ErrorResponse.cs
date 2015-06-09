namespace BaristaLabs.Espresso.Core.Responses
{
    using Nancy;
    using Nancy.Responses;
    using System.Collections.Generic;

    public class ErrorResponse : JsonResponse<ErrorResponseMessage>
    {
        public ErrorResponse(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            : base(new ErrorResponseMessage(message), EspressoJsonNetSerializer.Default.Value)
        {
            StatusCode = statusCode;
        }

        public ErrorResponse(string message, IEnumerable<string> errors)
            : base(new ErrorResponseMessage(message, errors), EspressoJsonNetSerializer.Default.Value)
        {
            StatusCode = HttpStatusCode.BadRequest;
        }
    }
}
