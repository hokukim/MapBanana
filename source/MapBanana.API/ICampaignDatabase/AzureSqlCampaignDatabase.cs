using MapBanana.Api.Configuration;
using MapBanana.Api.Models;
using MapBanana.API.Extensions;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MapBanana.API.ICampaignDatabase
{
    public class AzureSqlCampaignDatabase : ICampaignDatabase
    {
        private ApiConfiguration ApiConfiguration { get; }

        public AzureSqlCampaignDatabase(ApiConfiguration apiConfiguration)
        {
            ApiConfiguration = apiConfiguration;
        }

        #region Campaign
        public async Task<CampaignModel> CreateCampaignAsync(string userId, Guid campaignId, string campaignName)
        {
            const string COMMAND_NAME = "CreateCampaign";

            using SqlConnection sqlConnection = new SqlConnection(ApiConfiguration.DatabaseConnectionString);
            await sqlConnection.OpenAsync();

            using SqlCommand command = new SqlCommand(COMMAND_NAME, sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(ParameterName.USER_ID, userId));
            command.Parameters.Add(new SqlParameter(ParameterName.CAMPAIGN_NAME, campaignId));
            command.Parameters.Add(new SqlParameter(ParameterName.CAMPAIGN_ID, campaignId));

            await sqlConnection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            
            return await reader.GetCampaignModelAsync();
        }

        public async Task<List<CampaignModel>> GetCampaignsAsync(string userId)
        {
            const string COMMAND_NAME = "GetCampaigns";

            using SqlConnection sqlConnection = new SqlConnection(ApiConfiguration.DatabaseConnectionString);
            await sqlConnection.OpenAsync();

            using SqlCommand command = new SqlCommand(COMMAND_NAME, sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(ParameterName.USER_ID, userId));

            await sqlConnection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            return await reader.GetCampaignModelsAsync();
        }

        public async Task<CampaignModel> GetCampaignAsync(string userId, Guid campaignId)
        {
            const string COMMAND_NAME = "GetCampaign";

            using SqlConnection sqlConnection = new SqlConnection(ApiConfiguration.DatabaseConnectionString);
            await sqlConnection.OpenAsync();

            using SqlCommand command = new SqlCommand(COMMAND_NAME, sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(ParameterName.USER_ID, userId));
            command.Parameters.Add(new SqlParameter(ParameterName.CAMPAIGN_ID, campaignId));

            await sqlConnection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            return await reader.GetCampaignModelAsync();
        }

        /// <summary>
        /// Delete maps and campaign.
        /// </summary>
        public async Task DeleteCampaignAsync(string userId, Guid campaignId)
        {
            // Delete maps.
            await DeleteCampaignMapsAsync(userId, campaignId);

            // Delete campaign.
            const string COMMAND_NAME = "DeleteCampaign";

            using SqlConnection sqlConnection = new SqlConnection(ApiConfiguration.DatabaseConnectionString);
            await sqlConnection.OpenAsync();

            using SqlCommand command = new SqlCommand(COMMAND_NAME, sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(ParameterName.USER_ID, userId));
            command.Parameters.Add(new SqlParameter(ParameterName.MAP_ID, campaignId));

            await sqlConnection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            return;
        }
        #endregion

        #region Map
        public async Task<MapResponseModel> GetMapAsync(Guid mapId)
        {
            const string COMMAND_NAME = "GetMap";

            using SqlConnection sqlConnection = new SqlConnection(ApiConfiguration.DatabaseConnectionString);
            await sqlConnection.OpenAsync();

            using SqlCommand command = new SqlCommand(COMMAND_NAME, sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(ParameterName.MAP_ID, mapId));

            await sqlConnection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            // Read results.
            return await reader.GetMapResponseModelAsync();
        }

        public async Task DeleteMapAsync(string userId, Guid mapId)
        {
            const string COMMAND_NAME = "DeleteMap";

            using SqlConnection sqlConnection = new SqlConnection(ApiConfiguration.DatabaseConnectionString);
            using SqlCommand command = new SqlCommand(COMMAND_NAME, sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(ParameterName.USER_ID, userId));
            command.Parameters.Add(new SqlParameter(ParameterName.MAP_ID, mapId));

            await sqlConnection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            return;
        }

        public async Task<MapResponseModel> AddCampaignMapAsync(string userId, MapResponseModel map)
        {
            const string COMMAND_NAME = "AddCampaignMap";

            using SqlConnection sqlConnection = new SqlConnection(ApiConfiguration.DatabaseConnectionString);
            await sqlConnection.OpenAsync();

            using SqlCommand command = new SqlCommand(COMMAND_NAME, sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(ParameterName.USER_ID, userId));
            command.Parameters.Add(new SqlParameter(ParameterName.CAMPAIGN_ID, map.CampaignId));
            command.Parameters.Add(new SqlParameter(ParameterName.MAP_ID, map.Id));
            command.Parameters.Add(new SqlParameter(ParameterName.MAP_IMAGE_URL, map.ImageUrl));
            command.Parameters.Add(new SqlParameter(ParameterName.MAP_IMAGE_URL, map.ImageSmallUrl));

            await sqlConnection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            // Read results.
            return await reader.GetMapResponseModelAsync();
        }

        public async Task<List<MapResponseModel>> GetCampaignMapsAsync(string userId, Guid campaignId)
        {
            const string COMMAND_NAME = "AddCampaignMap";

            using SqlConnection sqlConnection = new SqlConnection(ApiConfiguration.DatabaseConnectionString);
            await sqlConnection.OpenAsync();

            using SqlCommand command = new SqlCommand(COMMAND_NAME, sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(ParameterName.CAMPAIGN_ID, campaignId));

            await sqlConnection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            // Read results.
            return await reader.GetMapResponseModelsAsync();
        }

        public async Task DeleteCampaignMapsAsync(string userId, Guid campaignId)
        {
            const string COMMAND_NAME = "DeleteCampaignMaps";

            using SqlConnection sqlConnection = new SqlConnection(ApiConfiguration.DatabaseConnectionString);
            using SqlCommand command = new SqlCommand(COMMAND_NAME, sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(ParameterName.USER_ID, userId));
            command.Parameters.Add(new SqlParameter(ParameterName.MAP_ID, campaignId));

            await sqlConnection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            return;
        }
        #endregion

        #region Active map
        public async Task<MapResponseModel> GetCampaignActiveMapAsync(Guid campaignId)
        {
            const string COMMAND_NAME = "GetCampaignActiveMap";

            using SqlConnection sqlConnection = new SqlConnection(ApiConfiguration.DatabaseConnectionString);
            await sqlConnection.OpenAsync();

            using SqlCommand command = new SqlCommand(COMMAND_NAME, sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(ParameterName.CAMPAIGN_ID, campaignId));

            await sqlConnection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            // Read results.
            return await reader.GetMapResponseModelAsync();
        }

        public async Task<MapResponseModel> SetCampaignActiveMapAsync(string userId, Guid campaignId, Guid mapId)
        {
            const string COMMAND_NAME = "SetCampaignActiveMap";

            using SqlConnection sqlConnection = new SqlConnection(ApiConfiguration.DatabaseConnectionString);
            await sqlConnection.OpenAsync();

            using SqlCommand command = new SqlCommand(COMMAND_NAME, sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(ParameterName.USER_ID, userId));
            command.Parameters.Add(new SqlParameter(ParameterName.CAMPAIGN_ID, campaignId));
            command.Parameters.Add(new SqlParameter(ParameterName.MAP_ID, mapId));

            await sqlConnection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            // Read results.
            return await reader.GetMapResponseModelAsync();
        }
        #endregion

        private class ParameterName
        {
            public const string USER_ID = "UserId";

            public const string CAMPAIGN_ID = "Id";
            public const string CAMPAIGN_NAME = "Name";

            public const string MAP_ID = "Id";
            public const string MAP_NAME = "Name";
            public const string MAP_IMAGE_URL = "ImageUrl";
            public const string MAP_IMAGE_SMALL_URL = "ImageSmallUrl";
        }
    }
}
