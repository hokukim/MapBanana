using System;
using System.Collections.Generic;
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

            return await Task.FromResult(Ok(new CampaignModel()
            {
                Id = campaignId
            }));
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Campaigns()
        {
            List<CampaignModel> campaigns = new List<CampaignModel>();

            await Task.Yield();

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

            await Task.Yield();

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
            await Task.Yield();

            return Ok(new List<MapResponseModel>());
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
            await Task.Yield();

            return Ok(new MapResponseModel());
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

            await Task.Yield();

            return Ok(new MapResponseModel());
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
            // Notify listeners that a map has been activated.
            await HubConnection.SendAsync(CampaignEvent.MapActive);

            return Ok(new MapResponseModel());
        }
    }
}