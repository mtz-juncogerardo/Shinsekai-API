using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace Shinsekai_API.Services
{
    public interface IBlobService
    {
        Task<string> GetBlobAsync(string fileName);

        Task<IEnumerable<string>> ListBlobAsync();

        Task<string> UploadContentBlobAsync(IFormFile content, string fileName); 

        Task DeleteBlobAsync(string blobName);
    }
}