using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Services
{
    public interface IIssuePostService
    {

        public Task<bool> CreateIssuePostAsync(IssuePost issuePost);

        public Task<bool> CreateIssuePostReactionAsync(IssuePostReaction issuePostReaction);

        public Task<bool> UpdateIssuePostAsync(IssuePost issuePostToUpdate);

        public Task<bool> DeleteIssuePostAsync(Guid issuePostId);

        public Task<bool> DeleteIssuePostReactionAsync(Guid issuePostReactionId);

        public Task<bool> UserOwnsIssuePostAsync(Guid issuePostId, string userId);

        public Task<Tuple<List<IssuePost>, int>> GetIssuePostsByIssueAsync(Guid issueId, PaginationFilter paginationFilter = null);

        public Task<Tuple<List<IssuePost>, int>> GetAllIssuePostsAsync(GetAllIssuePostsFilter filter, PaginationFilter paginationFilter = null);
    }
}
