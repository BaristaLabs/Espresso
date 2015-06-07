namespace BaristaLabs.Espresso.Core
{
    using Common;

    using System;
    using System.IO;
    using Newtonsoft.Json;

    public class JsonNetSerializer : ISerializer
    {
        public Lazy<JsonSerializer> m_jsonSerializer = new Lazy<JsonSerializer>(() =>
            {
                var serializer = new JsonSerializer();
                serializer.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                serializer.Formatting = Formatting.Indented;

                return serializer;
            });

        public void Serialize(Stream outputStream, object obj)
        {
            using (StreamWriter writer = new StreamWriter(outputStream))
            {
                m_jsonSerializer.Value.Serialize(writer, obj);
            }
        }
    }
}
