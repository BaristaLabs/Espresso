namespace BaristaLabs.Espresso.Common
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ScriptEngineFactoryAttribute : Attribute
    {
        public ScriptEngineFactoryAttribute(string scriptEngineType)
        {
            if (string.IsNullOrWhiteSpace(scriptEngineType))
                throw new ArgumentNullException("scriptEngineType", "A script engine type identifier must be specified.");

            ScriptEngineType = scriptEngineType;
        }

        public string ScriptEngineType
        {
            get;
            set;
        }
    }
}
