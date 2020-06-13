using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Services
{
    public interface IProjectAdminService
    {
        public Task<bool> CreateProjectAdminAsync(ProjectAdmin projectAdmin);
        public Task<(List<DragonflyUser> list, int count)> GetAllProjectAdminsAsync(Guid projectId, PaginationFilter paginationFilter = null);
        public Task<DragonflyUser> GetProjectAdminAsync(Guid projectId, string username);
        public Task<bool> DeleteProjectAdminAsync(Guid projectId, Guid adminId);
    }
}
