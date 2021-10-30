using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using MapBanana.Api.Configuration;
using MapBanana.Api.Models;
using MapBanana.API.Extensions;
using MapBanana.API.ICampaignDatabase;
using MapBanana.API.Storage;
using MapBanana.Core.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;

namespace MapBanana.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CampaignController : Controller
    {
        private ApiConfiguration ApiConfiguration { get; set; }
        private HubConnection HubConnection { get; }
        private ICampaignStorage CampaignStorage { get; }
        private ICampaignDatabase CampaignDatabase { get; }

        public CampaignController(
            ApiConfiguration apiConfiguration,
            HubConnection hubConnection,
            ICampaignStorage campaignStorage,
            ICampaignDatabase campaignDatabase)
        {
            ApiConfiguration = apiConfiguration;
            HubConnection = hubConnection;
            CampaignStorage = campaignStorage;
            CampaignDatabase = campaignDatabase;
        }

        /// <summary>
        /// Creates a new campaign with the specified name.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(CampaignModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Campaign name cannot be null or whitespace.");
            }

            Guid campaignId = Guid.NewGuid();
            string userId = User.GetBananaId(ApiConfiguration);
            CampaignModel campaignModel = await CampaignDatabase.CreateCampaignAsync(userId, campaignId, name);

            return Ok(campaignModel);
        }

        /// <summary>
        /// Creates a new campaign with the specified name.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{campaignId}")]
        public async Task<IActionResult> Get([FromRoute] Guid campaignId)
        {
            if (campaignId == Guid.Empty)
            {
                return BadRequest($"Campaign ID cannot be empty.");
            }

            string userId = User.GetBananaId(ApiConfiguration);

            CampaignModel campaign = await CampaignDatabase.GetCampaignAsync(userId, campaignId);

            if (campaign == null)
            {
                return NotFound();
            }

            return Ok(campaign);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Campaigns()
        {
            string userId = User.GetBananaId(ApiConfiguration);

            List<CampaignModel> campaigns = await CampaignDatabase.GetCampaignsAsync(userId);

            return Ok(campaigns);
        }

        [HttpPost]
        [Route("{campaignId}/[action]")]
        public async Task<IActionResult> Delete([FromRoute] Guid campaignId)
        {
            if (campaignId == Guid.Empty)
            {
                return BadRequest($"Campaign ID cannot be empty.");
            }

            string userId = User.GetBananaId(ApiConfiguration);

            await Task.WhenAll(
                CampaignDatabase.DeleteCampaignAsync(userId, campaignId),
                CampaignStorage.DeleteCampaignAsync(campaignId)
            );

            return Ok();
        }

        /// <summary>
        /// Gets all maps in the campaign.
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns>List of maps.</returns>
        [HttpGet]
        [Route("{campaignId}/[action]")]
        [ProducesResponseType(typeof(MapModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Maps([FromRoute] Guid campaignId)
        {
            if (campaignId == Guid.Empty)
            {
                return BadRequest($"Campaign ID cannot be empty.");
            }

            string userId = User.GetBananaId(ApiConfiguration);

            List<MapResponseModel> maps = await CampaignDatabase.GetCampaignMapsAsync(userId, campaignId);

            if (maps == null)
            {
                return NotFound();
            }

            return Ok(maps);
        }

        /// <summary>
        /// Adds a map to the campaign.
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{campaignId}/[action]")]
        [ProducesResponseType(typeof(MapModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Map([FromRoute] Guid campaignId, [FromBody] MapRequestModel mapRequest)
        {
            string userId = User.GetBananaId(ApiConfiguration);
            Guid mapId = Guid.NewGuid();

            // Storage.
            using Stream stream = new MemoryStream();
            await mapRequest.FormFile.CopyToAsync(stream);
            MapResponseModel mapResponseModel = await CampaignStorage.SetMapAsync(campaignId, mapId, stream);
            mapResponseModel.Name = mapRequest.Name;

            // Database.
            await CampaignDatabase.AddCampaignMapAsync(userId, mapResponseModel);

            return Ok(mapResponseModel);
        }
        
        /// <summary>
        /// Gets the active map.
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{campaignId}/[action]")]
        public async Task<IActionResult> ActiveMap([FromRoute] Guid campaignId)
        {
            MapResponseModel map = await CampaignDatabase.GetCampaignActiveMapAsync(campaignId);

            if (map == null)
            {
                return NotFound();
            }

            return Ok(map);
        }

        /// <summary>
        /// Activates a map.
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="mapId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{campaignId}/[action]")]
        public async Task<IActionResult> ActiveMap([FromRoute] Guid campaignId, [FromBody] Guid mapId)
        {
            string userId = User.GetBananaId(ApiConfiguration);

           // await CampaignDatabase.SetCampaignActiveMapAsync(userId, campaignId, mapId);

            // Notify listeners that a map has been activated.
            await HubConnection.SendAsync(CampaignEvent.MapActive, campaignId);

            return Ok(new MapResponseModel());
        }
    }
}