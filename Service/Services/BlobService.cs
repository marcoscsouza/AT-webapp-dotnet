using Azure.Storage.Blobs;
using Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private const string _containerName = "azureblobat";

        public BlobService(string connectionString)
        {
            _blobServiceClient = new BlobServiceClient(connectionString);
        }
        public async Task<string> UploadAsync(Stream stream)
        {
            var container = _blobServiceClient.GetBlobContainerClient(_containerName);

            await container.CreateIfNotExistsAsync(); //idempotencia
            await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);

            var blobClient = container.GetBlobClient($"{Guid.NewGuid()}.jpg");

            await blobClient.UploadAsync(stream);
            return blobClient.Uri.ToString();

        }

        public async Task DeleteAsync(string blobName)
        {
            //não funcionava
            /*
            var container = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = container.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
            */
            var container = _blobServiceClient.GetBlobContainerClient(_containerName);

            var blob = new BlobClient(new Uri(blobName));

            var blobClient = container.GetBlobClient(blob.Name);

            await blobClient.DeleteIfExistsAsync();
        }
    }
}
