namespace BaristaLabs.Espresso.FileSystem
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFileSystem
    {
        string Prefix
        {
            get;
        }

        /// <summary>
        /// Enumerate a directory at the given path, if any
        /// </summary>
        /// <param name="subpath">The path that identifies the directory</param>
        /// <returns>True if a directory was located at the given path</returns>
        Task<IDirectoryInfo>  GetDirectoryInfoAsync(string subpath);

        /// <summary>
        /// Locate a file at the given path
        /// </summary>
        /// <param name="subpath">The path that identifies the file</param>
        /// <returns>True if a file was located at the given path</returns>
        Task<IFileInfo> GetFileInfoAsync(string subpath);
    }
}
