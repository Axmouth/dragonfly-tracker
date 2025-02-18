﻿using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using DragonflyTracker.Extensions;
using DragonflyTracker.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IIssueService _issueService;
        private readonly IIssueStageRepository _issueStageRepository;
        private readonly IIssueTypeRepository _issueTypeRepository;
        private readonly IProjectAdminRepository _projectAdminRepository;
        private readonly IProjectMaintainerRepository _projectMaintainerRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<DragonflyUser> _userManager;

        public ProjectService(PgMainDataContext pgMainDataContext, IIssueService issueService, IProjectRepository projectRepository,
            IIssueStageRepository issueStageRepository, IIssueTypeRepository issueTypeRepository, IProjectAdminRepository projectAdminRepository,
            IProjectMaintainerRepository projectMaintainerRepository, IHttpContextAccessor httpContextAccessor, UserManager<DragonflyUser> userManager)
        {
            _issueService = issueService;
            _projectRepository = projectRepository;
            _issueStageRepository = issueStageRepository;
            _issueTypeRepository = issueTypeRepository;
            _projectAdminRepository = projectAdminRepository;
            _projectMaintainerRepository = projectMaintainerRepository;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<Project> GetProjectByIdAsync(Guid Id)
        {
            var queryable = _projectRepository
                    .FindByCondition(x => x.Id == Id);
            queryable = AddPrivateCheck(queryable);
            queryable = AddIncludes(queryable);
            return await queryable
                .SingleOrDefaultAsync()
                .ConfigureAwait(false);
        }

        public async Task<Project> GetProjectByUserAsync(string username, string projectName)
        {
            var queryable = _projectRepository
                    .FindByCondition(x => x.Name == projectName && x.Creator.UserName == username);
            queryable = AddPrivateCheck(queryable);
            queryable = AddIncludes(queryable);
            return await queryable
                .SingleOrDefaultAsync()
                .ConfigureAwait(false);
        }

        public async Task<Project> GetProjectByOrgAsync(string organizationName, string projectName)
        {
            var queryable = _projectRepository
                .FindByCondition(x => x.Name == projectName && x.ParentOrganization.Name == organizationName);
            queryable = AddPrivateCheck(queryable);
            queryable = AddIncludes(queryable);
            return await queryable
                .SingleOrDefaultAsync()
                .ConfigureAwait(false);
        }

        public async Task<(List<Project> list, int count)> GetProjectsAsync(GetAllProjectsFilter filter, PaginationFilter paginationFilter = null)
        {
            List<Project> projects;
            var queryable = _projectRepository
                    .FindAll();

            if (!string.IsNullOrEmpty(filter?.SearchText))
            {
                queryable = _projectRepository
                    .FindAllWithTextSearch(filter.SearchText);
            }

            queryable = AddPrivateCheck(queryable);
            queryable = AddIncludes(queryable);

            if (!string.IsNullOrEmpty(filter?.CreatorUsername))
            {
                queryable = queryable
                    .Where(p => p.Creator.UserName == filter.CreatorUsername);
            }

            if (!string.IsNullOrEmpty(filter?.Admin))
            {
                queryable = queryable
                    .Where(p => p.Admins.Any(a => a.Admin.UserName == filter.Admin) || p.Owner.UserName == filter.Admin);
            }

            if (!string.IsNullOrEmpty(filter?.Maintainer))
            {
                queryable = queryable
                    .Where(p => p.Maintainers.Any(m => m.Maintainer.UserName == filter.Maintainer) || p.Owner.UserName == filter.Maintainer);
            }

            if (filter.Admined.HasValue && filter.Admined == true && filter.CurrentUserId != null)
            {
                queryable = queryable
                    .Where(p => p.Admins.Any(a => a.AdminId == filter.CurrentUserId) || p.OwnerId == filter.CurrentUserId);
            }

            if (filter.Maintained.HasValue && filter.Maintained == true && filter.CurrentUserId != null)
            {
                queryable = queryable
                    .Where(p => p.Maintainers.Any(m => m.MaintainerId == filter.CurrentUserId) || p.OwnerId == filter.CurrentUserId);
            }
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

        public async Task<bool> CreateProjectAsync(Project project, List<IssueType> types, List<IssueStage> stages, List<DragonflyUser> admins, List<DragonflyUser> maintainers)
        {
            if (project == null)
            {
                return false;
            }
            await _projectRepository.CreateAsync(project).ConfigureAwait(false);
            var created = await _projectRepository.SaveAsync().ConfigureAwait(false);
            if (types != null)
            {
                await UpdateIssueTypes(project, types).ConfigureAwait(false);
            }
            if (stages != null)
            {
                await AddIssueStages(project, stages).ConfigureAwait(false);
            }
            if (admins != null)
            {
                await AddProjectAdmins(project, admins).ConfigureAwait(false);
            }
            if (maintainers != null)
            {
                await AddProjectMaintainers(project, maintainers).ConfigureAwait(false);
            }
            return created > 0;
        }

        public async Task<bool> DeleteProjectAsync(Guid projectId)
        {
            var project = new Project { Id = projectId };
            _projectRepository.Delete(project);
            var deleted = await _projectRepository.SaveAsync().ConfigureAwait(false);
            return deleted > 0;
        }

        public async Task<bool> UpdateProjectAsync(Project projectToUpdate, List<IssueType> types, List<IssueStage> stages, List<DragonflyUser> admins, List<DragonflyUser> maintainers)
        {
            if (projectToUpdate == null)
            {
                return false;
            }
            _projectRepository.Update(projectToUpdate);
            var updated = await _projectRepository.SaveAsync().ConfigureAwait(false);
            if (types != null)
            {
                await UpdateIssueTypes(projectToUpdate, types).ConfigureAwait(false);
            }
            if (stages != null)
            {
                await AddIssueStages(projectToUpdate, stages).ConfigureAwait(false);
            }
            if (admins != null)
            {
                await AddProjectAdmins(projectToUpdate, admins).ConfigureAwait(false);
            }
            if (maintainers != null)
            {
                await AddProjectMaintainers(projectToUpdate, maintainers).ConfigureAwait(false);
            }
            return updated > 0;
        }

        public async Task<bool> UserOwnsProjectAsync(Guid projectId, Guid userId)
        {
            var project = await _projectRepository.FindByCondition(x => x.Id == projectId).SingleOrDefaultAsync().ConfigureAwait(false);

            if (project == null)
            {
                return false;
            }

            if (project.OwnerId != userId)
            {
                return false;
            }

            return true;
        }

        public async Task<(List<Project> list, int count)> GetProjectsByOrganizationNameAsync(string organizationName, PaginationFilter paginationFilter = null)
        {
            var queryable = _projectRepository
                .FindByCondition(x => x.ParentOrganization.Name == organizationName);
            queryable = AddPrivateCheck(queryable);
            queryable = AddIncludes(queryable);
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
            queryable = AddPrivateCheck(queryable);
            queryable = AddIncludes(queryable);
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

        private async Task UpdateIssueTypes(Project project, List<IssueType> types)
        {
            for (int i = 0; i < types.Count; i++)
            {
                var existingType =
                    await _issueTypeRepository
                    .FindByCondition(x =>
                        x.Name == types[i].Name && x.ProjectId == project.Id)
                    .SingleOrDefaultAsync().ConfigureAwait(false);
                if (existingType != null)
                {
                    types[i] = existingType;
                    continue;
                }
                else
                {
                    var newType = new IssueType { Id = Guid.NewGuid(), Name = types[i].Name, ProjectId = project.Id };
                    await _issueTypeRepository.CreateAsync(newType).ConfigureAwait(false);
                }

            }
            project.Types = types;
            _projectRepository.Update(project);
            await _projectRepository.SaveAsync().ConfigureAwait(false);
            await _issueTypeRepository.SaveAsync().ConfigureAwait(false);
        }


        private async Task AddIssueTypes(Project project, List<IssueType> types)
        {
            foreach (var type in types)
            {
                var existingType =
                    await _issueTypeRepository
                    .FindByCondition(x =>
                        x.Name == type.Name && x.ProjectId == project.Id)
                    .SingleOrDefaultAsync().ConfigureAwait(false);
                if (existingType != null)
                {
                    continue;
                }
                else
                {
                    var newType = new IssueType { Id = Guid.NewGuid(), Name = type.Name, ProjectId = project.Id };
                    await _issueTypeRepository.CreateAsync(newType).ConfigureAwait(false);
                }

            }
            await _issueTypeRepository.SaveAsync().ConfigureAwait(false);
        }


        private async Task AddIssueStages(Project project, List<IssueStage> stages)
        {
            foreach (var stage in stages)
            {
                var existingStage =
                    await _issueStageRepository
                    .FindByCondition(x =>
                        x.Name == stage.Name && x.ProjectId == stage.ProjectId)
                    .SingleOrDefaultAsync().ConfigureAwait(false);
                if (existingStage != null)
                    continue;

                await _issueStageRepository.CreateAsync(new IssueStage
                { Name = stage.Name, ProjectId = project.Id }).ConfigureAwait(false);
            }
            await _issueStageRepository.SaveAsync().ConfigureAwait(false);
        }


        private async Task AddProjectAdmins(Project project, List<DragonflyUser> admins)
        {
            foreach (var admin in admins)
            {
                var existingStage =
                    await _projectAdminRepository
                    .FindByCondition(x =>
                        x.Admin.UserName == admin.UserName && x.ProjectId == project.Id)
                    .SingleOrDefaultAsync().ConfigureAwait(false);
                if (existingStage != null)
                    continue;
                var _admin = await _userManager.FindByNameAsync(admin.UserName).ConfigureAwait(false);
                if (_admin == null)
                {
                    continue;
                }

                await _projectAdminRepository.CreateAsync(new ProjectAdmin
                { AdminId = _admin.Id, ProjectId = project.Id }).ConfigureAwait(false);
            }
            await _projectAdminRepository.SaveAsync().ConfigureAwait(false);
        }


        private async Task AddProjectMaintainers(Project project, List<DragonflyUser> maintainers)
        {
            foreach (var maintainer in maintainers)
            {
                var existingStage =
                    await _projectMaintainerRepository
                    .FindByCondition(x =>
                        x.Maintainer.UserName == maintainer.UserName && x.ProjectId == project.Id)
                    .SingleOrDefaultAsync().ConfigureAwait(false);
                if (existingStage != null)
                    continue;
                var _maintainer = await _userManager.FindByNameAsync(maintainer.UserName).ConfigureAwait(false);
                if (_maintainer == null)
                {
                    continue;
                }

                await _projectMaintainerRepository.CreateAsync(new ProjectMaintainer
                { MaintainerId = _maintainer.Id, ProjectId = project.Id }).ConfigureAwait(false);
            }
            await _projectMaintainerRepository.SaveAsync().ConfigureAwait(false);
        }

        public async Task<bool> UserCanAdminProject(Project project, Guid userId) {

            var authorized = project.OwnerId == userId || project.Admins.Any(a => a.AdminId == userId);

            return authorized;
        }

        public async Task<bool> UserCanAdminProject(Project project)
        {
            var userId = _httpContextAccessor.HttpContext.GetUserId();

            var authorized = project.OwnerId == userId || project.Admins.Any(a => a.AdminId == userId);

            return authorized;
        }

        public async Task<bool> UserCanAdminProject(Guid projectId)
        {
            var project = await GetProjectByIdAsync(projectId).ConfigureAwait(false);
            var userId = _httpContextAccessor.HttpContext.GetUserId();

            var authorized = project.OwnerId == userId || project.Admins.Any(a => a.AdminId == userId);

            return authorized;
        }

        public async Task<bool> UserCanAdminProject(Guid projectId, Guid userId)
        {
            var project = await GetProjectByIdAsync(projectId).ConfigureAwait(false);

            var authorized = project.OwnerId == userId || project.Admins.Any(a => a.AdminId == userId);

            return authorized;
        }

        public async Task<bool> UserCanMaintainProject(Project project, Guid userId)
        {

            var authorized = project.OwnerId == userId || project.Admins.Any(a => a.AdminId == userId) || project.Maintainers.Any(m => m.MaintainerId == userId);

            return authorized;
        }

        public async Task<bool> UserCanMaintainProject(Project project)
        {
            var userId = _httpContextAccessor.HttpContext.GetUserId();

            var authorized = project.OwnerId == userId || project.Admins.Any(a => a.AdminId == userId) || project.Maintainers.Any(m => m.MaintainerId == userId);

            return authorized;
        }

        public async Task<bool> UserCanMaintainProject(Guid projectId)
        {
            var project = await GetProjectByIdAsync(projectId).ConfigureAwait(false);
            var userId = _httpContextAccessor.HttpContext.GetUserId();

            var authorized = project.OwnerId == userId || project.Admins.Any(a => a.AdminId == userId) || project.Maintainers.Any(m => m.MaintainerId == userId);

            return authorized;
        }

        public async Task<bool> UserCanMaintainProject(Guid projectId, Guid userId)
        {
            var project = await GetProjectByIdAsync(projectId).ConfigureAwait(false);

            var authorized = project.OwnerId == userId || project.Admins.Any(a => a.AdminId == userId) || project.Maintainers.Any(m => m.MaintainerId == userId);

            return authorized;
        }

        private IQueryable<Project> AddPrivateCheck(IQueryable<Project> queryable) {

            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated) {
                return queryable.Where(p => p.Private == false);
            }
            
            var userId = _httpContextAccessor.HttpContext.GetUserId();

            queryable = queryable.Where(p => p.Private == false ||
            p.OwnerId == userId ||
            p.Admins.Any(a => a.AdminId == userId) ||
            p.Maintainers.Any(m => m.MaintainerId == userId));

            return queryable;
        }

        private static IIncludableQueryable<Project, DragonflyUser> AddIncludes(IQueryable<Project> queryable) {
            return  queryable
                    .Include(p => p.ParentOrganization)
                    .Include(p => p.Creator)
                    .Include(p => p.Owner)
                    .Include(p => p.Stages)
                    .Include(p => p.Types)
                    .Include(p => p.Admins)
                    .ThenInclude(a => a.Admin)
                    .Include(p => p.Maintainers)
                    .ThenInclude(m => m.Maintainer);
        }
    }
}
