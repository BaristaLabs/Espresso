namespace BaristaLabs.Espresso.Core
{
    using Nancy;
    using NuGet;
    using System.Linq;

    public class PackageManagerModule : NancyModule
    {
        public PackageManagerModule()
        {
            Get["/packages/local"] = parameters =>
            {
                var repo = PackageRepositoryFactory.Default.CreateRepository("https://packages.nuget.org/api/v2");
                string path = ".\\packages";
                var packageManager = new PackageManager(repo, path);

                var pk = packageManager.LocalRepository.GetPackages();
                
                return new Nancy.Responses.JsonResponse(pk.Count(), EspressoJsonNetSerializer.Default.Value);
            };
        }
    }
}
