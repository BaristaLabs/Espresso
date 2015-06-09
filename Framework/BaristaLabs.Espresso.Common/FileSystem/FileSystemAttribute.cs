namespace BaristaLabs.Espresso.FileSystem
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class FileSystemAttribute : Attribute
    {
        public FileSystemAttribute(string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                throw new ArgumentNullException("prefix", "A prefix must be specified.");

            Prefix = prefix;
        }

        public string Prefix
        {
            get;
            set;
        }
    }
}
