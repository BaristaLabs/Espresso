namespace BaristaLabs.Espresso.Common
{
    /// <summary>
    /// Represents an object that constructs a new instance of a IScriptEngine which has debugging enabled.
    /// </summary>
    public interface IDebugScriptEngineFactory
    {
        /// <summary>
        /// Creates and returns a new instance of an IScriptEngine
        /// </summary>
        /// <returns></returns>
        IScriptEngine GetDebugScriptEngine(int debugPort);
    }
}
