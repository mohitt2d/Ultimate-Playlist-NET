#region Usings

using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using UltimatePlaylist.Common.Config;

#endregion

namespace UltimatePlaylist.Common.Mvc.Helpers
{
    public static class GeoCoderHelper
    {
        public static async Task<bool> IsUSAZipCodeAsync(string zipCode)
        {
            try
            {
                string url = $"http://ziptasticapi.com/{zipCode}";
                var client = new HttpClient();
                using (var response = await client.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResult = await response.Content.ReadAsStringAsync();
                        dynamic jsonObject = JObject.Parse(jsonResult);

                        if (jsonObject.country == "US")
                        {
                            return true;
                        }
                        return false;
                    }
                };
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}