﻿using MapBanana.Api.Configuration;
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
            using SqlCommand command = new SqlCommand(COMMAND_NAME, sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(ParameterName.USER_ID, userId));

            await sqlConnection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            return await reader.GetCampaignModelsAsync();
        }
        #endregion

        #region Map
        public async Task<MapResponseModel> GetMapAsync(Guid mapId)
        {
            const string COMMAND_NAME = "GetMap";

            using SqlConnection sqlConnection = new SqlConnection(ApiConfiguration.DatabaseConnectionString);
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

        public async Task<MapResponseModel> AddCampaignMapAsync(string userId, Guid campaignId, Guid mapId, string imageUrl, string imageSmallUrl)
        {
            const string COMMAND_NAME = "AddCampaignMap";

            using SqlConnection sqlConnection = new SqlConnection(ApiConfiguration.DatabaseConnectionString);
            using SqlCommand command = new SqlCommand(COMMAND_NAME, sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(ParameterName.USER_ID, userId));
            command.Parameters.Add(new SqlParameter(ParameterName.CAMPAIGN_ID, campaignId));
            command.Parameters.Add(new SqlParameter(ParameterName.MAP_ID, mapId));
            command.Parameters.Add(new SqlParameter(ParameterName.MAP_IMAGE_URL, imageUrl));
            command.Parameters.Add(new SqlParameter(ParameterName.MAP_IMAGE_URL, imageSmallUrl));

            await sqlConnection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            // Read results.
            return await reader.GetMapResponseModelAsync();
        }

        public async Task<List<MapResponseModel>> GetCampaignMapsAsync(string userId, Guid campaignId)
        {
            const string COMMAND_NAME = "AddCampaignMap";

            using SqlConnection sqlConnection = new SqlConnection(ApiConfiguration.DatabaseConnectionString);
            using SqlCommand command = new SqlCommand(COMMAND_NAME, sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(ParameterName.CAMPAIGN_ID, campaignId));

            await sqlConnection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            // Read results.
            return await reader.GetMapResponseModelsAsync();
        }
        #endregion

        #region Active map
        public async Task<MapResponseModel> GetCampaignActiveMapAsync(Guid campaignId)
        {
            const string COMMAND_NAME = "GetCampaignActiveMap";

            using SqlConnection sqlConnection = new SqlConnection(ApiConfiguration.DatabaseConnectionString);
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