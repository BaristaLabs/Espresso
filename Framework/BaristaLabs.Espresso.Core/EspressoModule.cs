namespace BaristaLabs.Espresso.Core
{
    using Common;

    using Ninject.Modules;
    using Ninject.Extensions.Conventions;

    public class EspressoModule : NinjectModule
    {
        public override void Load()
        {

            Bind<IBaristaFactory>()
                .To<BaristaFactory>()
                .InSingletonScope();

            Bind<IBarista>()
                .To<v1.DefaultBarista>()
                .Named("v1");

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
