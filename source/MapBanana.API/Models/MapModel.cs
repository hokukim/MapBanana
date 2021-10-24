using Microsoft.AspNetCore.Http;
using System;

namespace MapBanana.Api.Models
{
    public class MapRequestModel : MapModel
    {
        public IFormFile FormFile { get; set; }
    }

    public class MapResponseModel : MapModel
    {
        public string ImageUrl { get; set; }
        public string ImageSmallUrl { get; set; }
    }

    public class MapModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CampaignId { get; set; }
    }
}
