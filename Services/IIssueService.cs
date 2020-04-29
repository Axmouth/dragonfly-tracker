﻿using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Services
{
    public interface IIssueService
    {
        public Task<Issue> GetIssueByIdAsync(Guid issueId);

        public Task<Issue> GetIssueByUserAsync(string username, string projectName, int issueNumber);

        public Task<Issue> GetIssueByOrgAsync(string organizationName, string projectName, int issueNumber);

        public Task<bool> CreateIssueUpdateAsync(IssueUpdate issueUpdate);

        public Task<bool> CreateIssueStageAsync(IssueStage issueStage);

        public Task<bool> CreateIssueTypeAsync(IssueType issueType);

        public Task<bool> CreateIssueByUserAsync(Issue issue, string postContent, List<IssueType> types, string username, string projectName);

        public Task<bool> CreateIssueByOrgAsync(Issue issue, string postContent, List<IssueType> types, string organizationName, string projectName);

        public Task<bool> CreateIssueAsync(Issue issue, string postContent, List<IssueType> types);

        public Task<bool> UpdateIssueAsync(Issue issueToUpdate);

        public Task<bool> UpdateIssueStageAsync(IssueStage issueStageToUpdate);

        public Task<bool> UpdateIssueTypeAsync(IssueType issueTypeToUpdate);

        public Task<bool> DeleteIssueAsync(Guid issueId);

        public Task<bool> DeleteIssueStageAsync(Guid issueStageId);

        public Task<bool> DeleteIssueTypeAsync(Guid issueTypeId);

        public Task<bool> UserOwnsIssueAsync(Guid issueId, string userId);

        public Task<List<Issue>> GetIssuesByProjectIdAsync(Guid projectId, PaginationFilter paginationFilter = null);

        public Task<List<Issue>> GetIssuesAsync(GetAllIssuesFilter filter, PaginationFilter paginationFilter = null);

        public Task<List<Issue>> GetIssuesByOrganizationAndProjectNameAsync(string organizationName, string projectName, PaginationFilter paginationFilter = null);

        public Task<List<Issue>> GetIssuesByAuthorIdAsync(string authortId, PaginationFilter paginationFilter = null);

        public Task<List<Issue>> GetIssuesByAuthorUsernameAsync(string authortName, PaginationFilter paginationFilter = null);

        public Task<List<IssueUpdate>> GetIssueUpdatesInTimePeriodAsync(Guid issueId, DateTime start, DateTime end, PaginationFilter paginationFilter = null);

        public Task<List<Project>> GetProjectsByOrganizationNameAsync(string organizationName, PaginationFilter paginationFilter = null);

        public Task<List<Project>> GetProjectsByOrganizationIdAsync(Guid organizationId, PaginationFilter paginationFilter = null);

        public Task<List<Issue>> GetIssuesByProjectIdByTextSearchAsync(string authortId, PaginationFilter paginationFilter = null);



    }
}
