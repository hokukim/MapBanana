using System;

namespace MapBanana.Api.Models
{
    public class CampaignModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public CampaignModel()
        {
            Id = Guid.NewGuid();
        }
    }
}
