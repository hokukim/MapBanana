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
using Microsoft.AspNetCore.Http;
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
        [Route("")]
        [ProducesResponseType(typeof(CampaignModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCampaignRequestModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Name))
            {
                return BadRequest("Campaign name cannot be null or whitespace.");
            }

            Guid campaignId = Guid.NewGuid();
            string userId = User.GetBananaId(ApiConfiguration);
            CampaignModel campaignModel = await CampaignDatabase.CreateCampaignAsync(userId, campaignId, model.Name);

            // Notify listeners that a campaign has been created.
            await HubConnection.SendAsync(CampaignEvent.CampaignCreated, campaignId);

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

            return Ok(await CampaignDatabase.GetCampaignsAsync(userId));
        }

        public class Res
        {
            public Dictionary<Guid, CampaignModel> D;
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
        [ProducesResponseType(typeof(Dictionary<Guid, MapResponseModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Maps([FromRoute] Guid campaignId)
        {
            if (campaignId == Guid.Empty)
            {
                return BadRequest($"Campaign ID cannot be empty.");
            }

            string userId = User.GetBananaId(ApiConfiguration);

            Dictionary<Guid, MapResponseModel> maps = await CampaignDatabase.GetCampaignMapsAsync(userId, campaignId);

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
        public async Task<IActionResult> Map([FromRoute] Guid campaignId)
        {
            string userId = User.GetBananaId(ApiConfiguration);
            Guid mapId = Guid.NewGuid();

            // File.
            IFormFile file = Request.Form.Files[0];

            // Storage.
            using Stream stream = new MemoryStream();
            await file.CopyToAsync(stream);
            MapResponseModel mapResponseModel = await CampaignStorage.SetMapAsync(campaignId, mapId, stream);
            mapResponseModel.Name = file.FileName;

            // Database.
            await CampaignDatabase.AddCampaignMapAsync(userId, mapResponseModel);

            // Notify listeners that a campaign has been created.
            await HubConnection.SendAsync(CampaignEvent.MapAdded, campaignId);

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