using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Services
{
    public class IssuePostService: IIssuePostService
    {
        private readonly PgMainDataContext _pgMainDataContext;

        public IssuePostService(PgMainDataContext pgMainDataContext)
        {
            _pgMainDataContext = pgMainDataContext;
        }

        public async Task<bool> CreateIssuePostAsync(IssuePost issuePost)
        {
            // post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            // await AddNewTags(post).ConfigureAwait(false);
            await _pgMainDataContext.IssuePosts.AddAsync(issuePost);

            var created = await _pgMainDataContext.SaveChangesAsync().ConfigureAwait(false);
            return created > 0;
        }

        public async Task<bool> CreateIssuePostReactionAsync(IssuePostReaction issuePostReaction)
        {
            // post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            // await AddNewTags(post).ConfigureAwait(false);
            await _pgMainDataContext.IssuePostReactions.AddAsync(issuePostReaction);

            var created = await _pgMainDataContext.SaveChangesAsync().ConfigureAwait(false);
            return created > 0;
        }

        public async Task<bool> UpdateIssuePostAsync(IssuePost issuePostToUpdate)
        {
            // postToUpdate.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());
            // await AddNewTags(postToUpdate).ConfigureAwait(false);
            _pgMainDataContext.IssuePosts.Update(issuePostToUpdate);
            var updated = await _pgMainDataContext.SaveChangesAsync().ConfigureAwait(false);
            return updated > 0;
        }

        public async Task<bool> DeleteIssuePostAsync(Guid issuePostId)
        {
            var issuePost = new IssuePost { Id = issuePostId };

            _pgMainDataContext.IssuePosts.Attach(issuePost);


            _pgMainDataContext.IssuePosts.Remove(issuePost);
            var deleted = await _pgMainDataContext.SaveChangesAsync().ConfigureAwait(false);
            return deleted > 0;
        }

        public async Task<bool> DeleteIssuePostReactionAsync(Guid issuePostReactionId)
        {
            var issuePostReaction = new IssuePostReaction { Id = issuePostReactionId };

            _pgMainDataContext.IssuePostReactions.Attach(issuePostReaction);


            _pgMainDataContext.IssuePostReactions.Remove(issuePostReaction);
            var deleted = await _pgMainDataContext.SaveChangesAsync().ConfigureAwait(false);
            return deleted > 0;
        }

        public async Task<bool> UserOwnsIssuePostAsync(Guid issuePostId, string userId)
        {
            var issue = await _pgMainDataContext.IssuePosts
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == issuePostId)
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

        public async Task<List<IssuePost>> GetIssuePostsByIssueAsync(Guid issueId, PaginationFilter paginationFilter = null)
        {
            var queryable = _pgMainDataContext.IssuePosts.AsQueryable();

            if (paginationFilter == null)
            {
                return await queryable
                    .Include(x => x.Author)
                    .Include(x => x.Reactions)
                    .Where(x => x.IssueId == issueId)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }

            // queryable = AddFiltersOnQuery(filter, queryable);

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await queryable
                    .Include(x => x.Author)
                    .Include(x => x.Reactions)
                    .Where(x => x.IssueId == issueId)
                    .Skip(skip)
                    .Take(paginationFilter.PageSize)
                    .ToListAsync()
                    .ConfigureAwait(false);
        }

        public async Task<List<IssuePost>> GetAllIssuePostsAsync(GetAllIssuePostsFilter filter, PaginationFilter paginationFilter = null)
        {
            var queryable = _pgMainDataContext.IssuePosts.AsQueryable();

            if (!string.IsNullOrEmpty(filter?.ProjectName) && !string.IsNullOrEmpty(filter?.OrganizationName))
            {
                queryable = queryable
                    .Where(x => x.ParentIssue.ParentOrganization.Name == filter.OrganizationName && x.ParentIssue.ParentProject.Name == filter.ProjectName);
            }

            if (!string.IsNullOrEmpty(filter?.SearchText))
            {
                queryable = queryable
                    // .Where(ip => EF.Functions.ToTsVector("english", ip.Content).Matches(filter.SearchText));
                    .Where(ip => EF.Functions.TrigramsWordSimilarity(ip.Content, filter.SearchText) > 0.3);
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

            if (paginationFilter == null)
            {
                return await queryable
                    .Include(x => x.Author)
                    .Include(x => x.Reactions)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await queryable
                    .Include(x => x.Author)
                    .Include(x => x.Reactions)
                    .Skip(skip)
                    .Take(paginationFilter.PageSize)
                    .ToListAsync()
                    .ConfigureAwait(false);
        }
    }
}
