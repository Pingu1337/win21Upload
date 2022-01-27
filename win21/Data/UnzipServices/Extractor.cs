using System.IO.Compression;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using win21.Data.UploadServices;

namespace win21.Data.UnzipServices
{
    public class Extractor
    {
        private readonly FileTransferService _FileTransferService;
        private readonly IWebHostEnvironment _Environment;

        public Extractor(FileTransferService fileTransferService, IWebHostEnvironment environment)
        {
            _FileTransferService = fileTransferService;
            _Environment = environment;
        }
        public async Task Extract(string zipPath, string deletePath, string id, string name)
        {

            var path = Path.Combine("wwwroot/data",
                _Environment.EnvironmentName, "unsafe_uploads","users",id);

            try
            {
                string userID = id;
                string repo = name;
                ZipFile.ExtractToDirectory(zipPath, path, true);

                await _FileTransferService.recursiveDirectory(path, userID, repo);

                Directory.Delete(path, true);
                Directory.Delete(deletePath, true);
                if (!Directory.Exists(path) && !Directory.Exists(deletePath))
                {

                }
                else
                {

                }

            }
            catch (Exception e)
            {

            }

        }
    }
}
