#region Usings

using System;
using System.Threading.Tasks;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using UltimatePlaylist.Database.Infrastructure.Seeding;

#endregion

namespace UltimatePlaylist.MobileApi
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                    optional: true,
                    reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                Log.Information("Starting web host.");

                var host = CreateHostBuilder(args).Build();

                //await InitializeDatabaseAsync(host);
                //testing

                await host.RunAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");

                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                .ConfigureAppConfiguration((context, config) =>
                {
                     if (context.HostingEnvironment.IsProduction())
                     {
                         var builtConfig = config.Build();
                         var secretClient = new SecretClient(
                             new Uri(builtConfig["VaultUri"]),
                             new DefaultAzureCredential());
                         config.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
                     }
                });

        private static async Task InitializeDatabaseAsync(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var databaseInitializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();

            try
            {
                await databaseInitializer.InitializeAsync();
            }
            catch (Exception exception)
            {
                throw new Exception("Unhandled exception occured while seeding the database.", exception);
            }
        }
    }
}
