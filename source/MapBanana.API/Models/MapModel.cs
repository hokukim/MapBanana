using System;

namespace MapBanana.Api.Models
{
    public class MapRequestModel : MapModel
    {
    }

    public class MapResponseModel : MapModel
    {
        public ImageModel Image { get; set; }
    }

    public class MapModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CampaignId { get; set; }
    }

    public class ImageModel
    {
        public string Url { get; set; }
        public string SmallUrl { get; set; }
    }
}
