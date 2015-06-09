namespace BaristaLabs.Espresso.Core
{
    using Common;
    using FileSystem;

    using Ninject.Modules;
    using Ninject.Extensions.Conventions;
    using System.Linq;
    using System;

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
                  .FromAssembliesInPath(@".\Engine\")
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
                  .FromAssembliesInPath(@".\Engine\")
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

            //Bind FileSystem related stuff
            Kernel.Bind(x => x
                .FromAssembliesInPath(@".\FileSystem\")
                .SelectAllClasses()
                .InheritedFrom<IFileSystem>()
                .BindAllInterfaces()
                .Configure((c, fileSystemType) =>
                    {
                        var fileSystemAttribute = fileSystemType.GetCustomAttributes(false).OfType<FileSystemAttribute>().FirstOrDefault();
                        if (fileSystemAttribute == null)
                            throw new InvalidOperationException("Concrete implementations of IFileSystem must be decorated with a FileSystemAttribute. " + fileSystemType);

                        c.WithMetadata("Espresso-File-System-Prefix", fileSystemAttribute.Prefix);
                    })
                );

            Kernel.Bind<FileSystemsManager>()
                .ToSelf()
                .InSingletonScope();
        }
    }
}
