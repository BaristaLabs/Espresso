namespace EspressoHost
{
    using BaristaLabs.Espresso.Core;
    using Nancy.Hosting.Self;
    using Ninject;
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            var baseUri = new Uri("http://localhost:1234");

            using (var host = new NancyHost(baseUri))
            {
                host.Start();
                Console.WriteLine("Espresso listening on " + baseUri);
                Console.ReadLine();
            }
        }
    }
}
