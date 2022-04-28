#region Usings

using System;

#endregion

namespace UltimatePlaylist.Common.Config
{
    public class AzureMediaServicesConfig
    {
        public string AadClientId { get; set; }

        public string AadSecret { get; set; }

        public string AadTenantDomain { get; set; }

        public string AadTenantId { get; set; }

        public string AccountName { get; set; }

        public string EventGridQueryKey { get; set; }

        public string EventGridSubscriptionName { get; set; }

        public string Location { get; set; }

        public string ResourceGroup { get; set; }

        public string SubscriptionId { get; set; }

        public Uri ArmEndpoint { get; set; }
    }
}
