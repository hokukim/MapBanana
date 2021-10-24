using System;
using System.IO;
using System.Threading.Tasks;

namespace MapBanana.API.Storage
{
    public interface ICampaignStorage
    {
        Task<Stream> GetMapAsync(Guid campaignId, Guid mapId);
    }
}
