using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Services
{
    public class IssueService
    {
        private readonly DataContext _dataContext;

        public IssueService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Issue> GetIssueByIdAsync(Guid issueId)
        {
            return await _dataContext.Issues
                // .Include(x => x.Tags)
                .SingleOrDefaultAsync(x => x.Id == issueId).ConfigureAwait(false);
        }

        public async Task<bool> CreateIssuePostAsync(IssuePost issuePost)
        {
            // post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            // await AddNewTags(post).ConfigureAwait(false);
            await _dataContext.IssuePosts.AddAsync(issuePost);

            var created = await _dataContext.SaveChangesAsync().ConfigureAwait(false);
            return created > 0;
        }

        public async Task<bool> CreateIssueUpdateAsync(IssueUpdate issueUpdate)
        {
            // post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            // await AddNewTags(post).ConfigureAwait(false);
            await _dataContext.IssueUpdates.AddAsync(issueUpdate);

            var created = await _dataContext.SaveChangesAsync().ConfigureAwait(false);
            return created > 0;
        }

        public async Task<bool> CreateIssueStageAsync(IssueStage issueStage)
        {
            // post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            // await AddNewTags(post).ConfigureAwait(false);
            await _dataContext.IssueStages.AddAsync(issueStage);

            var created = await _dataContext.SaveChangesAsync().ConfigureAwait(false);
            return created > 0;
        }

        public async Task<bool> CreateIssueTypeAsync(IssueType issueType)
        {
            // post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            // await AddNewTags(post).ConfigureAwait(false);
            await _dataContext.IssueTypes.AddAsync(issueType);

            var created = await _dataContext.SaveChangesAsync().ConfigureAwait(false);
            return created > 0;
        }

        public async Task<bool> CreateIssueAsync(Issue issue)
        {
            // post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            // await AddNewTags(post).ConfigureAwait(false);
            await _dataContext.Issues.AddAsync(issue);

            var created = await _dataContext.SaveChangesAsync().ConfigureAwait(false);
            return created > 0;
        }

        public async Task<bool> UpdateIssueAsync(Issue issueToUpdate)
        {
            // postToUpdate.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());
            // await AddNewTags(postToUpdate).ConfigureAwait(false);
            _dataContext.Issues.Update(issueToUpdate);
            var updated = await _dataContext.SaveChangesAsync().ConfigureAwait(false);
            return updated > 0;
        }

        public async Task<bool> UpdateIssuePostAsync(IssuePost issuePostToUpdate)
        {
            // postToUpdate.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());
            // await AddNewTags(postToUpdate).ConfigureAwait(false);
            _dataContext.IssuePosts.Update(issuePostToUpdate);
            var updated = await _dataContext.SaveChangesAsync().ConfigureAwait(false);
            return updated > 0;
        }

        public async Task<bool> UpdateIssueStageAsync(IssueStage issueStageToUpdate)
        {
            // postToUpdate.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());
            // await AddNewTags(postToUpdate).ConfigureAwait(false);
            _dataContext.IssueStages.Update(issueStageToUpdate);
            var updated = await _dataContext.SaveChangesAsync().ConfigureAwait(false);
            return updated > 0;
        }

        public async Task<bool> UpdateIssueTypeAsync(IssueType issueTypeToUpdate)
        {
            // postToUpdate.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());
            // await AddNewTags(postToUpdate).ConfigureAwait(false);
            _dataContext.IssueTypes.Update(issueTypeToUpdate);
            var updated = await _dataContext.SaveChangesAsync().ConfigureAwait(false);
            return updated > 0;
        }

        public async Task<bool> DeleteIssueAsync(Guid issueId)
        {
            var issue = new Issue { Id = issueId };

            _dataContext.Issues.Attach(issue);


            _dataContext.Issues.Remove(issue);
            var deleted = await _dataContext.SaveChangesAsync().ConfigureAwait(false);
            return deleted > 0;
        }

        public async Task<bool> DeleteIssuePostAsync(Guid issuePostId)
        {
            var issuePost = new IssuePost { Id = issuePostId };

            _dataContext.IssuePosts.Attach(issuePost);


            _dataContext.IssuePosts.Remove(issuePost);
            var deleted = await _dataContext.SaveChangesAsync().ConfigureAwait(false);
            return deleted > 0;
        }

        public async Task<bool> DeleteIssuePostReactionAsync(Guid issuePostReactionId)
        {
            var issuePostReaction = new IssuePostReactions { Id = issuePostReactionId };

            _dataContext.IssuePostReactions.Attach(issuePostReaction);


            _dataContext.IssuePostReactions.Remove(issuePostReaction);
            var deleted = await _dataContext.SaveChangesAsync().ConfigureAwait(false);
            return deleted > 0;
        }

        public async Task<bool> DeleteIssueStageAsync(Guid issueStageId)
        {
            var issueStage = new IssueStage { Id = issueStageId };

            _dataContext.IssueStages.Attach(issueStage);


            _dataContext.IssueStages.Remove(issueStage);
            var deleted = await _dataContext.SaveChangesAsync().ConfigureAwait(false);
            return deleted > 0;
        }

        public async Task<bool> DeleteIssueTypeAsync(Guid issueTypeId)
        {
            var issueType = new IssueType { Id = issueTypeId };

            _dataContext.IssueTypes.Attach(issueType);


            _dataContext.IssueTypes.Remove(issueType);
            var deleted = await _dataContext.SaveChangesAsync().ConfigureAwait(false);
            return deleted > 0;
        }

        public async Task<bool> UserOwnsIssueAsync(Guid issueId, string userId)
        {
            var issue = await _dataContext.Issues
                    .AsNoTracking()
                    .Include(x => x.Author)
                    .SingleOrDefaultAsync(x => x.Id == issueId)
                    .ConfigureAwait(false);

            if (issue == null)
            {
                return false;
            }

            if (issue.Author.Id != userId)
            {
                return false;
            }

            return true;
        }

        public async Task<List<Issue>> GetIssuesByProjectIdAsync(Guid projectId, PaginationFilter paginationFilter = null)
        {
            var queryable = _dataContext.Issues.AsQueryable();

            if (paginationFilter == null)
            {
                return await queryable
                    .Include(x => x.Author)
                    .Where(x => x.ProjectId == projectId)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }

            // queryable = AddFiltersOnQuery(filter, queryable);

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await queryable
                    .Include(x => x.Author)
                    .Where(x => x.ProjectId == projectId)
                    .Skip(skip)
                    .Take(paginationFilter.PageSize)
                    .ToListAsync()
                    .ConfigureAwait(false);
        }

        public async Task<List<Issue>> GetIssuesByCompanyAndProjectNameAsync(string companyName, string projectName, PaginationFilter paginationFilter = null)
        {
            var queryable = _dataContext.Issues.AsQueryable();

            if (paginationFilter == null)
            {
                return await queryable
                    .Include(x => x.Author)
                    .Where(x => x.ParentCompany.Name == companyName && x.ParentProject.Name == projectName)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }

            // queryable = AddFiltersOnQuery(filter, queryable);

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await queryable
                    .Include(x => x.Author)
                    .Where(x => x.ParentCompany.Name == companyName && x.ParentProject.Name == projectName)
                    .Skip(skip)
                    .Take(paginationFilter.PageSize)
                    .ToListAsync()
                    .ConfigureAwait(false);
        }

        public async Task<List<Issue>> GetIssuesByAuthorIdAsync(Guid authortId, PaginationFilter paginationFilter = null)
        {
            var queryable = _dataContext.Issues.AsQueryable();

            if (paginationFilter == null)
            {
                return await queryable
                    .Include(x => x.Author)
                    .Where(x => x.AuthorId == authortId)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }

            // queryable = AddFiltersOnQuery(filter, queryable);

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await queryable
                    .Include(x => x.Author)
                    .Where(x => x.AuthorId == authortId)
                    .Skip(skip)
                    .Take(paginationFilter.PageSize)
                    .ToListAsync()
                    .ConfigureAwait(false);
        }

        public async Task<List<Issue>> GetIssuesByAuthorUsernameAsync(string authortName, PaginationFilter paginationFilter = null)
        {
            var queryable = _dataContext.Issues.AsQueryable();

            if (paginationFilter == null)
            {
                return await queryable
                    .Include(x => x.Author)
                    .Where(x => x.Author.UserName == authortName)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }

            // queryable = AddFiltersOnQuery(filter, queryable);

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await queryable
                    .Include(x => x.Author)
                    .Where(x => x.Author.UserName == authortName)
                    .Skip(skip)
                    .Take(paginationFilter.PageSize)
                    .ToListAsync()
                    .ConfigureAwait(false);
        }

        public async Task<List<IssuePost>> GetIssuePostsByIssueAsync(Guid issueId, PaginationFilter paginationFilter = null)
        {
            var queryable = _dataContext.IssuePosts.AsQueryable();

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

        public async Task<List<IssueUpdate>> GetIssueUpdatesInTimePeriodAsync(Guid issueId, DateTime start, DateTime end, PaginationFilter paginationFilter = null)
        {
            var queryable = _dataContext.IssueUpdates.AsQueryable();

            if (paginationFilter == null)
            {
                return await queryable
                    .Where(x => x.IssueId == issueId &&  end < x.CreatedAt && x.CreatedAt <= start)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }

            // queryable = AddFiltersOnQuery(filter, queryable);

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await queryable
                    .Where(x => x.IssueId == issueId && end < x.CreatedAt && x.CreatedAt <= start)
                    .Skip(skip)
                    .Take(paginationFilter.PageSize)
                    .ToListAsync()
                    .ConfigureAwait(false);
        }

        public async Task<List<Project>> GetProjectsByCompanyNameAsync(string companyName, PaginationFilter paginationFilter = null)
        {
            var queryable = _dataContext.Projects.AsQueryable();

            if (paginationFilter == null)
            {
                return await queryable
                    .Where(x => x.ParentCompany.Name == companyName)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }

            // queryable = AddFiltersOnQuery(filter, queryable);

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await queryable
                    .Where(x => x.ParentCompany.Name == companyName)
                    .Skip(skip)
                    .Take(paginationFilter.PageSize)
                    .ToListAsync()
                    .ConfigureAwait(false);
        }

        public async Task<List<Project>> GetProjectsByCompanyIdAsync(Guid companyId, PaginationFilter paginationFilter = null)
        {
            var queryable = _dataContext.Projects.AsQueryable();

            if (paginationFilter == null)
            {
                return await queryable
                    .Where(x => x.CompanyId == companyId)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }

            // queryable = AddFiltersOnQuery(filter, queryable);

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await queryable
                    .Where(x => x.CompanyId == companyId)
                    .Skip(skip)
                    .Take(paginationFilter.PageSize)
                    .ToListAsync()
                    .ConfigureAwait(false);
        }

        public async Task<List<Issue>> GetIssuesByProjectIdByTextSearchAsync(Guid authortId, PaginationFilter paginationFilter = null)
        {
            var queryable = _dataContext.Issues.AsQueryable();

            throw new NotImplementedException("Derp");

            if (paginationFilter == null)
            {
                return await queryable
                    // .Include(x => x.Tags)
                    .Where(x => x.AuthorId == authortId)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }

            // queryable = AddFiltersOnQuery(filter, queryable);

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await queryable
                    // .Include(x => x.Tags)
                    .Where(x => x.AuthorId == authortId)
                    .Skip(skip)
                    .Take(paginationFilter.PageSize)
                    .ToListAsync()
                    .ConfigureAwait(false);
        }

    }
}
