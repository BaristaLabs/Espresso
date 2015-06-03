namespace BaristaLabs.Espresso.Common
{
    /// <summary>
    /// Represents the source of a script to execute/evaluate.
    /// </summary>
    public interface IScriptSource
    {
        string GetScript();
    }
}
