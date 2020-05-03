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
            services.AddDbContext<PgMainDataContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("MainConnection")
                )
            //.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            );
            services.AddDefaultIdentity<DragonflyUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<PgMainDataContext>();

            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IIssueService, IssueService>();
            services.AddScoped<IIssuePostService, IssuePostService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IUriService, RestfulUriService>();
        }
    }
}
