using Azure.Storage.Blobs;
using MapBanana.Api.Models;
using System;
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
            string imageUrl = GetMapUrl(campaignId, mapId);
            BlobClient blobClient = BlobContainerClient.GetBlobClient(imageUrl);
            mapResponseModel.ImageUrl = blobClient.Uri.ToString();

            // Image small URL.
            string imageSmallUrl = GetMapUrl(campaignId, mapId, true);
            BlobClient blobClientSmall = BlobContainerClient.GetBlobClient(imageSmallUrl);
            mapResponseModel.ImageSmallUrl = blobClientSmall.Uri.ToString();

            return await Task.FromResult(mapResponseModel);
        }

        public async Task<MapResponseModel> SetMapAsync(Guid campaignId, Guid mapId, Stream dataStream, Stream dataSmallStream)
        {
            dataStream.Position = 0;
            dataSmallStream.Position = 0;

            // Upload.
            string imageUrl = GetMapUrl(campaignId, mapId);
            string imageSmallUrl = GetMapUrl(campaignId, mapId, true);

            await Task.WhenAll(
                BlobContainerClient.UploadBlobAsync(imageUrl, dataStream),
                BlobContainerClient.UploadBlobAsync(imageSmallUrl, dataSmallStream)
            );

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
            string imageUrl = GetMapUrl(campaignId, mapId, true);
            string imageSmallUrl = GetMapUrl(campaignId, mapId, true);

            await Task.WhenAll(
                BlobContainerClient.DeleteBlobAsync(imageUrl),
                BlobContainerClient.DeleteBlobAsync(imageSmallUrl)
            );
        }

        public async Task DeleteCampaignAsync(Guid campaignId)
        {
            string campaignUrl = GetCampaignUrl(campaignId);

            await BlobContainerClient.DeleteBlobIfExistsAsync(campaignUrl);
        }

        private string GetMapUrl(Guid campaignId, Guid mapId, bool small = false)
        {
            string campaignUrl = GetCampaignUrl(campaignId);

            StringBuilder builder = new StringBuilder();
            builder.Append($"{campaignUrl}/maps/{mapId}");

            if (small)
            {
                builder.Append("_small");
            }

            return builder.ToString();
        }

        private string GetCampaignUrl(Guid campaignId)
        {
            return campaignId.ToString();
        }
    }
}
