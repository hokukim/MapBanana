using MapBanana.Core.Events;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace MapBanana.EventHub.Hubs
{
    public class CampaignHub : Hub
    {
        public async Task Join(Guid campaignId)
        {
            Console.WriteLine($"User joined campaign: {campaignId}");

            await Groups.AddToGroupAsync(Context.ConnectionId, campaignId.ToString());
            await Clients.Others.SendAsync(CampaignEvent.Joined);
        }

        public async Task MapActive(Guid campaignId)
        {
            Console.WriteLine($"Campaign map activated.");

            await Clients.Group(campaignId.ToString()).SendAsync(CampaignEvent.MapActive);
        }
    }
}
