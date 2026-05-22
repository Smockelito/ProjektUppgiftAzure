using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;

namespace NwAPI.Services
{
    public class BlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _containerClient;

        public BlobService(IConfiguration config)
        {
            var accountName = config["AzureStorage:AccountName"];
            var containerName = config["AzureStorage:ContainerName"];
            var serviceUri = new Uri($"https://{accountName}.blob.core.windows.net");

            _blobServiceClient = new BlobServiceClient(serviceUri, new DefaultAzureCredential());
            _containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
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

        public async Task<string> GetSasUrlAsync(string blobName, TimeSpan validFor)
        {
            var blobClient = _containerClient.GetBlobClient(blobName);

            var expiresOn = DateTimeOffset.UtcNow.Add(validFor);
            var keyOptions = new Azure.Storage.Blobs.Models.BlobGetUserDelegationKeyOptions(expiresOn)
            {
                StartsOn = DateTimeOffset.UtcNow
            };
            var userDelegationKey = await _blobServiceClient.GetUserDelegationKeyAsync(keyOptions);

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _containerClient.Name,
                BlobName = blobName,
                Resource = "b",
                ExpiresOn = expiresOn
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var accountName = _blobServiceClient.AccountName;
            var sasToken = sasBuilder.ToSasQueryParameters(userDelegationKey, accountName);

            return $"{blobClient.Uri}?{sasToken}";
        }
    }
}
