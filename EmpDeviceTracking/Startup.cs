using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmpDeviceTracking.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using System.IO;
//using Newtonsoft.Json.Serialization;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace EmpDeviceTracking
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
            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie();
            //.AddOpenIdConnect(options =>
            //{

            //    options.ClientId = Configuration["okta:ClientId"];
            //    options.ClientSecret = Configuration["okta:ClientSecret"];
            //    options.Authority = Configuration["okta:Issuer"];
            //    options.RequireHttpsMetadata = true;
            //    options.CallbackPath = "/authorization-code/callback";
            //    options.SignedOutRedirectUri = Configuration["okta:SignedOutRedirectUri"];
            //    options.ResponseType = "code";
            //    options.SaveTokens = true;
            //    options.UseTokenLifetime = false;
            //    options.GetClaimsFromUserInfoEndpoint = true;
            //    options.Scope.Add("openid");
            //    options.Scope.Add("profile");
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        NameClaimType = "name",
            //        RoleClaimType = "groups",
            //        ValidateIssuer = true
            //    };
            //});

            //if (!string.IsNullOrEmpty(Configuration["okta:APIToken"]))
            //{
            //    services.AddSingleton<IOktaClient>
            //    (
            //        new OktaClient(new OktaClientConfiguration()
            //        {
            //            Token = Configuration["okta:APIToken"]
            //        })
            //    );
            //}
            services.AddMvc();
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddAuthorization();
           
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(6000);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }

        //https://www.tutorialsteacher.com/core/aspnet-core-logging
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var path = Directory.GetCurrentDirectory();
            //loggerFactory.AddFile($"{path}\\Logs\\Log.txt");
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/Home/Error");
                //app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseSession();
           
          

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=User}/{action=Login}/{id?}");
            });
        }
    }
}
