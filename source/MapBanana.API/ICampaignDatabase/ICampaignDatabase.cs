using MapBanana.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapBanana.API.ICampaignDatabase
{
    public interface ICampaignDatabase
    {
        Task<CampaignModel> GetCampaignsAsync(string userId);
        Task CreateCampaignAsync(string userId, Guid campaignId, string campaignName);
        
        Task<MapResponseModel> GetMapAsync(Guid mapId);
        Task<List<MapResponseModel>> GetCampaignMapsAsync(string userId, Guid campaignId);
        Task DeleteMapAsync(string userId, Guid mapId);

        Task<MapResponseModel> GetCampaignActiveMapAsync(Guid campaignId);
        Task SetCampaignActiveMapAsync(string userId, Guid campaignId, Guid mapId);
    }
}
