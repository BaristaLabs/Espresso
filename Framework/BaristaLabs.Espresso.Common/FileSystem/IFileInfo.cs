namespace BaristaLabs.Espresso.FileSystem
{
    using System.IO;

    /// <summary>
    /// Represents a file in the given file system.
    /// </summary>
    public interface IFileInfo : IFileSystemObject
    {
        /// <summary>
        /// Return file contents as readonly stream. Caller should dispose stream when complete.
        /// </summary>
        /// <returns>The file stream</returns>
        Stream Get();
    }
}
