using MapBanana.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapBanana.API.ICampaignDatabase
{
    public class AzureSqlCampaignDatabase : ICampaignDatabase
    {
        public Task CreateCampaignAsync(string userId, Guid campaignId, string campaignName)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMapAsync(string userId, Guid mapId)
        {
            throw new NotImplementedException();
        }

        public Task<MapResponseModel> GetCampaignActiveMapAsync(Guid campaignId)
        {
            throw new NotImplementedException();
        }

        public Task<List<MapResponseModel>> GetCampaignMapsAsync(string userId, Guid campaignId)
        {
            throw new NotImplementedException();
        }

        public Task<CampaignModel> GetCampaignsAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<MapResponseModel> GetMapAsync(Guid mapId)
        {
            await Task.Yield();

            MapResponseModel mapData = new MapResponseModel()
            {
                Id = mapId,
                CampaignId = Guid.Empty,
                Image = new()
                {
                    Url = string.Empty,
                    SmallUrl = string.Empty
                }
            };

            return mapData;
        }

        public Task SetCampaignActiveMapAsync(string userId, Guid campaignId, Guid mapId)
        {
            throw new NotImplementedException();
        }
    }
}
