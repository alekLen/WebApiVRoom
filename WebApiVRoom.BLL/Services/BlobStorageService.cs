using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.BLL.DTO;

namespace WebApiVRoom.BLL.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public BlobStorageService(BlobServiceClient blobServiceClient, string containerName)
        {
            _blobServiceClient = blobServiceClient;
            _containerName = containerName;
        }

        public async Task<BlobStorageDTO> UploadFileAsync(Stream fileStream, string fileName)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                Console.WriteLine($"Connecting to container: {_containerName}");

                await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

                var blobClient = containerClient.GetBlobClient(fileName);
                Console.WriteLine($"Uploading file: {fileName}");

                await blobClient.UploadAsync(fileStream, overwrite: true);
                var blobProperties = await blobClient.GetPropertiesAsync();

                return new BlobStorageDTO
                {
                    FileUrl = blobClient.Uri.ToString(),
                    ContainerName = _containerName,
                    BlobName = fileName,
                    UploadDate = DateTime.UtcNow,
                    ContentType = blobProperties.Value.ContentType,
                    Size = blobProperties.Value.ContentLength
                };
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Azure Request Failed: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UploadFileAsync: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteFileAsync(string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
            {
                throw new ArgumentException("File URL cannot be null or empty");
            }

            Uri uri = new Uri(fileUrl);
            string fileName = Path.GetFileName(uri.LocalPath);
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            Console.WriteLine($"Deleting file: {fileName}");

            try
            {
                var response = await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
                if (!response)
                {
                    Console.WriteLine("File did not exist or could not be deleted.");
                }
                else
                {
                    Console.WriteLine("File successfully deleted.");
                }
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Failed to delete blob: {ex.Message}");
                throw;
            }
        }


        public async Task<bool> BlobExistsAsync(string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            return await blobClient.ExistsAsync();
        }

        public async Task<BlobStorageDTO> DownloadFileAsync(string fileUrl)
        {
            Uri uri = new Uri(fileUrl);
            string fileName = Path.GetFileName(uri.LocalPath);

            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            if (await blobClient.ExistsAsync())
            {
                var response = await blobClient.DownloadAsync();
                var blobProperties = await blobClient.GetPropertiesAsync();

                using var memoryStream = new MemoryStream();
                await response.Value.Content.CopyToAsync(memoryStream);

                return new BlobStorageDTO
                {
                    FileUrl = blobClient.Uri.ToString(),
                    ContainerName = _containerName,
                    BlobName = fileName,
                    UploadDate = blobProperties.Value.LastModified.DateTime,
                    ContentType = blobProperties.Value.ContentType,
                    Size = blobProperties.Value.ContentLength
                };
            }

            throw new FileNotFoundException("The specified file was not found in blob storage.");
        }

        public async Task<IEnumerable<BlobStorageDTO>> ListBlobsAsync()
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobs = new List<BlobStorageDTO>();

            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                var blobClient = containerClient.GetBlobClient(blobItem.Name);
                var blobProperties = await blobClient.GetPropertiesAsync();

                blobs.Add(new BlobStorageDTO
                {
                    FileUrl = blobClient.Uri.ToString(),
                    ContainerName = _containerName,
                    BlobName = blobItem.Name,
                    UploadDate = blobProperties.Value.LastModified.DateTime,
                    ContentType = blobProperties.Value.ContentType,
                    Size = blobProperties.Value.ContentLength
                });
            }

            return blobs;
        }
    }

}
