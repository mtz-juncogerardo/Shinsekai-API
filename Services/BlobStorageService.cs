using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Shinsekai_API.Config;

namespace Shinsekai_API.Services
{
    public class BlobStorageService
    {
        private CloudStorageAccount _storageAccount;
        private CloudBlobClient _blobClient;
        private CloudBlobContainer _blobContainer;
        private ApiConfiguration _configuration;

        public BlobStorageService(string containerName, IConfiguration configuration)
        {
            _storageAccount = CloudStorageAccount.Parse(new ApiConfiguration(configuration).SvrConnectionString);
            _blobClient = _storageAccount.CreateCloudBlobClient();
            _blobContainer = _blobClient.GetContainerReference(containerName);
        }

        public void CreateNewBlobStorage(BlobContainerPublicAccessType accesType)
        {
            _blobContainer.CreateIfNotExists();
            _blobContainer.SetPermissions(new BlobContainerPermissions
            {
                PublicAccess = accesType
            });
        }

        public async Task<string> UploadFileToStorage(IFormFile myFile, string fileName)
        {
            var myBlob = _blobContainer.GetBlockBlobReference(fileName);
            var totalSize = myFile.Length;
            var fileBytes = new byte[myFile.Length];

            using (var fileStream = myFile.OpenReadStream())
            {
                var offset = 0;

                while (offset < myFile.Length)
                {
                    var chunkSize = totalSize - offset < 8192 ? (int) totalSize - offset : 8192;

                    offset += await fileStream.ReadAsync(fileBytes, offset, chunkSize);
                }
                
                myBlob.UploadFromStream(fileStream);
            }

            return fileName;
        }
    }
}