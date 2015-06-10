namespace BaristaLabs.Espresso.Core
{
    using Responses;
    using Nancy;
    using Ninject;

    /// <summary>
    /// Module used to interact with IFileSystem implementation.
    /// </summary>
    public class FileSystemModule : NancyModule
    {
        public FileSystemModule(IKernel kernel)
        {
            Get["/files/{fileName*}", true] = async (parameters, ct) =>
            {
                string subpath = parameters.fileName;
                var fsm = kernel.Get<FileSystemsManager>();

                //TODO: Change this to use configuration from the app...
                var fs = fsm.GetFileSystemByPrefix("local", ".\\API");

                var fileInfo = await fs.GetFileInfoAsync(subpath);
                if (fileInfo.Exists == false)
                    return new ErrorResponse("A file does not exist in that location.", HttpStatusCode.NotFound);

                return new Nancy.Responses.StreamResponse(() => fileInfo.Get(), "application/octect-stream");
            };
        }
    }
}
