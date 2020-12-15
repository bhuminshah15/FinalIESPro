using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
namespace client
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAuthentication(config =>
              {

                  // We check the cookie to confirm that we are authenticated
                  config.DefaultAuthenticateScheme = "ClientCookie";
                  // When we sign in we will deal out a cookie
                  config.DefaultSignInScheme = "ClientCookie";
                  // use this to check if we are allowed to do something.
                  config.DefaultChallengeScheme = "OurServer";
              })
             .AddCookie("ClientCookie")
             .AddOAuth("OurServer", config =>
             {

                 config.ClientId = "client_id";
                 config.ClientSecret = "client_secret";
                 config.CallbackPath = "/OAuth/callback";
                 config.AuthorizationEndpoint = "https://localhost:44319/OAuth/Authorize";
                 config.TokenEndpoint = "https://localhost:44319/OAuth/token";
                 config.SaveTokens = true;

                 config.Events = new OAuthEvents()
                 {
                     /*OnCreatingTicket = context =>
                    {
                        string accessToken1 = context.AccessToken;
                        var accessToken = accessToken1;
                        var base64payload = accessToken.Split('.')[1];
                        var bytes = Convert.FromBase64String(base64payload);
                        var jsonPayload = Encoding.UTF8.GetString(bytes);
                        var claims = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonPayload);

                        foreach (var claim in claims)
                        {
                            context.Identity.AddClaim(new Claim(claim.Key, claim.Value));
                        }

                        return Task.CompletedTask;
                    }*/

                 };
             });

             services.AddHttpClient();

            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}