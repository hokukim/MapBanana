using MapBanana.Api.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MapBanana.API.Storage
{
    public interface ICampaignStorage
    {
        Task<MapResponseModel> GetMapAsync(Guid campaignId, Guid mapId);
        Task<MapResponseModel> SetMapAsync(Guid campaignId, Guid mapId, Stream fileStream);
        Task DeleteMapAsync(Guid campaignId, Guid mapId);
        Task DeleteCampaignAsync(Guid campaignId);
    }
}
