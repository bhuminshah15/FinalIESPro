using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace server
{
    public class Startup
    {
       
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("oauth")
                .AddJwtBearer("oauth", config =>
                 {
        var secretBytes = Encoding.UTF8.GetBytes(Constants.Secret);
               var key = new SymmetricSecurityKey(secretBytes);

                     config.Events = new JwtBearerEvents()
                     {
                         OnMessageReceived = context =>
                         {
                             if (context.Request.Query.ContainsKey("access_token"))
                             {
                                 context.Token = context.Request.Query["access_token"];
                             }
                             return Task.CompletedTask;
                         }
                     };
                     config.TokenValidationParameters = new TokenValidationParameters()
                    {
                         ClockSkew = TimeSpan.Zero,
                         ValidIssuer = Constants.Issuer,
                         ValidAudience = Constants.Audiance,
                        IssuerSigningKey = key,

                      
                     };
                 });
                

           
            services.AddControllersWithViews()
           .AddRazorRuntimeCompilation();
            //.AddRazorPagesOptions(confg =>
            //{
            //  confg.Conventions.AuthorizePage("/Razor/secured");
            //  confg.Conventions.AuthorizePage("/Razor/policy");
            //  confg.Conventions.AuthorizeFolder("/RazorSecured");
            // confg.Conventions.AllowAnonymousToPage("/RazorSecured/anonymous");
            // });
        }

     
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
                //endpoints.MapRazorPages();
            });
        }
    }
}
