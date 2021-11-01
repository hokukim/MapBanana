using System;

namespace MapBanana.Api.Models
{
    public class CampaignModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ActiveMapId { get; set; }

        public CampaignModel()
        {
            Id = Guid.NewGuid();
        }
    }

    public class CreateCampaignRequestModel
    {
        public string Name { get; set; }
    }
}
