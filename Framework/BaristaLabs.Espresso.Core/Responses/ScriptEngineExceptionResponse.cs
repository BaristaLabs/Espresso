namespace BaristaLabs.Espresso.Core.Responses
{
    using Common;
    using Nancy;
    using Nancy.Responses;

    public class ScriptEngineExceptionResponse : JsonResponse<IScriptEngineException>
    {
        public ScriptEngineExceptionResponse(IScriptEngineException ex)
            : base(ex, EspressoJsonNetSerializer.Default.Value)
        {
            StatusCode = HttpStatusCode.BadRequest;
        }
    }
}
