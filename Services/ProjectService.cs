using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using DragonflyTracker.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Services
{
    public class ProjectService: IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IIssueService _issueService;

        public ProjectService(PgMainDataContext pgMainDataContext, IIssueService issueService, IProjectRepository projectRepository)
        {
            _issueService = issueService;
            _projectRepository = projectRepository;
        }

        public async Task<Project> GetProjectByIdAsync(Guid Id)
        {
            return await _projectRepository
                .FindByCondition(x => x.Id == Id)
                .Include(p => p.Creator)
                .SingleOrDefaultAsync()
                .ConfigureAwait(false);
        }

        public async Task<Project> GetProjectByUserAsync(string username, string projectName)
        {
            return await _projectRepository
                .FindByCondition(x => x.Name == projectName && x.Creator.UserName == username)
                .Include(p => p.Creator)
                .SingleOrDefaultAsync()
                .ConfigureAwait(false);
        }

        public async Task<Project> GetProjectByOrgAsync(string organizationName, string projectName)
        {
            return await _projectRepository
                .FindByCondition(x => x.Name == projectName && x.ParentOrganization.Name == organizationName)
                .Include(p => p.Creator)
                .SingleOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<(List<Project> list, int count)> GetProjectsAsync(GetAllProjectsFilter filter, PaginationFilter paginationFilter = null)
        {
            var queryable = _projectRepository.FindAll();

            if (!string.IsNullOrEmpty(filter?.SearchText))
            {
                queryable = _projectRepository.FindAllWithTextSearch(filter.SearchText);
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
            return (list: projects, count);
        }

        public async Task<bool> CreateProjectAsync(Project project, List<IssueType> types)
        {
            // post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            // await AddNewTags(post).ConfigureAwait(false);
            await _projectRepository.CreateAsync(project).ConfigureAwait(false);
            var created = await _projectRepository.SaveAsync().ConfigureAwait(false);

            return created > 0;
        }

        public async Task<bool> DeleteProjectAsync(Guid projectId)
        {
            var project = new Project { Id = projectId };
            _projectRepository.Delete(project);
            var deleted = await _projectRepository.SaveAsync().ConfigureAwait(false);
            return deleted > 0;
        }

        public async Task<bool> UpdateProjectAsync(Project projectToUpdate)
        {
            // projectToUpdate.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());
            // await AddNewTags(projectToUpdate).ConfigureAwait(false);
            _projectRepository.Update(projectToUpdate);
            var updated = await _projectRepository.SaveAsync().ConfigureAwait(false);
            return updated > 0;
        }

        public async Task<bool> UserOwnsProjectAsync(Guid projectId, string userId)
        {
            var project = await _projectRepository.FindByCondition(x => x.Id == projectId).SingleOrDefaultAsync().ConfigureAwait(false);

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

        public async Task<(List<Project> list, int count)> GetProjectsByOrganizationNameAsync(string organizationName, PaginationFilter paginationFilter = null)
        {
            var queryable = _projectRepository
                .FindByCondition(x => x.ParentOrganization.Name == organizationName);
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
            return (list: projects, count);
        }

        public async Task<(List<Project> list, int count)> GetProjectsByOrganizationIdAsync(Guid organizationId, PaginationFilter paginationFilter = null)
        {
            var queryable = _projectRepository
                .FindByCondition(x => x.OrganizationId == organizationId);
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
            return (list: projects, count);
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
