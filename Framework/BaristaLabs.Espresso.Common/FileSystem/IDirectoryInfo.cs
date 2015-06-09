namespace BaristaLabs.Espresso.FileSystem
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDirectoryInfo : IFileSystemObject
    {
        Task<IEnumerable<IDirectoryInfo>> GetDirectoriesAsync();

        Task<IEnumerable<IFileInfo>> GetFilesAsync();
    }
}
