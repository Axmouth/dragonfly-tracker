using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using DragonflyTracker.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DragonflyTracker.Installers
{
    public class DdInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<DragonflyUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DataContext>();

            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IssueService, IssueService>();
            services.AddScoped<ProjectService, ProjectService>();
            services.AddScoped<UserService, UserService>();
            services.AddScoped<NotificationService, NotificationService>();
        }
    }
}
