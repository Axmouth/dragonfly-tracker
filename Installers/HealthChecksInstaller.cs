using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DragonflyTracker.Data;
using DragonflyTracker.HealthChecks;

namespace DragonflyTracker.Installers
{
    public class HealthChecksInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddDbContextCheck<PgMainDataContext>()
                .AddCheck<RedisHealthCheck>("Redis");
        }
    }
}