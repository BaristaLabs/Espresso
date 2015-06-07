namespace BaristaLabs.Espresso.Common.BrewResponses
{
    using System;
    using System.IO;

    public class JsonBrewResponse<TModel> : BrewResponse
    {
        public JsonBrewResponse(TModel model)
        {
            Contents = model == null ? NoBody : GetJsonContents(model);
            ContentType = DefaultContentType;
            StatusCode = 200;
        }

        private ISerializer m_serializer;
        public ISerializer JsonSerializer
        {
            get
            {
                if (m_serializer == null)
                    return JsonSettings.DefaultJsonSerializer;
                return m_serializer;
            }
            set
            {
                m_serializer = value;
            }
        }

        private static string DefaultContentType
        {
            get { return "application/json; charset=utf-8"; }
        }

        private Action<Stream> GetJsonContents(TModel model)
        {
            return stream => JsonSerializer.Serialize(stream, model);
        }
    }

    public class JsonBrewResponse : JsonBrewResponse<object>
    {
        public JsonBrewResponse(object model)
            : base(model)
        {
        }
    }
}
