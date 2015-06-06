namespace BaristaLabs.Espresso.Engine.V8
{
    internal interface IScriptMarshalWrapper
    {
        V8ScriptEngine Engine { get; }
        object Unwrap();
    }
}
