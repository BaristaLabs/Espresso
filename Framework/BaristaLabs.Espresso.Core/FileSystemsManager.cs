namespace BaristaLabs.Espresso.Core
{
    using FileSystem;
    using Ninject;
    using Ninject.Parameters;
    using System;

    public class FileSystemsManager
    {
        private IKernel m_kernel;

        public FileSystemsManager(IKernel kernel)
        {
            if (kernel == null)
                throw new ArgumentNullException("kernel");

            m_kernel = kernel;
        }

        public IFileSystem GetFileSystemByPrefix(string prefix, string rootPath)
        {
            var fileSystem = m_kernel.Get<IFileSystem>(md => md.Get<string>("Espresso-File-System-Prefix") == prefix, new[] { new ConstructorArgument("root", rootPath) });

            return fileSystem;
        }
    }
}
