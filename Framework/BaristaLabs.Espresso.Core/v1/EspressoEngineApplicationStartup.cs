namespace BaristaLabs.Espresso.Core.v1
{
    using Nancy.Bootstrapper;
    using System;

    public class EspressoEngineApplicationStartup : IApplicationStartup
    {
        private IBaristaFactory m_baristaFactory;
        public EspressoEngineApplicationStartup(IBaristaFactory baristaFactory)
        {
            m_baristaFactory = baristaFactory;
        }

        public void Initialize(IPipelines pipelines)
        {
            pipelines.BeforeRequest.AddItemToStartOfPipeline(ctx => {
                
                Console.WriteLine("Hi");
                var barista = m_baristaFactory.AssignBarista(ctx);
                return barista.Brew(ctx);
            });
        }
    }
}
