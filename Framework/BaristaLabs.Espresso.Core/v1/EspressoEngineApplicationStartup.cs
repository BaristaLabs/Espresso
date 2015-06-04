namespace BaristaLabs.Espresso.Core.v1
{
    using Nancy.Bootstrapper;
    using System;

    public class EspressoEngineApplicationStartup : IApplicationStartup
    {
        //private IFooFactory fooFactory;
        //public EspressoEngineApplicationStartup(IFooFactory fooFactory)
        //{
        //    this.fooFactory = fooFactory;
        //}

        public void Initialize(IPipelines pipelines)
        {
            pipelines.BeforeRequest.AddItemToStartOfPipeline(ctx => {
                //var foo = fooFactory.Make();
                //foo.Bar(ctx);
                Console.WriteLine("Hi");
                return null;
            });
        }
    }
}
