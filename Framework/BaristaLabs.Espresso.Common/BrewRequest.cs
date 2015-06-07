namespace BaristaLabs.Espresso.Common
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Concrete implementation of a brew request.
    /// </summary>
    public class BrewRequest : IBrewRequest
    {
        public IEnumerable<KeyValuePair<string, IEnumerable<string>>> Headers
        {
            get;
            set;
        }
        public Stream Body
        {
            get;
            set;
        }

        public string Method
        {
            get;
            set;
        }

        public string VirtualScriptFilePath
        {
            get;
            set;
        }

        public Uri Url
        {
            get;
            set;
        }
    }
}
