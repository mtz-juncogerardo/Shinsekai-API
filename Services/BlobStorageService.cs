using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace Shinsekai_API.Services
{
    public class BlobStorageService : IBlobService
    {
        private readonly BlobContainerClient _containerClient;
        private readonly string _path;
        private const string MainPath = "https://shinsekai.blob.core.windows.net/";

        public BlobStorageService(BlobServiceClient blobServiceClient, string containerId = "shinsekai-storage")
        {
            _containerClient = blobServiceClient.GetBlobContainerClient(containerId);
            _path = $"{MainPath}{containerId}/";
            CreateNewBlobStorage();
        }

        private void CreateNewBlobStorage()
        {
            _containerClient.CreateIfNotExists();
        }

        public async Task<string> GetBlobAsync(string fileName = null)
        {
            var filePath = "";

            await foreach (var item in _containerClient.GetBlobsAsync())
            {
                if (fileName == null)
                {
                    filePath = $"{_path}{item.Name}";
                    break;
                }
                if (item.Name == fileName)
                {
                    filePath = $"{_path}{item.Name}";
                    break;
                }
            }

            return filePath;
        }

        public async Task<IEnumerable<string>> ListBlobAsync()
        {
            var files = new List<string>();

            await foreach (var item in _containerClient.GetBlobsAsync())
            {
                files.Add($"{_path}{item.Name}");
            }

            return files;
        }

        public async Task<string> UploadContentBlobAsync(IFormFile content, string fileName)
        {
            var length = content.Length;
            if (length < 0)
            {
                return null;
            }
            
            var fullName = fileName + "." + content.ContentType.Split("/")[1];
            var blobClient = _containerClient.GetBlobClient(fullName);

            await using (var fileStream = content.OpenReadStream())
            {
                await blobClient.UploadAsync(fileStream, new BlobHttpHeaders
                {
                    ContentType = fileName.GetContentType()
                });
            }

            return _path + fullName;
        }

        public async Task DeleteBlobAsync(string blobName)
        {
            var blobContainer = _containerClient.GetBlobClient(blobName);
            await blobContainer.DeleteIfExistsAsync();
        }

        public void DeleteManyBlobs(IEnumerable<string> blobsPaths)
        {
            foreach (var path in blobsPaths)
            {
                var blobContainer = _containerClient.GetBlobClient(path);
                blobContainer.DeleteIfExistsAsync(); 
            }
        }
    }
}