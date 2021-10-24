using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using MapBanana.Api.Models;
using MapBanana.API.ICampaignDatabase;
using MapBanana.API.Storage;
using Microsoft.AspNetCore.Mvc;

namespace MapBanana.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MapController : Controller
    {
        private ICampaignStorage CampaignStorage { get; }
        private ICampaignDatabase CampaignDatabase { get; }

        public MapController(
            ICampaignStorage campaignStorage,
            ICampaignDatabase campaignDatabase)
        {
            CampaignStorage = campaignStorage;
            CampaignDatabase = campaignDatabase;
        }

        [HttpGet]
        [Route("{mapId}")]
        [ProducesResponseType(typeof(MapResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get([FromRoute] Guid mapId)
        {
            MapResponseModel map = await CampaignDatabase.GetMapAsync(mapId);

            return await Task.FromResult(Ok(map));
        }
    }
}
