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

        public static async Task<List<CampaignModel>> GetCampaignModelsAsync(this SqlDataReader reader)
        {
            if (!reader.HasRows)
            {
                return new List<CampaignModel>();
            }

            // Read results.
            List<CampaignModel> campaigns = new List<CampaignModel>()
            {
                await reader.GetCampaignModelAsync()
            };

            while (await reader.ReadAsync())
            {
                campaigns.Add(await reader.GetCampaignModelAsync());
            }

            return campaigns;
        }

        public static Task<MapResponseModel> GetMapResponseModelAsync(this SqlDataReader reader)
        {
            if (!reader.HasRows)
            {
                return null;
            }

            return Task.FromResult(new MapResponseModel()
            {
                Id = (Guid)reader[nameof(MapResponseModel.Id)],
                CampaignId = (Guid)reader[nameof(MapResponseModel.CampaignId)],
                Name = (string)reader[nameof(MapResponseModel.Name)],
                ImageUrl = (string)reader[nameof(MapResponseModel.ImageUrl)],
                ImageSmallUrl = (string)reader[nameof(MapResponseModel.ImageSmallUrl)]
            });
        }

        public static async Task<List<MapResponseModel>> GetMapResponseModelsAsync(this SqlDataReader reader)
        {
            if (!reader.HasRows)
            {
                return new List<MapResponseModel>();
            }

            // Read results.
            List<MapResponseModel> maps = new List<MapResponseModel>()
            {
                await reader.GetMapResponseModelAsync()
            };

            while (await reader.ReadAsync())
            {
                maps.Add(await reader.GetMapResponseModelAsync());
            }

            return maps;
        }
    }
}
