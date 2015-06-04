namespace BaristaLabsHost
{
    using Nancy.Hosting.Self;
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new NancyHost(new Uri("http://localhost:1234")))
            {
                host.Start();
                Console.ReadLine();
            }
        }
    }
}
