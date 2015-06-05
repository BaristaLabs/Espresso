namespace BaristaLabs.Espresso.Core
{
    using Common;

    using Ninject.Modules;
    using Ninject.Extensions.Conventions;

    public class EspressoModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IBarista>().To<DefaultBarista>();

            Bind<IBaristaFactory>().To<BaristaFactory>();

            ///Bind any IJavaScriptEngine instances in the path.
            Kernel.Bind(x => x
                  .FromAssembliesInPath(".")
                  .SelectAllClasses()
                  .InheritedFrom<IJavaScriptEngine>()
                  .BindAllInterfaces()
                  );
        }
    }
}
