namespace BaristaLabs.Espresso.Engine.V8
{
    using BaristaLabs.Espresso.Common;

    internal interface IHostInvokeContext
    {
        V8ScriptEngine Engine { get; }
        ScriptAccess DefaultAccess { get; }
    }
}
