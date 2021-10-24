using Azure.Storage.Blobs;
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

        public async Task<Stream> GetMapAsync(Guid campaignId, Guid mapId)
        {
            Stream stream = new MemoryStream();
            BlobClient blobClient = BlobContainerClient.GetBlobClient($"{campaignId}/maps/{mapId}");

            Azure.Response response = blobClient.DownloadTo(stream);
            stream.Position = 0;

            return await Task.FromResult(stream);
        }
    }
}
