namespace EspressoHost
{
    using BaristaLabs.Espresso.Core;
    using Nancy.Bootstrappers.Ninject;
    using Ninject;

    public class BaristaLabsHostBootstrapper : NinjectNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(IKernel existingContainer)
        {
            base.ConfigureApplicationContainer(existingContainer);
            existingContainer.Load(new EspressoModule());
        }

        protected override IKernel GetApplicationContainer()
        {
            return base.GetApplicationContainer();
            //return _kernel;
        }
    }
}
