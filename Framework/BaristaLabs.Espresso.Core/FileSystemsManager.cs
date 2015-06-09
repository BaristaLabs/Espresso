namespace BaristaLabs.Espresso.Core
{
    using BaristaLabs.Espresso.FileSystem;
    using System;
    using System.Collections.Generic;

    public class FileSystemsManager
    {
        private readonly Dictionary<string, IFileSystem> m_fileSystems = new Dictionary<string, IFileSystem>();
        public FileSystemsManager(ICollection<IFileSystem> allFileSystems)
        {
            if (allFileSystems == null || allFileSystems.Count == 0)
                throw new ArgumentNullException("allFileSystems", "At least one file system implementation must be provided.");

            foreach (var fileSystem in allFileSystems)
            {
                if (m_fileSystems.ContainsKey(fileSystem.Prefix.ToLowerInvariant()))
                    throw new InvalidOperationException("All file system instances added to the FileSystem service must have a unique prefix.");

                m_fileSystems.Add(fileSystem.Prefix.ToLowerInvariant(), fileSystem);
            }
        }

        public IFileSystem GetFileSystemByPrefix(string prefix)
        {
            if (String.IsNullOrWhiteSpace(prefix))
                throw new ArgumentNullException("prefix");

            if (m_fileSystems.ContainsKey(prefix.ToLowerInvariant()) == false)
                throw new InvalidOperationException("A file system with the specified prefix does not exist.");

            return m_fileSystems[prefix.ToLowerInvariant()];
        }
    }
}
