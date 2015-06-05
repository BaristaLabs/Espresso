namespace BaristaLabs.Espresso.Core
{
    using System;
    using Nancy;
    using Ninject;

    /// <summary>
    /// Represents a factory which returns IBarista Instances that will process requests.
    /// </summary>
    public interface IBaristaFactory
    {
        /// <summary>
        /// Returns a concrete IBarista instance to process the request.
        /// </summary>
        /// <param name="ctx">The nancy context to use</param>
        /// <param name="version">The version of the Espresso path</param>
        /// <returns>An IBarista instance that will handle the request.</returns>
        IBarista AssignBarista(NancyContext ctx, string version);
    }

    public class BaristaFactory : IBaristaFactory
    {
        private readonly IKernel m_kernel;
        public BaristaFactory(IKernel kernel)
        {
            if (kernel == null)
                throw new ArgumentNullException("kernel");

            m_kernel = kernel;
        }

        public IBarista AssignBarista(NancyContext ctx, string version)
        {
            if (string.IsNullOrWhiteSpace(version))
                throw new ArgumentNullException("version");

            var baristaInstance = m_kernel.TryGet<IBarista>(version);
            return baristaInstance;
        }
    }
}
