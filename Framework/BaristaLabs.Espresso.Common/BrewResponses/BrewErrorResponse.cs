namespace BaristaLabs.Espresso.Common.BrewResponses
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a Brew Response that is used when there is an error.
    /// </summary>
    public class BrewErrorResponse : JsonBrewResponse<BrewErrorResponseMessage>
    {
        public BrewErrorResponse(string message)
            : base(new BrewErrorResponseMessage(message))
        {
            StatusCode = 400;
        }

        public BrewErrorResponse(string message, IEnumerable<string> errors)
            : base(new BrewErrorResponseMessage(message, errors))
        {
            StatusCode = 400;
        }
    }
}
