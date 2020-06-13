using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Services
{
    public interface IProjectService
    {
        public Task<Project> GetProjectByIdAsync(Guid Id);
        public Task<Project> GetProjectByUserAsync(string username, string projectName);
        public Task<Project> GetProjectByOrgAsync(string organizationName, string projectName);
        public Task<(List<Project> list, int count)> GetProjectsAsync(GetAllProjectsFilter filter, PaginationFilter paginationFilter = null);
        public Task<bool> CreateProjectAsync(Project project, List<IssueType> types, List<IssueStage> stages, List<DragonflyUser> admins, List<DragonflyUser> maintainers);
        public Task<bool> DeleteProjectAsync(Guid projectId);
        public Task<bool> UpdateProjectAsync(Project projectToUpdate, List<IssueType> types, List<IssueStage> stages, List<DragonflyUser> admins, List<DragonflyUser> maintainers);
        public Task<bool> UserOwnsProjectAsync(Guid projectId, Guid userId);
        public Task<(List<Project> list, int count)> GetProjectsByOrganizationNameAsync(string organizationName, PaginationFilter paginationFilter = null);
        public Task<(List<Project> list, int count)> GetProjectsByOrganizationIdAsync(Guid organizationId, PaginationFilter paginationFilter = null);
        public Task<bool> UserCanAdminProject(Project project, Guid userId);
        public Task<bool> UserCanAdminProject(Project project);
        public Task<bool> UserCanAdminProject(Guid projectId);
        public Task<bool> UserCanAdminProject(Guid projectId, Guid userId);
        public Task<bool> UserCanMaintainProject(Project project, Guid userId);
        public Task<bool> UserCanMaintainProject(Project project);
        public Task<bool> UserCanMaintainProject(Guid projectId);
        public Task<bool> UserCanMaintainProject(Guid projectId, Guid userId);
    }
}
