namespace BaristaLabs.Espresso.Common
{
    /// <summary>
    /// Represents the method that specifies to a script engine whether script execution should continue.
    /// </summary>
    /// <returns><c>True</c> to continue script execution, <c>false</c> to interrupt it.</returns>
    /// <seealso cref="IScriptEngine.ContinuationCallback"/>
    public delegate bool ContinuationCallback();
}
