namespace BaristaLabs.Espresso.Common
{
    using System.Runtime.InteropServices.Expando;

    /// <summary>
    /// Represents a JavaScript object.
    /// </summary>
    public interface IJavaScriptObject : IExpando, IDynamic
    {
    }
}
