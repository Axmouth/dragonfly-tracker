using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Services
{
    public interface IIssueStageService
    {
        Task<bool> CreateIssueStageAsync(IssueStage issueStage);
        Task<bool> UpdateIssueStageAsync(IssueStage issueStageToUpdate);
        Task<bool> DeleteIssueStageAsync(Guid issueStageId);
        Task<IssueStage> GetIssueStageByIssueAsync(Guid issueId);
        Task<(List<IssueStage> list, int count)> GetIssueStagesByProjectAsync(Guid projectId, PaginationFilter paginationFilter = null);
        Task<IssueStage> GetIssueStageByProjectAsync(Guid projectId, string name);
    }
}
