namespace BaristaLabs.Espresso.Common
{
    /// <summary>
    /// Represents an object that constructs a new instance of a IScriptEngine.
    /// </summary>
    public interface IScriptEngineFactory
    {
        /// <summary>
        /// Returns an instance of an IScriptEngine
        /// </summary>
        /// <returns></returns>
        IScriptEngine GetScriptEngine();
    }
}
