using System;
using System.IO.Compression;
using $safeprojectname$.Core.Client;
using $safeprojectname$.Core.Client.TestClient;
using $safeprojectname$.Core.Services.CacheService;
using $safeprojectname$.Models.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace $safeprojectname$
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Get config settings
            services.Configure<TestConfig>(Configuration.GetSection("Test"));

            // Configure Compression level
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

            // Add Response compression services
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });

            // Cache on server based on client cache rules
            services.AddResponseCaching();


            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });

            // Client side caching profiles, keep small because no way to invalidate
            services.AddMvc(options =>
            {
                options.CacheProfiles.Add("Default",
                    new CacheProfile()
                    {
                        VaryByHeader = "Accept-Encoding",
                        Location = ResponseCacheLocation.Any,
                        Duration = 60
                    });
                options.CacheProfiles.Add("Never",
                    new CacheProfile()
                    {
                        Location = ResponseCacheLocation.None,
                        NoStore = true
                    });
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //services.AddSession();

            // Add application services
            services.AddSingleton<IBaseClient, BaseClient>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddTransient<ITestClient, TestClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Add default security headers
            var policyCollection = new HeaderPolicyCollection()
                .AddDefaultSecurityHeaders();

            app.UseSecurityHeaders(policyCollection);

            if (env.IsDevelopment())
            {
                // detailed error pages
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // friendly error pages
                app.UseStatusCodePagesWithReExecute("/error/{0}");

                // output caching in production
                app.UseResponseCaching();

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Gzip
            app.UseResponseCompression();

            // HTTPS Redirection
            app.UseHttpsRedirection();

            // add cache control header to static resources
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = (context) =>
                {
                    var headers = context.Context.Response.GetTypedHeaders();
                    headers.CacheControl = new CacheControlHeaderValue()
                    {
                        MaxAge = TimeSpan.FromDays(365),
                        Public = true
                    };
                }
            });

            app.UseCookiePolicy();

            // Enable sessions
            //app.UseSession();            

            // don't pass routes for Attribute routing
            app.UseMvc();
        }
    }
}
