using DragonflyTracker.Domain;
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

        public Task<bool> CreateIssueByUserAsync(Issue issue, List<IssueType> types, string username, string projectName);

        public Task<bool> CreateIssueByOrgAsync(Issue issue, List<IssueType> types, string organizationName, string projectName);

        public Task<bool> CreateIssueAsync(Issue issue, List<IssueType> types);

        public Task<bool> UpdateIssueAsync(Issue issueToUpdate);

        public Task<bool> UpdateIssueStageAsync(IssueStage issueStageToUpdate);

        public Task<bool> UpdateIssueTypeAsync(IssueType issueTypeToUpdate);

        public Task<bool> DeleteIssueAsync(Guid issueId);

        public Task<bool> DeleteIssueStageAsync(Guid issueStageId);

        public Task<bool> DeleteIssueTypeAsync(Guid issueTypeId);

        public Task<bool> UserOwnsIssueAsync(Guid issueId, string userId);

        public Task<Tuple<List<Issue>, int>> GetIssuesByProjectIdAsync(Guid projectId, PaginationFilter paginationFilter = null);

        public Task<Tuple<List<Issue>, int>> GetIssuesAsync(GetAllIssuesFilter filter, PaginationFilter paginationFilter = null);

        public Task<Tuple<List<Issue>, int>> GetIssuesByOrganizationAndProjectNameAsync(string organizationName, string projectName, PaginationFilter paginationFilter = null);

        public Task<Tuple<List<Issue>, int>> GetIssuesByAuthorIdAsync(string authortId, PaginationFilter paginationFilter = null);

        public Task<Tuple<List<Issue>, int>> GetIssuesByAuthorUsernameAsync(string authortName, PaginationFilter paginationFilter = null);

        public Task<Tuple<List<IssueUpdate>, int>> GetIssueUpdatesInTimePeriodAsync(Guid issueId, DateTime start, DateTime end, PaginationFilter paginationFilter = null);

        public Task<Tuple<List<Issue>, int>> GetIssuesByProjectIdByTextSearchAsync(string authortId, PaginationFilter paginationFilter = null);



    }
}
