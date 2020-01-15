using Microsoft.AspNetCore.Authorization;

namespace DragonflyTracker.Authorization
{
    public class WorksForCompanyRequirement : IAuthorizationRequirement
    {
        public string DomainName { get; }
        
        public WorksForCompanyRequirement(string domainName)
        {
            DomainName = domainName;
        }
    }
}