using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DragonflyTracker.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DragonflyTracker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<PgMainDataContext>();

                await dbContext.Database.MigrateAsync().ConfigureAwait(false);

                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync("Admin").ConfigureAwait(false))
                {
                    var adminRole = new IdentityRole("Admin");
                    await roleManager.CreateAsync(adminRole).ConfigureAwait(false);
                }

                if (!await roleManager.RoleExistsAsync("Poster").ConfigureAwait(false))
                {
                    var posterRole = new IdentityRole("Poster");
                    await roleManager.CreateAsync(posterRole).ConfigureAwait(false);
                }

                if (!await roleManager.RoleExistsAsync("Member").ConfigureAwait(false))
                {
                    var memberRole = new IdentityRole("Member");
                    await roleManager.CreateAsync(memberRole).ConfigureAwait(false);
                }

                if (!await roleManager.RoleExistsAsync("Ghost").ConfigureAwait(false))
                {
                    var ghostRole = new IdentityRole("Ghost");
                    await roleManager.CreateAsync(ghostRole).ConfigureAwait(false);
                }
            }

            await host.RunAsync().ConfigureAwait(false);
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                          .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
                          .AddJsonFile($"appsettings.Local.json", optional: true, reloadOnChange: true)
                          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .UseStartup<Startup>();
        }
    }
}
