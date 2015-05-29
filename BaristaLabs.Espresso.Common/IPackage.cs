namespace BaristaLabs.Espresso.Common
{
    public interface IPackage
    {
        /// <summary>
        /// Gets the name of the package.
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// Gets description of the package (optional).
        /// </summary>
        string Description
        {
            get;
        }

        /// <summary>
        /// Gets the symantic version of the package.
        /// </summary>
        string Version
        {
            get;
        }

        /// <summary>
        /// Installs the package into the specified context, optionally returning a result.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        IJavaScriptObject InstallPackage(IJavaScriptContext context);
    }
}
