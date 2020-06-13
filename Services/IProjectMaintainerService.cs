using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Services
{
    public interface IProjectMaintainerService
    {
        public Task<bool> CreateProjectMaintainerAsync(ProjectMaintainer projectMaintainer);
        public Task<(List<DragonflyUser> list, int count)> GetAllProjectMaintainersAsync(Guid projectId, PaginationFilter paginationFilter = null);
        public Task<DragonflyUser> GetProjectMaintainerAsync(Guid projectId, string username);
        public Task<bool> DeleteProjectMaintainerAsync(Guid projectId, Guid maintainerId);
    }
}

