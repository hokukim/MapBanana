using MapBanana.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapBanana.API.ICampaignDatabase
{
    public interface ICampaignDatabase
    {
        Task<List<CampaignModel>> GetCampaignsAsync(string userId);
        Task<CampaignModel> CreateCampaignAsync(string userId, Guid campaignId, string campaignName);
        
        Task<MapResponseModel> GetMapAsync(Guid mapId);
        Task DeleteMapAsync(string userId, Guid mapId);
        Task<MapResponseModel> AddCampaignMapAsync(string userId, Guid campaignId, Guid mapId, string imageUrl, string imageSmallUrl);
        Task<List<MapResponseModel>> GetCampaignMapsAsync(string userId, Guid campaignId);

        Task<MapResponseModel> GetCampaignActiveMapAsync(Guid campaignId);
        Task<MapResponseModel> SetCampaignActiveMapAsync(string userId, Guid campaignId, Guid mapId);
    }
}
