using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using ServiceLibrary.AzureBlobService.Blob.Dtos;
using ServiceLibrary.AzureBlobService.IAzureBlobInterface;
using ServiceLibrary.EmailService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Http;
using Azure.Storage.Blobs.Specialized;
using ServiceLibrary.AzureBlobService.Helpers;
using System.Reflection.Metadata;

namespace ServiceLibrary.AzureBlobService.AzureBlob
{
    public class AzureStorage : IAzureStorage
    {
       
        #region Dependency Injection / Constructor

        

        private readonly BlobDetails _blobDetails;
        //private readonly ILogger<AzureStorage> _logger;

        public AzureStorage(BlobDetails blobDetails /*, ILogger<AzureStorage> logger*/)
        {
            _blobDetails= blobDetails;
            //_logger = logger;
        }

        #endregion


        public async Task<List<BlobDto>> ListAsync(string? BlobContainer)
        {
            // Get a reference to a container named in appsettings.json
            BlobContainerClient container = new BlobContainerClient(_blobDetails.BlobConnectionString, BlobContainer);

            // Create a new list object for 
            List<BlobDto> files = new List<BlobDto>();

            await foreach (BlobItem file in container.GetBlobsAsync())
            {
                // Add each file retrieved from the storage container to the files list by creating a BlobDto object
                string uri = container.Uri.ToString();
                var name = file.Name;
                var fullUri = $"{uri}/{name}";

                files.Add(new BlobDto
                {
                    Uri = fullUri,
                    Name = name,
                    ContentType = file.Properties.ContentType
                });
            }

            // Return all files to the requesting method
            return files;
        }

        public async Task<BlobResponseDto> UploadAsync(IFormFile blob, string? BlobContainer)
        {
            // Create new upload response object that we can return to the requesting method
            BlobResponseDto response = new();

            // Get a reference to a container named in appsettings.json and then create it
            BlobContainerClient container = new BlobContainerClient(_blobDetails.BlobConnectionString, BlobContainer);
            //await container.CreateAsync();
            try
            {
                // Get a reference to the blob just uploaded from the API in a container from configuration settings
                BlockBlobClient client = container.GetBlockBlobClient(blob.FileName);

                // Set the chunk size (e.g., 1MB)
                int chunkSizeInBytes = 1024 * 1024; // 1MB

                // Create a buffer to read chunks of the file
                byte[] buffer = new byte[chunkSizeInBytes];

                // Create a list to store the block IDs
                List<string> blockIds = new List<string>();

                // Open the file stream
                using (Stream fileStream = blob.OpenReadStream())
                {
                    int bytesRead;
                    long offset = 0;
                    int blockId = 0;

                    // Read chunks of the file and upload them as blocks
                    while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        // Generate a block ID for the chunk
                        string blockIdBase64 = Convert.ToBase64String(BitConverter.GetBytes(blockId++));

                        // Upload the chunk as a block
                        await client.StageBlockAsync(blockIdBase64, new MemoryStream(buffer, 0, bytesRead));

                        // Add the block ID to the list
                        blockIds.Add(blockIdBase64);

                        // Update the offset
                        offset += bytesRead;
                    }

                    // Commit the uploaded blocks to finalize the file upload
                    await client.CommitBlockListAsync(blockIds);

                }
                  

                // Everything is OK and file got uploaded
                response.Status = $"File {blob.FileName} Uploaded Successfully";
                response.Error = false;
                response.Blob.Uri = client.Uri.AbsoluteUri;
                response.Blob.Name = client.Name;

            }
            // If the file already exists, we catch the exception and do not upload it
            catch (RequestFailedException ex)
               when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                //_logger.LogError($"File with name {blob.FileName} already exists in container. Set another name to store the file in the container: '{_storageContainerName}.'");
                response.Status = $"File with name {blob.FileName} already exists. Please use another name to store your file.";
                response.Error = true;
                return response;
            }
            // If we get an unexpected error, we catch it here and return the error message
            catch (RequestFailedException ex)
            {
                // Log error to console and create a new response we can return to the requesting method
               // _logger.LogError($"Unhandled Exception. ID: {ex.StackTrace} - Message: {ex.Message}");
                response.Status = $"Unexpected error: {ex.StackTrace}. Check log with StackTrace ID.";
                response.Error = true;
                return response;
            }

            // Return the BlobUploadResponse object
            return response;
        }

        public async Task<BlobDto> DownloadAsync(string blobFilename, string? BlobContainer)
        {
            // Get a reference to a container named in appsettings.json
            BlobContainerClient client = new BlobContainerClient(_blobDetails.BlobConnectionString, BlobContainer);

            try
            {
                // Get a reference to the blob uploaded earlier from the API in the container from configuration settings
                BlobClient file = client.GetBlobClient(blobFilename);

               

                // Check if the file exists in the container
                if (await file.ExistsAsync())
                {
                    var data = await file.OpenReadAsync();
                    Stream blobContent = data;

                    // Download the file details async
                    var content = await file.DownloadContentAsync();

                    // Add data to variables in order to return a BlobDto
                    string name = blobFilename;
                    string contentType = content.Value.Details.ContentType;

                    // Create new BlobDto with blob data from variables
                    return new BlobDto { Content = blobContent, Name = name, ContentType = contentType };
                }
                return new BlobDto();
            }
            catch (RequestFailedException ex)
                when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
            {
                // Log error to console
                // _logger.LogError($"File {blobFilename} was not found.");
                // File does not exist, return null and handle that in requesting method
                return new BlobDto();
            }

        }

        public async Task<BlobResponseDto> DeleteAsync(string blobFilename, string? BlobContainer)
        {
            BlobContainerClient client = new BlobContainerClient(_blobDetails.BlobConnectionString, BlobContainer);

            BlobClient file = client.GetBlobClient(blobFilename);

            try
            {
                // Delete the file
                await file.DeleteAsync();
            }
            catch (RequestFailedException ex)
                when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
            {
                // File did not exist, log to console and return new response to requesting method
               // _logger.LogError($"File {blobFilename} was not found.");
                return new BlobResponseDto { Error = true, Status = $"File with name {blobFilename} not found." };
            }

            // Return a new BlobResponseDto to the requesting method
            return new BlobResponseDto { Error = false, Status = $"File: {blobFilename} has been successfully deleted." };

        }
    }
}

