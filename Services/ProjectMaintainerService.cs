using DragonflyTracker.Domain;
using DragonflyTracker.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Services
{
    public class ProjectMaintainerService: IProjectMaintainerService
    {
        private readonly IProjectMaintainerRepository _projectMaintainerRepository;

        public ProjectMaintainerService(IProjectMaintainerRepository projectMaintainerRepository)
        {
            _projectMaintainerRepository = projectMaintainerRepository;
        }

        public async Task<bool> CreateProjectMaintainerAsync(ProjectMaintainer projectMaintainer)
        {
            if (projectMaintainer == null)
            {
                return false;
            }

            await _projectMaintainerRepository.CreateAsync(projectMaintainer).ConfigureAwait(false);

            var created = await _projectMaintainerRepository.SaveAsync().ConfigureAwait(false);
            return created > 0;
        }

        public async Task<(List<DragonflyUser> list, int count)> GetAllProjectMaintainersAsync(Guid projectId, PaginationFilter paginationFilter = null)
        {
            var queryable = _projectMaintainerRepository.FindByCondition(pm => pm.ProjectId == projectId).Include(pm => pm.Maintainer).Select(pm => pm.Maintainer);

            List<DragonflyUser> maintainers;
            var count = await queryable.CountAsync().ConfigureAwait(false);

            if (paginationFilter == null)
            {
                maintainers = await queryable
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
            else
            {
                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                maintainers = await queryable
                        .Skip(skip)
                        .Take(paginationFilter.PageSize)
                        .ToListAsync()
                        .ConfigureAwait(false);
            }
            return (list: maintainers, count);
        }

        public async Task<DragonflyUser> GetProjectMaintainerAsync(Guid projectId, string username)
        {
            var queryable = _projectMaintainerRepository.FindByCondition(pm => pm.ProjectId == projectId && pm.Maintainer.UserName == username).Include(pm => pm.Maintainer).Select(pm => pm.Maintainer);

            return await queryable.SingleOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<bool> DeleteProjectMaintainerAsync(Guid projectId, Guid maintainerId)
        {
            var projectMaintainer = new ProjectMaintainer { ProjectId = projectId, MaintainerId = maintainerId };
            _projectMaintainerRepository.Delete(projectMaintainer);
            var deleted = await _projectMaintainerRepository.SaveAsync().ConfigureAwait(false);
            return deleted > 0;
        }
    }
}
