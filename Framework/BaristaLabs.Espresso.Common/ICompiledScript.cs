namespace BaristaLabs.Espresso.Common
{
    using System;

    /// <summary>
    /// Represents a compiled script that can be executed multiple times without recompilation.
    /// </summary>
    public interface ICompiledScript : IDisposable
    {
        string Name
        {
            get;
        }
    }
}
