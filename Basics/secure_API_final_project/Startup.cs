using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Basics.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Authorization;
using Basics.Controllers;
using Microsoft.AspNetCore.Authentication;
using Basics.Transformer;
using Basics.CustompolicyProvider;

namespace Basics
{
    public class Startup
    {
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", config =>
                 {
                     config.Cookie.Name = "Secret.Cookie";
                     config.LoginPath = "/Home/Authenticate";      
                 });

            services.AddAuthorization(config =>
            {
                //var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                //var defaultAuthPolicy = defaultAuthBuilder
                // .RequireClaim(ClaimTypes.DateOfBirth)
                // .RequireAuthenticatedUser()
                // .Build();
                // config.DefaultPolicy = defaultAuthPolicy;
                //config.AddPolicy("Claim.DoB", policyBuilder =>
                //{
                // policyBuilder.RequireClaim(ClaimTypes.DateOfBirth);
                // });

                config.AddPolicy("Admin", policyBuilder => policyBuilder.RequireClaim(ClaimTypes.Role, "Admin"));

                config.AddPolicy("Claim.DoB", policyBuilder =>
                {
                    policyBuilder.RequireClaim(ClaimTypes.DateOfBirth);
                });
            });
            services.AddScoped<IAuthorizationHandler, SecurityLevelHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, CustomRequirementClaimHandler>();
            services.AddScoped<IAuthorizationHandler, CookieJarAuthorizationHandler>();
            services.AddScoped<IClaimsTransformation, Claimstransformation>();

            services.AddControllersWithViews(config =>
            {
                // global authorization filter
                var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                var defaultAuthPolicy = defaultAuthBuilder
               
                .RequireAuthenticatedUser()
                .Build();
                //config.Filters.Add(new AuthorizeFilter(defaultAuthPolicy));
            });
            services.AddRazorPages()
                .AddRazorPagesOptions(confg =>
                {
                    confg.Conventions.AuthorizePage("/Razor/secured");
                    confg.Conventions.AuthorizePage("/Razor/policy");
                    confg.Conventions.AuthorizeFolder("/RazorSecured");
                    confg.Conventions.AllowAnonymousToPage("/RazorSecured/anonymous");
                });



        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            //Who are you?
            app.UseAuthentication();

            //Are you allowed?
            app.UseAuthorization();

           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }
    }
}
