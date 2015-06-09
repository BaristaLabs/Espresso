namespace BaristaLabs.Espresso.FileSystem
{
    using System;

    public interface IFileSystemObject
    {/// <summary>
        /// Gets a value that indicates if the file exists.
        /// </summary>
        bool Exists
        {
            get;
        }

        /// <summary>
        /// The length of the file in bytes.
        /// </summary>
        long Length
        {
            get;
        }

        /// <summary>
        /// The path to the file, including the file name. Relative path is returned if root is relative.
        /// </summary>
        string FullPath
        {
            get;
        }

        /// <summary>
        /// The name of the file
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// Gets a value that indicates when the file system object was created.
        /// </summary>
        DateTimeOffset Created
        {
            get;
        }

        /// <summary>
        /// When the file system object was last modified
        /// </summary>
        DateTimeOffset LastModified
        {
            get;
        }
    }
}
