namespace BaristaLabs.Espresso.Common
{
    public interface IRuntimeHeapInfo
    {
        ulong TotalHeapSize
        {
            get;
        }

        /// <summary>
        /// Gets the total executable heap size in bytes.
        /// </summary>
        ulong TotalHeapSizeExecutable
        {
            get;
        }

        /// <summary>
        /// Gets the total physical memory size in bytes.
        /// </summary>
        ulong TotalPhysicalSize
        {
            get;
        }

        /// <summary>
        /// Gets the used heap size in bytes.
        /// </summary>
        ulong UsedHeapSize
        {
            get;
        }

        /// <summary>
        /// Gets the heap size limit in bytes.
        /// </summary>
        ulong HeapSizeLimit
        {
            get;
        }
    }
}
