using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Services
{
    public class ProjectService: IProjectService
    {
        private readonly PgMainDataContext _pgMainDataContext;
        private readonly IIssueService _issueService;

        public ProjectService(PgMainDataContext pgMainDataContext, IIssueService issueService)
        {
            _pgMainDataContext = pgMainDataContext;
            _issueService = issueService;
        }

        public async Task<Project> GetProjectByIdAsync(Guid Id)
        {
            return await _pgMainDataContext.Projects
                .Include(p => p.Creator)
                .SingleOrDefaultAsync(x => x.Id == Id).ConfigureAwait(false);
        }

        public async Task<Project> GetProjectByUserAsync(string username, string projectName)
        {
            return await _pgMainDataContext.Projects
                .Include(p => p.Creator)
                .SingleOrDefaultAsync(x => x.Name == projectName && x.Creator.UserName == username).ConfigureAwait(false);
        }

        public async Task<Project> GetProjectByOrgAsync(string organizationName, string projectName)
        {
            return await _pgMainDataContext.Projects
                .Include(p => p.Creator)
                .SingleOrDefaultAsync(x => x.Name == projectName && x.ParentOrganization.Name == organizationName).ConfigureAwait(false);
        }

        public async Task<Tuple<List<Project>, int>> GetProjectsAsync(GetAllProjectsFilter filter, PaginationFilter paginationFilter = null)
        {
            var queryable = _pgMainDataContext.Projects.AsQueryable();

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

            List<Project> projects;
            var count = await queryable.CountAsync().ConfigureAwait(false);

            if (paginationFilter == null)
            {
                projects = await queryable
                    .Include(x => x.ParentOrganization)
                    .Include(x => x.Creator)
                    .ToListAsync()
                    .ConfigureAwait(false);
            } else
            {
                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                projects = await queryable
                        .Include(x => x.ParentOrganization)
                        .Include(x => x.Creator)
                        .Skip(skip)
                        .Take(paginationFilter.PageSize)
                        .ToListAsync()
                        .ConfigureAwait(false);
            }

            return Tuple.Create(projects, count);
        }

        public async Task<bool> CreateProjectAsync(Project project, List<IssueType> types)
        {
            // post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            // await AddNewTags(post).ConfigureAwait(false);
            await _pgMainDataContext.Projects.AddAsync(project);
            var created = await _pgMainDataContext.SaveChangesAsync().ConfigureAwait(false);

            return created > 0;
        }

        public async Task<bool> DeleteProjectAsync(Guid projectId)
        {
            var project = new Project { Id = projectId };
            _pgMainDataContext.Projects.Attach(project);
            _pgMainDataContext.Projects.Remove(project);
            var deleted = await _pgMainDataContext.SaveChangesAsync().ConfigureAwait(false);
            return deleted > 0;
        }

        public async Task<bool> UpdateProjectAsync(Project projectToUpdate)
        {
            // projectToUpdate.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());
            // await AddNewTags(projectToUpdate).ConfigureAwait(false);
            _pgMainDataContext.Projects.Update(projectToUpdate);
            var updated = await _pgMainDataContext.SaveChangesAsync().ConfigureAwait(false);
            return updated > 0;
        }

        public async Task<bool> UserOwnsProjectAsync(Guid projectId, string userId)
        {
            var project = await _pgMainDataContext.Projects.AsNoTracking().SingleOrDefaultAsync(x => x.Id == projectId).ConfigureAwait(false);

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

        public async Task<Tuple<List<Project>, int>> GetProjectsByOrganizationNameAsync(string organizationName, PaginationFilter paginationFilter = null)
        {
            var queryable = _pgMainDataContext.Projects.AsQueryable()
                    .Where(x => x.ParentOrganization.Name == organizationName);
            List<Project> projects;
            var count = await queryable.CountAsync().ConfigureAwait(false);

            if (paginationFilter == null)
            {
                projects = await queryable
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
            else
            {
                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                projects = await queryable
                        .Skip(skip)
                        .Take(paginationFilter.PageSize)
                        .ToListAsync()
                        .ConfigureAwait(false);
            }
            return Tuple.Create(projects, count);
        }

        public async Task<Tuple<List<Project>, int>> GetProjectsByOrganizationIdAsync(Guid organizationId, PaginationFilter paginationFilter = null)
        {
            var queryable = _pgMainDataContext.Projects.AsQueryable()
                    .Where(x => x.OrganizationId == organizationId);
            List<Project> projects;
            var count = await queryable.CountAsync().ConfigureAwait(false);

            if (paginationFilter == null)
            {
                projects = await queryable
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
            else
            {
                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                projects = await queryable
                        .Skip(skip)
                        .Take(paginationFilter.PageSize)
                        .ToListAsync()
                        .ConfigureAwait(false);
            }
            return Tuple.Create(projects, count);
        }

        private async Task AddIssueStages(Issue issue)
        {/*
            foreach (var type in issue.Types)
            {
                var existingIssueType =
                    await _dataContext.IssueTypes.SingleOrDefaultAsync(x =>
                        x.Name == type.Name && x.ProjectId == type.ProjectId).ConfigureAwait(false);
                if (existingIssueType != null)
                    continue;

                await _dataContext.IssueTypes.AddAsync(new IssueType
                { Name = type.Name, ProjectId = issue.ProjectId });
            }*/
        }
    }
}
