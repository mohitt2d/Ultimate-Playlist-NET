#region Usings

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UltimatePlaylist.Common.Mvc.Exceptions;

#endregion

namespace UltimatePlaylist.Common.Mvc.Extensions
{
    public static class ConfigExtensions
    {
        public static T BindConfigurationWithValidation<T>(this IServiceCollection services, IConfiguration configuration, string sectionName)
            where T : class, new()
        {
            var config = new T();
            configuration.GetSection(sectionName).Bind(config);

            var vc = new ValidationContext(config);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(config, vc, results, true);

            if (!isValid)
            {
                foreach (var error in results)
                {
                    throw new InvalidConfigurationException(error.ToString());
                }
            }

            services.AddOptions<T>()
               .Bind(configuration.GetSection(sectionName));

            return config;
        }
    }
}
