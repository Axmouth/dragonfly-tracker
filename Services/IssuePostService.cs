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
    public class IssuePostService : IIssuePostService
    {
        private readonly IIssuePostRepository _issuePostRepository;
        private readonly IIssuePostReactionRepository _issuePostReactionRepository;

        public IssuePostService(PgMainDataContext pgMainDataContext, IIssuePostRepository issuePostRepository, IIssuePostReactionRepository issuePostReactionRepository)
        {
            _issuePostRepository = issuePostRepository;
            _issuePostReactionRepository = issuePostReactionRepository;
        }

        public async Task<bool> CreateIssuePostAsync(IssuePost issuePost)
        {
            // post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            // await AddNewTags(post).ConfigureAwait(false);
            await _issuePostRepository.CreateAsync(issuePost).ConfigureAwait(false);

            var created = await _issuePostRepository.SaveAsync().ConfigureAwait(false);
            return created > 0;
        }

        public async Task<bool> CreateIssuePostReactionAsync(IssuePostReaction issuePostReaction)
        {
            // post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            // await AddNewTags(post).ConfigureAwait(false);
            await _issuePostReactionRepository.CreateAsync(issuePostReaction).ConfigureAwait(false);

            var created = await _issuePostReactionRepository.SaveAsync().ConfigureAwait(false);
            return created > 0;
        }

        public async Task<bool> UpdateIssuePostAsync(IssuePost issuePostToUpdate)
        {
            // postToUpdate.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());
            // await AddNewTags(postToUpdate).ConfigureAwait(false);
            _issuePostRepository.Update(issuePostToUpdate);
            var updated = await _issuePostRepository.SaveAsync().ConfigureAwait(false);
            return updated > 0;
        }

        public async Task<bool> DeleteIssuePostAsync(Guid issuePostId)
        {
            var issuePost = new IssuePost { Id = issuePostId };
            _issuePostRepository.Delete(issuePost);
            var deleted = await _issuePostRepository.SaveAsync().ConfigureAwait(false);
            return deleted > 0;
        }

        public async Task<bool> DeleteIssuePostReactionAsync(Guid issuePostReactionId)
        {
            var issuePostReaction = new IssuePostReaction { Id = issuePostReactionId };
            _issuePostReactionRepository.Delete(issuePostReaction);
            var deleted = await _issuePostReactionRepository.SaveAsync().ConfigureAwait(false);
            return deleted > 0;
        }

        public async Task<bool> UserOwnsIssuePostAsync(Guid issuePostId, Guid userId)
        {
            var issue = await _issuePostRepository
                .FindByCondition(x => x.Id == issuePostId)
                .SingleOrDefaultAsync()
                .ConfigureAwait(false);

            if (issue == null)
            {
                return false;
            }

            if (issue.AuthorId != userId)
            {
                return false;
            }

            return true;
        }

        public async Task<(List<IssuePost> list, int count)> GetIssuePostsByIssueAsync(Guid issueId, PaginationFilter paginationFilter = null)
        {
            var queryable = _issuePostRepository
                .FindByCondition(x => x.IssueId == issueId)
                .Include(x => x.Author)
                .Include(x => x.Reactions);
            List<IssuePost> issuePosts;
            var count = await queryable.CountAsync().ConfigureAwait(false);

            if (paginationFilter == null)
            {
                issuePosts = await queryable
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
            else
            {
                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                issuePosts = await queryable
                        .Skip(skip)
                        .Take(paginationFilter.PageSize)
                        .ToListAsync()
                        .ConfigureAwait(false);
            }
            return (list: issuePosts, count);
        }

        public async Task<(List<IssuePost> list, int count)> GetAllIssuePostsAsync(GetAllIssuePostsFilter filter, PaginationFilter paginationFilter = null)
        {
            var queryable = _issuePostRepository.FindAll();
            List<IssuePost> issuePosts;

            if (!string.IsNullOrEmpty(filter?.SearchText))
            {
                queryable = _issuePostRepository.FindAllWithTextSearch(filter.SearchText);
            }

            if (!string.IsNullOrEmpty(filter?.ProjectName) && !string.IsNullOrEmpty(filter?.OrganizationName))
            {
                queryable = queryable
                    .Where(x => x.ParentIssue.ParentOrganization.Name == filter.OrganizationName && x.ParentIssue.ParentProject.Name == filter.ProjectName);
            }

            if (!string.IsNullOrEmpty(filter?.AuthorUsername))
            {
                queryable = queryable
                    .Where(i => i.Author.UserName == filter.AuthorUsername);
            }

            if (filter.Open.HasValue)
            {
                queryable = queryable
                    .Where(i => i.ParentIssue.Open == filter.Open);
            }

            var count = await queryable.CountAsync().ConfigureAwait(false);
            queryable = queryable
                    .Include(x => x.Author)
                    .Include(x => x.Reactions);

            if (paginationFilter == null)
            {
                issuePosts = await queryable
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
            else
            {
                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                issuePosts = await queryable
                        .Skip(skip)
                        .Take(paginationFilter.PageSize)
                        .ToListAsync()
                        .ConfigureAwait(false);
            }
            return (list: issuePosts, count);
        }
    }
}
