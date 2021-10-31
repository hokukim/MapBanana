using MapBanana.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapBanana.API.ICampaignDatabase
{
    public interface ICampaignDatabase
    {
        Task<Dictionary<Guid, CampaignModel>> GetCampaignsAsync(string userId);
        Task<CampaignModel> GetCampaignAsync(string userId, Guid campaignId);
        Task<CampaignModel> CreateCampaignAsync(string userId, Guid campaignId, string campaignName);
        Task DeleteCampaignAsync(string userId, Guid campaignId);

        Task<MapResponseModel> GetMapAsync(Guid mapId);
        Task DeleteMapAsync(string userId, Guid mapId);
        Task<MapResponseModel> AddCampaignMapAsync(string userId, MapResponseModel map);
        Task<Dictionary<Guid, MapResponseModel>> GetCampaignMapsAsync(string userId, Guid campaignId);
        Task DeleteCampaignMapsAsync(string userId, Guid campaignId);

        Task<MapResponseModel> GetCampaignActiveMapAsync(Guid campaignId);
        Task<MapResponseModel> SetCampaignActiveMapAsync(string userId, Guid campaignId, Guid mapId);
    }
}
