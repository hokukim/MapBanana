using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MapBanana.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MapController : Controller
    {
        [HttpGet]
        [Route("{mapId}")]
        public async Task<IActionResult> Get([FromRoute] Guid mapId)
        {
            return await Task.FromResult(Ok());
        }
    }
}
