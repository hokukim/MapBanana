using System;

namespace MapBanana.Api.Configuration
{
    public class ApiConfiguration
    {
        public bool IsLocalhost { get; set; }

        // Authentication.
        public string AuthenticationDomain { get; set; }
        public string AuthenticationAudience { get; set; }
        public string AuthenticationClientId { get; set; }

        // Event hub.
        public string EventHubCampaignUrl { get; set; }
        public TimeSpan[] EventHubReconnectDelays { get; set; }

        // Data and storage.
        public string DatabaseConnectionString { get; set; }
        public string StorageConnectionString { get; set; }
        public string StorageCampaignRootName { get; set; }
    }
}
