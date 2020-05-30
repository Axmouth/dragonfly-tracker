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
        Task<bool> UpdateIssueTypeAsync(IssueType issueTypeToUpdate);
        Task<bool> DeleteIssueTypeAsync(Guid issueTypeId);
        Task<(List<IssueType> list, int count)> GetIssueTypesByIssueAsync(Guid issueId, PaginationFilter paginationFilter = null);
        Task<(List<IssueType> list, int count)> GetIssueTypesByProjectAsync(Guid projectId, PaginationFilter paginationFilter = null);
    }
}
