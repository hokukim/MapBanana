using MapBanana.Api.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapBanana.API.Extensions
{
    public static class SqlDataReaderExtensions
    {
        public static Task<CampaignModel> GetCampaignModelAsync(this SqlDataReader reader)
        {
            if (!reader.HasRows)
            {
                return null;
            }

            return Task.FromResult(new CampaignModel()
            {
                Id = (Guid)reader[nameof(CampaignModel.Id)],
                Name = (string)reader[nameof(CampaignModel.Name)]
            });
        }

        public static async Task<Dictionary<Guid, CampaignModel>> GetCampaignModelsAsync(this SqlDataReader reader)
        {
            if (!reader.HasRows)
            {
                return new();
            }

            // Read results.
            Dictionary<Guid, CampaignModel> campaigns = new();

            while (await reader.ReadAsync())
            {
                CampaignModel campaign = await reader.GetCampaignModelAsync();
                campaigns.Add(campaign.Id, campaign);
            }

            return campaigns;
        }

        public static async Task<MapResponseModel> GetMapResponseModelAsync(this SqlDataReader reader)
        {
            if (!reader.HasRows)
            {
                return null;
            }

            await reader.ReadAsync();

            return reader.ToMapResponseModel();
        }

        public static async Task<Dictionary<Guid, MapResponseModel>> GetMapResponseModelsAsync(this SqlDataReader reader)
        {
            if (!reader.HasRows)
            {
                return new();
            }

            // Read results.
            Dictionary<Guid, MapResponseModel> maps = new();

            while (await reader.ReadAsync())
            {
                MapResponseModel model = reader.ToMapResponseModel();
                maps.Add(model.Id, model);
            }

            return maps;
        }

        private static MapResponseModel ToMapResponseModel(this SqlDataReader reader)
        {
            return new()
            {
                Id = (Guid)reader[nameof(MapResponseModel.Id)],
                CampaignId = (Guid)reader[nameof(MapResponseModel.CampaignId)],
                Name = (string)reader[nameof(MapResponseModel.Name)],
                ImageUrl = (string)reader[nameof(MapResponseModel.ImageUrl)],
                ImageSmallUrl = (string)reader[nameof(MapResponseModel.ImageSmallUrl)]
            };
        }
    }
}
