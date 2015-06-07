namespace BaristaLabs.Espresso.Common.BrewResponses
{
    using Common;

    public class ScriptEngineExceptionResponse : JsonBrewResponse<IScriptEngineException>
    {
        public ScriptEngineExceptionResponse(IScriptEngineException ex)
            : base(ex)
        {
        }
    }
}
