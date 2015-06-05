using System;

namespace BaristaLabs.Espresso.Common
{
    /// <summary>
    /// Represents an Espresso Application
    /// </summary>
    public interface IApplication
    {
        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum heap that the application is allowed to use.
        /// </summary>
        UIntPtr? MaxRuntimeHeapSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum stack that the application is allowed to use.
        /// </summary>
        UIntPtr? MaxRuntimeStackUsage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum execution time of the application.
        /// </summary>
        TimeSpan? MaxExecutionTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the virtual file system implementation that the application will use.
        /// </summary>
        /// <remarks>
        /// Various Virtual File System implementations might be PhysicalFileSystem, Github, SharePoint, etc...
        /// </remarks>
        string FileSystemImpl
        {
            get;
            set;
        }
    }
}
