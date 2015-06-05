namespace BaristaLabs.Espresso.Core
{
    using System;
    using Nancy;
    using Ninject;

    public interface IBaristaFactory
    {
        /// <summary>
        /// Returns a concrete IBarista instance to process the request.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        IBarista AssignBarista(NancyContext ctx);
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

        public IBarista AssignBarista(NancyContext ctx)
        {
            //TODO: Parse the context.request.url and get the app associated with it using IoC.
            //Should be a way to do this similar to how Get('url') does it.

            return m_kernel.Get<IBarista>();
        }
    }
}
