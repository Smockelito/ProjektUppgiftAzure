using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace NwAPI.Services
{
    public class BlobService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobService(IConfiguration config)
        {
            var accountName = config["AzureStorage:AccountName"];
            var containerName = config["AzureStorage:ContainerName"];
            var serviceUri = new Uri($"https://{accountName}.blob.core.windows.net");

            var blobServiceClient = new BlobServiceClient(serviceUri, new DefaultAzureCredential());
            _containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var blobClient = _containerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(file.OpenReadStream(), new BlobHttpHeaders
            {
                ContentType = file.ContentType
            });

            return blobClient.Uri.ToString();
        }
    }
}
