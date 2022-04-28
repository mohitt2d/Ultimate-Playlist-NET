#region Usings

using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using Microsoft.Rest.Azure.Authentication;
using UltimatePlaylist.Services.Common.Interfaces.Media;

#endregion

namespace UltimatePlaylist.Services.Media
{
    public class AzureClientService : IAzureClientService
    {
        #region Public methods

        public Task<ServiceClientCredentials> LogInAsync(string activeDirectoryTenantId, ClientCredential clientCredential)
        {
            return ApplicationTokenProvider.LoginSilentAsync(
                activeDirectoryTenantId,
                clientCredential,
                ActiveDirectoryServiceSettings.Azure);
        }

        #endregion
    }
}
