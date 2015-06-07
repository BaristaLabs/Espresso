namespace BaristaLabs.Espresso.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;

    public interface IBrewRequest
    {
        IEnumerable<KeyValuePair<string, IEnumerable<string>>> Headers
        {
            get;
        }

        Stream Body
        {
            get;
        }

        string Method
        {
            get;
        }

        Uri Url
        {
            get;
        }

        string VirtualScriptFilePath
        {
            get;
        }
    }
}
