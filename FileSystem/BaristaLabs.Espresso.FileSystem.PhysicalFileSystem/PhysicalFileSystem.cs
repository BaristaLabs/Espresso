namespace BaristaLabs.Espresso.FileSystem
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a IFileSystem implementation using the local physical file system.
    /// </summary>
    public class PhysicalFileSystem : IFileSystem
    {
        // These are restricted file names on Windows, regardless of extension.
        private static readonly Dictionary<string, string> RestrictedFileNames =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                {"con", string.Empty},
                {"prn", string.Empty},
                {"aux", string.Empty},
                {"nul", string.Empty},
                {"com1", string.Empty},
                {"com2", string.Empty},
                {"com3", string.Empty},
                {"com4", string.Empty},
                {"com5", string.Empty},
                {"com6", string.Empty},
                {"com7", string.Empty},
                {"com8", string.Empty},
                {"com9", string.Empty},
                {"lpt1", string.Empty},
                {"lpt2", string.Empty},
                {"lpt3", string.Empty},
                {"lpt4", string.Empty},
                {"lpt5", string.Empty},
                {"lpt6", string.Empty},
                {"lpt7", string.Empty},
                {"lpt8", string.Empty},
                {"lpt9", string.Empty},
                {"clock$", string.Empty},
            };

        /// <summary>
        /// Creates a new instance of a PhysicalFileSystem at the given root directory.
        /// </summary>
        /// <param name="root">The root directory</param>
        public PhysicalFileSystem(string root)
        {
            Root = GetFullRoot(root);
            if (!Directory.Exists(Root))
            {
                throw new DirectoryNotFoundException(Root);
            }
        }

        public string Prefix
        {
            get { return "local"; }
        }

        /// <summary>
        /// The root directory for this instance.
        /// </summary>
        public string Root
        {
            get;
            private set;
        }

        private static string GetFullRoot(string root)
        {
            var applicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var fullRoot = Path.GetFullPath(Path.Combine(applicationBase, root));
            if (!fullRoot.EndsWith(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal))
            {
                // When we do matches in GetFullPath, we want to only match full directory names.
                fullRoot += Path.DirectorySeparatorChar;
            }
            return fullRoot;
        }

        private string GetFullPath(string path)
        {
            var fullPath = Path.GetFullPath(Path.Combine(Root, path));
            return !fullPath.StartsWith(Root, StringComparison.OrdinalIgnoreCase)
                ? null
                : fullPath;
        }

        /// <summary>
        /// Locate a directory at the given path.
        /// </summary>
        /// <param name="subpath"></param>
        /// <returns></returns>
        public Task<IDirectoryInfo> GetDirectoryInfoAsync(string subpath)
        {
            if (String.IsNullOrWhiteSpace(subpath))
                return Task.FromResult<IDirectoryInfo>(null);

            try
            {
                if (subpath.StartsWith("/", StringComparison.Ordinal))
                {
                    subpath = subpath.Substring(1);
                }
                var fullPath = GetFullPath(subpath);
                if (fullPath != null)
                {
                    var directoryInfo = new DirectoryInfo(fullPath);
                    IDirectoryInfo fileInfo = new PhysicalDirectoryInfo(directoryInfo);
                    return Task.FromResult(fileInfo);
                }
            }
            catch (ArgumentException)
            {
            }
            catch (DirectoryNotFoundException)
            {
            }
            catch (IOException)
            {
            }

            return Task.FromResult<IDirectoryInfo>(null);
        }

        /// <summary>
        /// Locate a file at the given path.
        /// </summary>
        /// <param name="subpath">A path under the root directory</param>
        /// <returns>True if a file was discovered at the given path</returns>
        public Task<IFileInfo> GetFileInfoAsync(string subpath)
        {
            if (String.IsNullOrWhiteSpace(subpath))
                return Task.FromResult<IFileInfo>(null);

            try
            {
                if (subpath.StartsWith("/", StringComparison.Ordinal))
                {
                    subpath = subpath.Substring(1);
                }
                var fullPath = GetFullPath(subpath);
                if (fullPath != null)
                {
                    var info = new FileInfo(fullPath);
                    if (!IsRestricted(info))
                    {
                        IFileInfo fileInfo = new PhysicalFileInfo(info);
                        return Task.FromResult(fileInfo);
                    }
                }
            }
            catch (ArgumentException)
            {
            }

            return Task.FromResult<IFileInfo>(null);
        }

        private static bool IsRestricted(FileSystemInfo fileInfo)
        {
            string fileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
            return RestrictedFileNames.ContainsKey(fileName);
        }

        #region Nested Classes
        private class PhysicalDirectoryInfo : IDirectoryInfo
        {
            private readonly DirectoryInfo m_info;

            public PhysicalDirectoryInfo(DirectoryInfo info)
            {
                if (m_info == null)
                    throw new ArgumentNullException("info");

                m_info = info;
            }

            public bool Exists
            {
                get { return m_info.Exists; }
            }

            public long Length
            {
                get { return -1; }
            }

            public string FullPath
            {
                get { return m_info.FullName; }
            }

            public string Name
            {
                get { return m_info.Name; }
            }

            public DateTimeOffset Created
            {
                get { return m_info.CreationTimeUtc; }
            }

            public DateTimeOffset LastModified
            {
                get { return m_info.LastWriteTimeUtc; }
            }

            public Task<IEnumerable<IDirectoryInfo>> GetDirectoriesAsync()
            {
                if (!m_info.Exists)
                    return null;

                var physicalInfos = m_info.GetDirectories();
                var virtualInfos = new IDirectoryInfo[physicalInfos.Length];
                for (var index = 0; index != physicalInfos.Length; ++index)
                {
                    virtualInfos[index] = new PhysicalDirectoryInfo(physicalInfos[index]);
                }
                return Task.FromResult(virtualInfos.AsEnumerable());
            }

            public Task<IEnumerable<IFileInfo>> GetFilesAsync()
            {
                if (!m_info.Exists)
                    return null;

                var physicalInfos = m_info.GetFiles();
                var virtualInfos = new IFileInfo[physicalInfos.Length];
                for (var index = 0; index != physicalInfos.Length; ++index)
                {
                    virtualInfos[index] = new PhysicalFileInfo(physicalInfos[index]);
                }
                return Task.FromResult(virtualInfos.AsEnumerable());
            }
        }

        private class PhysicalFileInfo : IFileInfo
        {
            private readonly FileInfo m_info;

            public PhysicalFileInfo(FileInfo info)
            {
                if (info == null)
                    throw new ArgumentNullException("info");

                m_info = info;
            }

            public bool Exists
            {
                get { return m_info.Exists; }
            }

            public long Length
            {
                get { return m_info.Length; }
            }

            public string FullPath
            {
                get { return m_info.FullName; }
            }

            public string Name
            {
                get { return m_info.Name; }
            }

            public DateTimeOffset Created
            {
                get { return m_info.CreationTimeUtc; }
            }

            public DateTimeOffset LastModified
            {
                get { return m_info.LastWriteTimeUtc; }
            }

            public Stream Get()
            {
                // Note: Buffer size must be greater than zero, even if the file size is zero.
                var fs = new FileStream(m_info.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1024*64,
                    FileOptions.Asynchronous | FileOptions.SequentialScan);

                return fs;
            }

        }
        #endregion
    }
}
