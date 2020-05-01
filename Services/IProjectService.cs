﻿using DragonflyTracker.Domain;
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
        public Task<Tuple<List<Project>, int>> GetProjectsAsync(GetAllProjectsFilter filter, PaginationFilter paginationFilter = null);
        public Task<bool> CreateProjectAsync(Project project, List<IssueType> types);
        public Task<bool> DeleteProjectAsync(Guid projectId);
        public Task<bool> UpdateProjectAsync(Project projectToUpdate);
        public Task<bool> UserOwnsProjectAsync(Guid projectId, string userId);

        public Task<Tuple<List<Project>, int>> GetProjectsByOrganizationNameAsync(string organizationName, PaginationFilter paginationFilter = null);

        public Task<Tuple<List<Project>, int>> GetProjectsByOrganizationIdAsync(Guid organizationId, PaginationFilter paginationFilter = null);
    }
}
