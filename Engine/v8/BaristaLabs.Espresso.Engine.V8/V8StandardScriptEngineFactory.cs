namespace BaristaLabs.Espresso.Engine.V8
{
    using Common;

    [ScriptEngineFactory("V8")]
    public class V8StandardScriptEngineFactory : IScriptEngineFactory
    {
        public IScriptEngine GetScriptEngine()
        {
            return new V8ScriptEngine(V8ScriptEngineFlags.DisableGlobalMembers);
        }
    }

    [ScriptEngineFactory("V8")]
    public class V8StandardDebugScriptEngineFactory : IDebugScriptEngineFactory
    {
        public IScriptEngine GetDebugScriptEngine(int debugPort)
        {
            return new V8ScriptEngine(V8ScriptEngineFlags.DisableGlobalMembers | V8ScriptEngineFlags.EnableDebugging, debugPort);
        }
    }
}
