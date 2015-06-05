namespace BaristaLabs.Espresso.Common
{
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// Defines common script engine exception properties.
    /// </summary>
    public interface IScriptEngineException
    {
        /// <summary>
        /// Gets the error message.
        /// </summary>
        [JsonProperty("message")]
        string Message
        {
            get;
        }

        /// <summary>
        /// Gets an <see href="http://en.wikipedia.org/wiki/HRESULT">HRESULT</see> error code if one is available, zero otherwise.
        /// </summary>
        [JsonProperty("hresult")]
        int HResult
        {
            get;
        }

        /// <summary>
        /// Gets the name associated with the script engine instance.
        /// </summary>
        [JsonProperty("engineName")]
        string EngineName
        {
            get;
        }

        /// <summary>
        /// Gets a detailed error message if one is available, <c>null</c> otherwise.
        /// </summary>
        [JsonProperty("errorDetails")]
        string ErrorDetails
        {
            get;
        }

        /// <summary>
        /// Gets a value that indicates whether the exception represents a fatal error.
        /// </summary>
        [JsonProperty("isFatal")]
        bool IsFatal
        {
            get;
        }

        /// <summary>
        /// Gets the exception that caused the current exception to be thrown, or <c>null</c> if one was not specified.
        /// </summary>
        [JsonProperty("innerException")]
        Exception InnerException
        {
            get;
        }
    }
}
