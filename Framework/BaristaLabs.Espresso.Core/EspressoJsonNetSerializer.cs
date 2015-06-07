namespace BaristaLabs.Espresso.Core
{
    using Newtonsoft.Json;
    using System;
    using JsonNetSerializer = Nancy.Serialization.JsonNet.JsonNetSerializer;

    public class EspressoJsonNetSerializer : Nancy.Serialization.JsonNet.JsonNetSerializer
    {
        public static Lazy<Nancy.Serialization.JsonNet.JsonNetSerializer> Default = new Lazy<Nancy.Serialization.JsonNet.JsonNetSerializer>(() => new Nancy.Serialization.JsonNet.JsonNetSerializer());

        public EspressoJsonNetSerializer()
            : base(new JsonSerializer { Formatting = Formatting.Indented })
        {
        }
    }
}
