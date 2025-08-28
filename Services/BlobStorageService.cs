using Azure.Storage.Blobs;
using System.IO;
using System.Threading.Tasks;

namespace azuresolution1.Services
{
    public class BlobStorageService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobStorageService(string containerName, string connectionString)
        {
            _containerClient = new BlobContainerClient(connectionString, containerName);
            _containerClient.CreateIfNotExists();
        }

        public async Task UploadBlobAsync(string blobName, Stream content)
        {
            await _containerClient.UploadBlobAsync(blobName, content);
        }

        public async Task<Stream> DownloadBlobAsync(string blobName)
        {
            var blobClient = _containerClient.GetBlobClient(blobName);
            var response = await blobClient.DownloadContentAsync();
            return response.Value.Content.ToStream();
        }

        public string GetBlobUrl(string blobName)
        {
            var blobClient = _containerClient.GetBlobClient(blobName);
            return blobClient.Uri.ToString();
        }
    }
}