#region Usings

using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Media
{
    public interface IAzureClientService
    {
        Task<ServiceClientCredentials> LogInAsync(string activeDirectoryTenantId, ClientCredential clientCredential);
    }
}
