using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using DragonflyTracker.Repositories;
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
                configuration.GetConnectionString("PgMainConnection")
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

            services.AddScoped<IProjectRepository, ProjectPgRepository>();
            services.AddScoped<IIssueRepository, IssuePgRepository>();
            services.AddScoped<IIssuePostRepository, IssuePostPgRepository>();
            services.AddScoped<IIssueStageRepository, IssueStagePgRepositorycs>();
            services.AddScoped<IIssueTypeRepository, IssueTypePgRepository>();
            services.AddScoped<IIssueUpdateRepository, IssueUpdatePgRepository>();
            services.AddScoped<IIssuePostReactionRepository, IssuePostReactionPgRepository>();
            services.AddScoped<IIssueIssueTypeRepository, IssueIssueTypePgRepository>();
            services.AddScoped<INotificationRepository, NotificationPgRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationPgRepository>();
            services.AddScoped<IProjectAdminRepository, ProjectAdminPgRepository>();
            services.AddScoped<IProjectMaintainerRepository, ProjectMaintainerPgRepository>();
            services.AddScoped<IUserRepository, UserPgRepository>();
        }
    }
}
