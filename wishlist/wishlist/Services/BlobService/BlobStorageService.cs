using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wishlist.Services.BlobService
{
    public class BlobStorageService : IBlobStorageService
    {
        IConfiguration configuration;
        public BlobStorageService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void DeleteBlobFolder(long id)
        {
            CloudBlobContainer blobContainer = GetCloudBlobContainer();
            foreach (IListBlobItem blob in blobContainer.GetDirectoryReference(id.ToString()).ListBlobs(true))
            {
                if (blob.GetType() == typeof(CloudBlob) || blob.GetType().BaseType == typeof(CloudBlob))
                {
                    ((CloudBlob)blob).DeleteIfExists();
                }
            }
        }

        public CloudBlobContainer GetCloudBlobContainer()
        {
            string storageConnectionString = Environment.GetEnvironmentVariable("BLOB_CONNECTIONSTRING");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            string containerName = Environment.GetEnvironmentVariable("BLOBCONTAINER_NAME");
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(containerName);
            if (blobContainer.CreateIfNotExists())
            {
                blobContainer.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }
            return blobContainer;
        }

        public async Task<CloudBlockBlob> MakeBlobFolderAndSaveImageAsync(long id, IFormFile image)
        {
            CloudBlobContainer blobContainer = GetCloudBlobContainer();
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(id + "/" + image.Name + ".jpg");
            using (var stream = image.OpenReadStream())
            {
                await blob.UploadFromStreamAsync(stream);
            }
            return blob;
        }
    }
}
