namespace BaristaLabs.Espresso.Core
{
    using Common;
    using Ninject.Modules;
    using Ninject.Extensions.Conventions;
    using System.Linq;

    public class EspressoModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind(x => x
                    .FromThisAssembly()
                    .SelectAllClasses()
                    .InheritedFrom<IBarista>()
                    .BindAllInterfaces()
                    .Configure((c, baristaType) =>
                        {
                            var espressoApiAttribute = baristaType.GetCustomAttributes(false).OfType<EspressoApiAttribute>().FirstOrDefault();
                            if (espressoApiAttribute != null)
                                c.WithMetadata("Espresso-Api-Version-Number", espressoApiAttribute.Version);
                        })
                    );

            ///Bind any IScriptEngineFactory instances in the path.
            Kernel.Bind(x => x
                  .FromAssembliesInPath(".")
                  .SelectAllClasses()
                  .InheritedFrom<IScriptEngineFactory>()
                  .BindAllInterfaces()
                  .Configure( (c, scriptEngineFactoryType) =>
                      {
                          c.InSingletonScope();

                          var scriptEngineFactoryAttribute = scriptEngineFactoryType.GetCustomAttributes(false).OfType<ScriptEngineFactoryAttribute>().FirstOrDefault();
                          if (scriptEngineFactoryAttribute != null)
                              c.WithMetadata("Espresso-Script-Engine-Type", scriptEngineFactoryAttribute.ScriptEngineType);
                      })
                  );

            //Bind any IDebugScriptEngineFactory instances in the path.
            Kernel.Bind(x => x
                  .FromAssembliesInPath(".")
                  .SelectAllClasses()
                  .InheritedFrom<IDebugScriptEngineFactory>()
                  .BindAllInterfaces()
                  .Configure((c, scriptEngineFactoryType) =>
                      {
                          c.InSingletonScope();

                          var scriptEngineFactoryAttribute = scriptEngineFactoryType.GetCustomAttributes(false).OfType<ScriptEngineFactoryAttribute>().FirstOrDefault();
                          if (scriptEngineFactoryAttribute != null)
                              c.WithMetadata("Espresso-Script-Engine-Type", scriptEngineFactoryAttribute.ScriptEngineType);
                      })
                  );
        }
    }
}
