using DragonflyTracker.Domain;
using DragonflyTracker.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Services
{
    public class ProjectAdminService: IProjectAdminService
    {
        private readonly IProjectAdminRepository _projectAdminRepository;

        public ProjectAdminService(IProjectAdminRepository projectAdminRepository)
        {
            _projectAdminRepository = projectAdminRepository;
        }

        public async Task<bool> CreateProjectAdminAsync(ProjectAdmin projectAdmin)
        {
            if (projectAdmin == null)
            {
                return false;
            }

            await _projectAdminRepository.CreateAsync(projectAdmin).ConfigureAwait(false);

            var created = await _projectAdminRepository.SaveAsync().ConfigureAwait(false);
            return created > 0;
        }

        public async Task<(List<DragonflyUser> list, int count)> GetAllProjectAdminsAsync(Guid projectId, PaginationFilter paginationFilter = null)
        {
            var queryable = _projectAdminRepository.FindByCondition(pa => pa.ProjectId == projectId).Include(pa => pa.Admin).Select(pa => pa.Admin);

            List<DragonflyUser> admins;
            var count = await queryable.CountAsync().ConfigureAwait(false);

            if (paginationFilter == null)
            {
                admins = await queryable
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
            else
            {
                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                admins = await queryable
                        .Skip(skip)
                        .Take(paginationFilter.PageSize)
                        .ToListAsync()
                        .ConfigureAwait(false);
            }
            return (list: admins, count);
        }

        public async Task<DragonflyUser> GetProjectAdminAsync(Guid projectId, string username)
        {
            var queryable = _projectAdminRepository.FindByCondition(pa => pa.ProjectId == projectId && pa.Admin.UserName == username).Include(pa => pa.Admin).Select(pa => pa.Admin);

            return await queryable.SingleOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<bool> DeleteProjectAdminAsync(Guid projectId, Guid adminId)
        {
            var projectAdmin = new ProjectAdmin { ProjectId = projectId, AdminId = adminId };
            _projectAdminRepository.Delete(projectAdmin);
            var deleted = await _projectAdminRepository.SaveAsync().ConfigureAwait(false);
            return deleted > 0;
        }
    }
}
