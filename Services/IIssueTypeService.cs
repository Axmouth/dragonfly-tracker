using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Services
{
    public interface IIssueTypeService
    {
        Task<bool> CreateIssueTypeAsync(IssueType issueType);
        Task<bool> CreateIssueIssueTypeAsync(IssueIssueType issueIssueType);
        Task<bool> UpdateIssueTypeAsync(IssueType issueTypeToUpdate);
        Task<bool> DeleteIssueTypeAsync(Guid issueTypeId);
        Task<bool> DeleteIssueIssueTypeAsync(Guid issueId, Guid issueTypeId);
        Task<(List<IssueType> list, int count)> GetAllIssueTypesByIssueAsync(Guid issueId, PaginationFilter paginationFilter = null);
        Task<(List<IssueType> list, int count)> GetAllIssueTypesByProjectAsync(Guid projectId, PaginationFilter paginationFilter = null);
        Task<IssueType> GetIssueTypeByProjectAsync(Guid projectId, string name);
    }
}
