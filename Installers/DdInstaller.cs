using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using DragonflyTracker.Repositories;
using DragonflyTracker.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
namespace DragonflyTracker.Installers
{
    public class DdInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PgMainDataContext>(options =>{

                options.UseNpgsql(
                    configuration.GetConnectionString("PgMainConnection")
                    );
                } 
            )
            ;

            services.AddIdentity<DragonflyUser, IdentityRole<Guid>>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-.";
                })
                .AddEntityFrameworkStores<PgMainDataContext>()
                .AddDefaultTokenProviders()
                ;

            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IIssueService, IssueService>();
            services.AddScoped<IIssuePostService, IssuePostService>();
            services.AddScoped<IIssueStageService, IssueStageService>();
            services.AddScoped<IIssueTypeService, IssueTypeService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<INotificationService, NotificationService>();

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
