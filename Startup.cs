using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using DragonflyTracker.Installers;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using DragonflyTracker.Contracts.HealthChecks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using AutoMapper;
using DragonflyTracker.Filters;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Antiforgery;
using System;

namespace DragonflyTracker
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins(
                                                          "http://dragonflytracker.test",
                                                          "http://dragonflytracker.com",
                                                          "http://localhost:4205",
                                                          "https://api.dragonflytracker.com")
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod()
                                                  .AllowCredentials()
                                                  .SetIsOriginAllowedToAllowWildcardSubdomains()
                                                  ;
                                  });
            });
            services.InstallServicesInAssembly(Configuration);
            services.AddAutoMapper(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMapper mapper, IAntiforgery antiforgery)
        {
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";

                    var response = new HealthCheckResponse
                    {
                        Status = report.Status.ToString(),
                        Checks = report.Entries.Select(x => new HealthCheck
                        {
                            Component = x.Key,
                            Status = x.Value.Status.ToString(),
                            Description = x.Value.Description
                        }),
                        Duration = report.TotalDuration
                    };

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response)).ConfigureAwait(false);
                }
            });

            var swaggerOptions = new Options.SwaggerOptions();
            Configuration.GetSection(nameof(Options.SwaggerOptions)).Bind(swaggerOptions);

            app.UseSwagger(option =>
            {
                option.RouteTemplate = swaggerOptions.JsonRoute;
            });
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description);
            });

            // app.UseMvc();

            if (!env.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();
            app.UseCors(builder =>
            {
                builder.WithOrigins("http://*.dragonflytracker.com", "https://*.dragonflytracker.com", "http://localhost")
                            .SetIsOriginAllowedToAllowWildcardSubdomains()
                            .AllowCredentials()
                            .SetIsOriginAllowed((host) => true)
                            .AllowAnyHeader()
                            .AllowAnyMethod();
            });

            app.UseAuthentication();
            app.UseAuthorization();
            /*
            app.Use(next => context =>
            {
                return next(context);
                string path = context.Request.Path.Value;
                if (string.Equals("POST", context.Request.Method, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals("PUT", context.Request.Method, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals("PATCH", context.Request.Method, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals("DELETE", context.Request.Method, StringComparison.OrdinalIgnoreCase) || true)
                {
                    var tokens = antiforgery.GetAndStoreTokens(context);
                    context.Response.Cookies.Append(
                    "X-XSRF-TOKEN",
                    tokens.RequestToken,
                    new CookieOptions
                    {
                        HttpOnly = false,
                        Domain = "." + string.Join(".", context.Request.Host.ToString().Split('.').TakeLast(2)),
                        SameSite = SameSiteMode.None,
                        Secure = false
                    });
                }
                return next(context);
            });*/

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}")
                .RequireCors(MyAllowSpecificOrigins)
                ;
            });

            // if (!env.IsDevelopment())
            if (env.IsDevelopment())
            {
                app.UseSpa(spa =>
                {
                    // To learn more about options for serving an Angular SPA from ASP.NET Core,
                    // see https://go.microsoft.com/fwlink/?linkid=864501

                    spa.Options.SourcePath = "ClientApp";

                    if (env.IsDevelopment())
                    {
                        // spa.UseAngularCliServer(npmScript: "start");
                        spa.UseProxyToSpaDevelopmentServer("http://localhost:4205");
                    }
                    else
                    {
                        // spa.UseAngularCliServer(npmScript: "start");
                    }
                });
            }
        }
    }
}
