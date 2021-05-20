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

        public string UploadFileToStorage(string fileName, string path)
        {
            var myBlob = _blobContainer.GetBlockBlobReference(fileName);
            using (var fileStream = System.IO.File.OpenRead(path))
            {
                myBlob.UploadFromStreamAsync(fileStream);
            }

            return path;
        }
    }
}