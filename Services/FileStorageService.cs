using Azure.Storage.Files.Shares;
using System.IO;
using System.Threading.Tasks;

namespace azuresolution1.Services
{
    public class FileStorageService
    {
        private readonly ShareClient _shareClient;

        public FileStorageService(string shareName, string connectionString)
        {
            _shareClient = new ShareClient(connectionString, shareName);
            _shareClient.CreateIfNotExists();
        }

        public async Task UploadFileAsync(string fileName, Stream content)
        {
            var directoryClient = _shareClient.GetDirectoryClient("contracts");
            await directoryClient.CreateIfNotExistsAsync();
            var fileClient = directoryClient.GetFileClient(fileName);
            await fileClient.CreateAsync(content.Length);
            await fileClient.UploadAsync(content);
        }

        public async Task<Stream> DownloadFileAsync(string fileName)
        {
            var directoryClient = _shareClient.GetDirectoryClient("contracts");
            var fileClient = directoryClient.GetFileClient(fileName);
            var download = await fileClient.DownloadAsync();
            return download.Value.Content;
        }
    }
}