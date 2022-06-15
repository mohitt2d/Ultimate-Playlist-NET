#region Usings

using System;
using System.Collections.Generic;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using UltimatePlaylist.Common.Mvc.Enums;
using UltimatePlaylist.Common.Mvc.Filters;
using UltimatePlaylist.Common.Mvc.Middlewares;
using static UltimatePlaylist.Common.Mvc.Consts.Consts;

#endregion

namespace UltimatePlaylist.Common.Mvc.Extensions
{
    public static class AppExtensions
    {
        #region Swagger

        public static IApplicationBuilder SetupSwaggerAndHealth(this IApplicationBuilder app, ApiType apiType, bool enable = false)
        {
            if (enable)
            {
                app.UseSwagger();

                app.UseSwaggerUI(options =>
                {
                    foreach (var group in GetApiGroups(apiType))
                    {
                        options.SwaggerEndpoint($"/swagger/{group.Value}/swagger.json", $"{group.Key} API");
                    }
                });
            }

            app.UseHealthChecks("/healthcheck");

            return app;
        }

        #endregion

        #region Hangfire

        public static IApplicationBuilder SetupHangfire(this IApplicationBuilder app)
        {
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                //Authorization = new[] { new HangfireAuthorizationFilter(), },
            });

            return app;
        }

        #endregion

        #region ApiSetting

        public static IApplicationBuilder SetupApi(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();
            app.UseMiddleware<UserTokenMiddleware>();
            app.UseMiddleware<EmailChangeWebConfirmationMiddleware>();
            app.UseMiddleware<TokenBlacklistMiddleware>();
            app.UseMiddleware<UserLastActiveMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // return OK 200 for Azure AlwaysOn
            app.MapWhen(ctx => ctx.Request.Path.Value == "/", a => a.Run(async (context) =>
            {
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("OK");
            }));

            return app;
        }

        #endregion

        #region Middlewares

        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            return app;
        }

        #endregion

        #region Private

        private static IDictionary<string, string> GetApiGroups(ApiType apiType) => apiType switch
        {
            ApiType.Mobile => MobileApiGroups.GetNameValueDictionary(),
            ApiType.AdminPanel => AdminApiGroups.GetNameValueDictionary(),
            _ => throw new Exception("Incorrect API type!")
        };

        #endregion
    }
}
