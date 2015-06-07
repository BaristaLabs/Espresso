namespace BaristaLabs.Espresso.Common
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;

    [Serializable]
    public class BrewResponse : IBrewResponse, IDisposable, IExtensibleDataObject

        {/// <summary>
         /// Null object representing no body
         /// </summary>
        public static Action<Stream> NoBody = s => { };

        private string m_contentType;

        /// <summary>
        /// Initializes a new instance of the <see cref="Response"/> class.
        /// </summary>
        public BrewResponse()
        {
            Contents = NoBody;
            ContentType = "application/json";
            Headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            StatusCode = 200;
            Cookies = new List<ICookie>(2);
            ExtendedProperties = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>The type of the content.</value>
        /// <remarks>The default value is <c>text/html</c>.</remarks>
        public string ContentType
        {
            get { return Headers.ContainsKey("content-type") ? Headers["content-type"] : m_contentType; }
            set { m_contentType = value; }
        }

        /// <summary>
        /// Gets the delegate that will render contents to the response stream.
        /// </summary>
        /// <value>An <see cref="Action{T}"/> delegate, containing the code that will render contents to the response stream.</value>
        /// <remarks>The host of Nancy will pass in the output stream after the response has been handed back to it by Nancy.</remarks>
        public Action<Stream> Contents
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the collection of HTTP response headers that should be sent back to the client.
        /// </summary>
        /// <value>An <see cref="IDictionary{TKey,TValue}"/> instance, containing the key/value pair of headers.</value>
        public IDictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Gets or sets the HTTP status code that should be sent back to the client.
        /// </summary>
        /// <value>A <see cref="HttpStatusCode"/> value.</value>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets a text description of the HTTP status code returned to the client.
        /// </summary>
        /// <value>The HTTP status code description.</value>
        public string ReasonPhrase
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the <see cref="ICookie"/> instances that are associated with the response.
        /// </summary>
        /// <value>A <see cref="IList{T}"/> instance, containing <see cref="ICookie"/> instances.</value>
        public IList<ICookie> Cookies
        {
            get;
            private set;
        }

        [DataMember]
        public IDictionary<string, string> ExtendedProperties
        {
            get;
            set;
        }

        [NonSerialized]
        private ExtensionDataObject m_extensionData;

        public ExtensionDataObject ExtensionData
        {
            get { return m_extensionData; }
            set { m_extensionData = value; }
        }

        /// <summary>
        /// Implicitly cast an int value to a <see cref="BrewResponse"/> instance, with the <see cref="StatusCode"/>
        /// set to the value of the int.
        /// </summary>
        /// <param name="statusCode">The int value that is being cast from.</param>
        /// <returns>A <see cref="BrewResponse"/> instance.</returns>
        public static implicit operator BrewResponse(int statusCode)
        {
            return new BrewResponse { StatusCode = statusCode };
        }

        /// <summary>
        /// Implicitly cast an string instance to a <see cref="Response"/> instance, with the <see cref="Contents"/>
        /// set to the value of the string.
        /// </summary>
        /// <param name="contents">The string that is being cast from.</param>
        /// <returns>A <see cref="BrewResponse"/> instance.</returns>
        public static implicit operator BrewResponse(string contents)
        {
            return new BrewResponse { Contents = GetStringContents(contents) };
        }

        /// <summary>
        /// Implicitly cast an <see cref="Action{T}"/>, where T is a <see cref="Stream"/>, instance to
        /// a <see cref="BrewResponse"/> instance, with the <see cref="Contents"/> set to the value of the action.
        /// </summary>
        /// <param name="streamFactory">The <see cref="Action{T}"/> instance that is being cast from.</param>
        /// <returns>A <see cref="BrewResponse"/> instance.</returns>
        public static implicit operator BrewResponse(Action<Stream> streamFactory)
        {
            return new BrewResponse { Contents = streamFactory };
        }

        /// <summary>
        /// Converts a string content value to a response action.
        /// </summary>
        /// <param name="contents">The string containing the content.</param>
        /// <returns>A response action that will write the content of the string to the response stream.</returns>
        protected static Action<Stream> GetStringContents(string contents)
        {
            return stream =>
            {
                var writer = new StreamWriter(stream) { AutoFlush = true };
                writer.Write(contents);
            };
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks>This method can be overridden in sub-classes to dispose of response specific resources.</remarks>
        public virtual void Dispose()
        {
        }
    }
}
