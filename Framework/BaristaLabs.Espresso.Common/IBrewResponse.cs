namespace BaristaLabs.Espresso.Common
{
    using System;
    using System.IO;

    public interface IBrewResponse
    {
        int StatusCode
        {
            get;
            set;
        }

        string ContentType
        {
            get;
            set;
        }

        Action<Stream> Contents
        {
            get;
            set;
        }
    }
}
