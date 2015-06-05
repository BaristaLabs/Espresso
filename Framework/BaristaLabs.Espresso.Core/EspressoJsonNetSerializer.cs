namespace BaristaLabs.Espresso.Core
{
    using Nancy.Serialization.JsonNet;
    using Newtonsoft.Json;
    using System;

    public class EspressoJsonNetSerializer : JsonNetSerializer
    {
        public static Lazy<JsonNetSerializer> Default = new Lazy<JsonNetSerializer>(() => new JsonNetSerializer());

        public EspressoJsonNetSerializer()
            : base(new JsonSerializer { Formatting = Formatting.Indented })
        {
        }
    }
}
