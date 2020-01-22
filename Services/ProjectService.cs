using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Services
{
    public class ProjectService
    {
        private readonly DataContext _dataContext;

        public ProjectService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Project> GetProjectByIdAsync(Guid Id)
        {
            return await _dataContext.Projects
                .Include(p => p.Creator)
                .SingleOrDefaultAsync(x => x.Id == Id).ConfigureAwait(false);
        }

        public async Task<Project> GetProjectByUserAsync(string username, string projectName)
        {
            return await _dataContext.Projects
                .Include(p => p.Creator)
                .SingleOrDefaultAsync(x => x.Name == projectName && x.Creator.UserName == username).ConfigureAwait(false);
        }

        public async Task<Project> GetProjectByOrgAsync(string organizationName, string projectName)
        {
            return await _dataContext.Projects
                .Include(p => p.Creator)
                .SingleOrDefaultAsync(x => x.Name == projectName && x.ParentOrganization.Name == organizationName).ConfigureAwait(false);
        }

        public async Task<List<Project>> GetProjectsAsync(GetAllProjectsFilter filter, PaginationFilter paginationFilter = null)
        {
            var queryable = _dataContext.Projects.AsQueryable();

            if (!string.IsNullOrEmpty(filter?.SearchText))
            {
                queryable = queryable
                    .Where(p => EF.Functions.ToTsVector("english", p.Name + ' ' + p.Description).Matches(filter.SearchText));
            }

            if (!string.IsNullOrEmpty(filter?.CreatorUsername))
            {
                queryable = queryable
                    .Include(x => x.ParentOrganization)
                    .Include(p => p.Creator)
                    .Where(i => i.Creator.UserName == filter.CreatorUsername);
            }

            if (paginationFilter == null)
            {
                return await queryable
                    .Include(x => x.ParentOrganization)
                    .Include(x => x.Creator)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await queryable
                    .Include(x => x.ParentOrganization)
                    .Include(x => x.Creator)
                    .Skip(skip)
                    .Take(paginationFilter.PageSize)
                    .ToListAsync()
                    .ConfigureAwait(false);
        }

        public async Task<bool> CreateProjectAsync(Project project, List<IssueType> types)
        {
            // post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            // await AddNewTags(post).ConfigureAwait(false);
            await _dataContext.Projects.AddAsync(project);
            var created = await _dataContext.SaveChangesAsync().ConfigureAwait(false);

            return created > 0;
        }

        public async Task<bool> DeleteProjectAsync(Guid projectId)
        {
            var project = new Project { Id = projectId };

            _dataContext.Projects.Attach(project);


            _dataContext.Projects.Remove(project);
            var deleted = await _dataContext.SaveChangesAsync().ConfigureAwait(false);
            return deleted > 0;
        }

        public async Task<bool> UpdateProjectAsync(Project projectToUpdate)
        {
            // projectToUpdate.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());
            // await AddNewTags(projectToUpdate).ConfigureAwait(false);
            _dataContext.Projects.Update(projectToUpdate);
            var updated = await _dataContext.SaveChangesAsync().ConfigureAwait(false);
            return updated > 0;
        }

        public async Task<bool> UserOwnsProjectAsync(Guid projectId, string userId)
        {
            var project = await _dataContext.Projects.AsNoTracking().SingleOrDefaultAsync(x => x.Id == projectId).ConfigureAwait(false);

            if (project == null)
            {
                return false;
            }

            if (project.UserId != userId)
            {
                return false;
            }

            return true;
        }
    }
}
