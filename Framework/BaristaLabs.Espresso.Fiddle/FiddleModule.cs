namespace BaristaLabs.Espresso.Fiddle
{
    using Nancy;

    /// <summary>
    /// Serve up some fiddle files
    /// </summary>
    public class FiddleModule : NancyModule
    {
        public FiddleModule()
        {
            //Add stuffs.
            Get["/fiddle/"] = p =>
            {
                string fiddleIndexPath = "/fiddle/index.html";
                return Response.AsRedirect(fiddleIndexPath);
            };

            Get["/fiddle/{file*}"] = p =>
            {
                string path = string.Format("Fiddle/{0}", p.file);
                return Response.AsFile(path);
            };
        }
    }
}
