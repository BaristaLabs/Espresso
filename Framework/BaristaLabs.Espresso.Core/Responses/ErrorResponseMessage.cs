namespace BaristaLabs.Espresso.Core.Responses
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class ErrorResponseMessage
    {
        public ErrorResponseMessage(string message)
        {
            Message = message;
        }

        public ErrorResponseMessage(string message, IEnumerable<string> errors)
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
