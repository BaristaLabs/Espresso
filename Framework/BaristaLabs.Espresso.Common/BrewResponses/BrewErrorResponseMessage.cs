namespace BaristaLabs.Espresso.Common.BrewResponses
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class BrewErrorResponseMessage
    {
        public BrewErrorResponseMessage(string message)
        {
            Message = message;
        }

        public BrewErrorResponseMessage(string message, IEnumerable<string> errors)
        {
            Message = message;
            Errors = errors;
        }

        [JsonProperty("message")]
        public string Message
        {
            get;
            set;
        }

        [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<string> Errors
        {
            get;
            set;
        }
    }
}
