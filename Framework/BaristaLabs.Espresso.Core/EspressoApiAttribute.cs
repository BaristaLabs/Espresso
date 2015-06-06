namespace BaristaLabs.Espresso.Core
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class EspressoApiAttribute : Attribute
    {
        public EspressoApiAttribute(string version)
        {
            if (string.IsNullOrWhiteSpace(version))
                throw new ArgumentNullException("version", "A version identifier must be specified.");

            Version = version;
        }
        
        public string Version
        {
            get;
            set;
        }
    }
}
