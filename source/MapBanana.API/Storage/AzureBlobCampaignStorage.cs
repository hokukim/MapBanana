using Azure.Storage.Blobs;
using MapBanana.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MapBanana.API.Storage
{
    public class AzureBlobCampaignStorage : ICampaignStorage
    {
        private BlobContainerClient BlobContainerClient { get; }

        public AzureBlobCampaignStorage(BlobContainerClient blobContainerClient)
        {
            BlobContainerClient = blobContainerClient;
        }

        public async Task<MapResponseModel> GetMapAsync(Guid campaignId, Guid mapId)
        {
            MapResponseModel mapResponseModel = new MapResponseModel()
            {
                Id = mapId,
                CampaignId = campaignId
            };

            // Image URL.
            string imageUrl = GetImageUrl(campaignId, mapId);
            BlobClient blobClient = BlobContainerClient.GetBlobClient(imageUrl);
            mapResponseModel.ImageUrl = blobClient.Uri.ToString();

            // Image small URL.
            string imageSmallUrl = GetImageUrl(campaignId, mapId, true);
            BlobClient blobClientSmall = BlobContainerClient.GetBlobClient(imageSmallUrl);
            mapResponseModel.ImageSmallUrl = blobClientSmall.Uri.ToString();

            return await Task.FromResult(mapResponseModel);
        }

        public async Task<MapResponseModel> SetMapAsync(Guid campaignId, Guid mapId, Stream dataStream)
        {
            // Upload.
            string imageUrl = GetImageUrl(campaignId, mapId);
            string imageSmallUrl = GetImageUrl(campaignId, mapId, true);

            List<Task> uploadTasks = new List<Task>()
            {
                BlobContainerClient.UploadBlobAsync(imageUrl, dataStream),
                BlobContainerClient.UploadBlobAsync(imageSmallUrl, dataStream)
            };

            await Task.WhenAll(uploadTasks);

            // Build model.
            BlobClient blobClient = BlobContainerClient.GetBlobClient(imageUrl);
            BlobClient blobClientSmall = BlobContainerClient.GetBlobClient(imageSmallUrl);

            MapResponseModel mapResponseModel = new MapResponseModel()
            {
                Id = mapId,
                CampaignId = campaignId,
                ImageUrl = blobClient.Uri.ToString(),
                ImageSmallUrl = blobClientSmall.Uri.ToString()
            };

            return mapResponseModel;
        }

        public async Task DeleteMapAsync(Guid campaignId, Guid mapId)
        {
            string imageUrl = GetImageUrl(campaignId, mapId, true);
            string imageSmallUrl = GetImageUrl(campaignId, mapId, true);

            List<Task> tasks = new List<Task>()
            {
                BlobContainerClient.DeleteBlobAsync(imageUrl),
                BlobContainerClient.DeleteBlobAsync(imageSmallUrl)
            };

            await Task.WhenAll(tasks);
        }

        private string GetImageUrl(Guid campaignId, Guid mapId, bool small = false)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"{campaignId}/maps/{mapId}");

            if (small)
            {
                builder.Append("_small");
            }

            return builder.ToString();
        }
    }
}
