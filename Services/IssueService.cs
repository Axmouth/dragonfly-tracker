using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using Microsoft.AspNetCore.Http;
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
                .SingleOrDefaultAsync(x => x.Id == issueId).ConfigureAwait(false);
        }

        public async Task<Issue> GetIssueByUserAsync(string username, string projectName, int issueNumber)
        {
            return await _dataContext.Issues
                .SingleOrDefaultAsync(x => x.ParentProject.Name == projectName && x.Author.UserName == username && x.Number == issueNumber).ConfigureAwait(false);
        }

        public async Task<Issue> GetIssueByOrgAsync(string organizationName, string projectName, int issueNumber)
        {
            return await _dataContext.Issues
                .SingleOrDefaultAsync(x => x.ParentProject.Name == projectName && x.ParentOrganization.Name == organizationName && x.Number == issueNumber).ConfigureAwait(false);
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

        public async Task<bool> CreateIssuePostReactionAsync(IssuePostReaction issuePostReaction)
        {
            // post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            // await AddNewTags(post).ConfigureAwait(false);
            await _dataContext.IssuePostReactions.AddAsync(issuePostReaction);

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

        public async Task<bool> CreateIssueByUserAsync(Issue issue, string postContent, List<IssueType> types, string username, string projectName)
        {
            // post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            // await AddNewTags(post).ConfigureAwait(false);
            issue.ProjectId = _dataContext.Projects.Where(p => p.Name == projectName && p.Creator.UserName == username).SingleOrDefault().Id;
            issue.Number = _dataContext.Issues.Where(i => i.ProjectId == issue.ProjectId).Count();
            await _dataContext.Issues.AddAsync(issue);

            var created = await CreateIssuePostAsync(new IssuePost { Id = new Guid(), AuthorId = issue.AuthorId, Content = postContent, Number = 0, IssueId = issue.Id, CreatedAt = DateTime.UtcNow }).ConfigureAwait(false);

            // var created = await _dataContext.SaveChangesAsync().ConfigureAwait(false);

            return created;
        }

        public async Task<bool> CreateIssueByOrgAsync(Issue issue, string postContent, List<IssueType> types, string organizationName, string projectName)
        {
            // post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            // await AddNewTags(post).ConfigureAwait(false);
            issue.ProjectId = _dataContext.Projects.Where(p => p.Name == projectName && p.ParentOrganization.Name == organizationName).SingleOrDefault().Id;
            issue.Number = _dataContext.Issues.Where(i => i.ProjectId == issue.ProjectId).Count();
            await _dataContext.Issues.AddAsync(issue);

            var created = await CreateIssuePostAsync(new IssuePost { Id = new Guid(), AuthorId = issue.AuthorId, Content = postContent, Number = 0, IssueId = issue.Id, CreatedAt = DateTime.UtcNow }).ConfigureAwait(false);

            // var created = await _dataContext.SaveChangesAsync().ConfigureAwait(false);

            return created;
        }

        public async Task<bool> CreateIssueAsync(Issue issue, string postContent, List<IssueType> types)
        {
            // post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            // await AddNewTags(post).ConfigureAwait(false);
            // issue.ProjectId = _dataContext.Projects.Where(p => p.Name == projectName && p.ParentOrganization.Name == organizationName).SingleOrDefault().Id;
            issue.Number = _dataContext.Issues.Where(i => i.ProjectId == issue.ProjectId).Count();
            await _dataContext.Issues.AddAsync(issue);

            var created = await CreateIssuePostAsync(new IssuePost { Id = new Guid(), AuthorId = issue.AuthorId, Content = postContent, Number = 0, IssueId = issue.Id, CreatedAt = DateTime.UtcNow }).ConfigureAwait(false);

            // var created = await _dataContext.SaveChangesAsync().ConfigureAwait(false);

            return created;
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
            var issuePostReaction = new IssuePostReaction { Id = issuePostReactionId };

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
                    .SingleOrDefaultAsync(x => x.Id == issueId)
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

        public async Task<bool> UserOwnsIssuePostAsync(Guid issuePostId, string userId)
        {
            var issue = await _dataContext.IssuePosts
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

        public async Task<List<Issue>> GetIssuesAsync(GetAllIssuesFilter filter, PaginationFilter paginationFilter = null)
        {
            var queryable = _dataContext.Issues.AsQueryable();

            if (filter == null)
            {
                return new List<Issue>(){ };
            }

            if (filter.ProjectId.HasValue)
            {
                queryable = queryable.Where(x => x.ProjectId == filter.ProjectId);
            }

            if (!string.IsNullOrEmpty(filter?.ProjectName))
            {
                queryable = queryable
                    .Where(x => x.ParentProject.Name == filter.ProjectName);
            }

            if (!string.IsNullOrEmpty(filter?.ProjectName))
            {
                queryable = queryable
                    .Where(x => x.ParentProject.Name == filter.ProjectName);
            }

            if (!string.IsNullOrEmpty(filter?.ProjectName) && !string.IsNullOrEmpty(filter?.OrganizationName))
            {
                queryable = queryable
                    .Where(x => x.ParentOrganization.Name == filter.OrganizationName && x.ParentProject.Name == filter.ProjectName);
            }

            if (!string.IsNullOrEmpty(filter?.SearchText))
            {
                queryable = queryable
                    .Where(i => EF.Functions.ToTsVector("english", i.Title).Matches(filter.SearchText));
                    // .Where(i => EF.Functions.FuzzyStringMatchLevenshteinLessEqual(i.Title, filter.SearchText, 5) <= 5);
            }

            if (!string.IsNullOrEmpty(filter?.AuthorUsername))
            {
                queryable = queryable
                    .Where(i => i.Author.UserName == filter.AuthorUsername);
            }

            if (filter.Open.HasValue)
            {
                queryable = queryable
                    .Where(i => i.Open == filter.Open);
            }

            if (paginationFilter == null)
            {
                return await queryable
                    .Include(x => x.Author)
                    .Include(x => x.ParentProject)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await queryable
                    .Include(x => x.Author)
                    .Include(x => x.ParentProject)
                    .Skip(skip)
                    .Take(paginationFilter.PageSize)
                    .ToListAsync()
                    .ConfigureAwait(false);
        }

        public async Task<List<Issue>> GetIssuesByOrganizationAndProjectNameAsync(string organizationName, string projectName, PaginationFilter paginationFilter = null)
        {
            var queryable = _dataContext.Issues.AsQueryable();

            if (paginationFilter == null)
            {
                return await queryable
                    .Include(x => x.Author)
                    .Where(x => x.ParentOrganization.Name == organizationName && x.ParentProject.Name == projectName)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }

            // queryable = AddFiltersOnQuery(filter, queryable);

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await queryable
                    .Include(x => x.Author)
                    .Where(x => x.ParentOrganization.Name == organizationName && x.ParentProject.Name == projectName)
                    .Skip(skip)
                    .Take(paginationFilter.PageSize)
                    .ToListAsync()
                    .ConfigureAwait(false);
        }

        public async Task<List<Issue>> GetIssuesByAuthorIdAsync(string authortId, PaginationFilter paginationFilter = null)
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

        public async Task<List<IssuePost>> GetAllIssuePostsAsync(GetAllIssuePostsFilter filter, PaginationFilter paginationFilter = null)
        {
            var queryable = _dataContext.IssuePosts.AsQueryable();

            if (!string.IsNullOrEmpty(filter?.ProjectName) && !string.IsNullOrEmpty(filter?.OrganizationName))
            {
                queryable = queryable
                    .Where(x => x.ParentIssue.ParentOrganization.Name == filter.OrganizationName && x.ParentIssue.ParentProject.Name == filter.ProjectName);
            }

            if (!string.IsNullOrEmpty(filter?.SearchText))
            {
                queryable = queryable
                    .Where(ip => EF.Functions.ToTsVector("english", ip.Content).Matches(filter.SearchText));
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

        public async Task<List<Project>> GetProjectsByOrganizationNameAsync(string organizationName, PaginationFilter paginationFilter = null)
        {
            var queryable = _dataContext.Projects.AsQueryable();

            if (paginationFilter == null)
            {
                return await queryable
                    .Where(x => x.ParentOrganization.Name == organizationName)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }

            // queryable = AddFiltersOnQuery(filter, queryable);

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await queryable
                    .Where(x => x.ParentOrganization.Name == organizationName)
                    .Skip(skip)
                    .Take(paginationFilter.PageSize)
                    .ToListAsync()
                    .ConfigureAwait(false);
        }

        public async Task<List<Project>> GetProjectsByOrganizationIdAsync(Guid organizationId, PaginationFilter paginationFilter = null)
        {
            var queryable = _dataContext.Projects.AsQueryable();

            if (paginationFilter == null)
            {
                return await queryable
                    .Where(x => x.OrganizationId == organizationId)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }

            // queryable = AddFiltersOnQuery(filter, queryable);

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await queryable
                    .Where(x => x.OrganizationId == organizationId)
                    .Skip(skip)
                    .Take(paginationFilter.PageSize)
                    .ToListAsync()
                    .ConfigureAwait(false);
        }

        public async Task<List<Issue>> GetIssuesByProjectIdByTextSearchAsync(string authortId, PaginationFilter paginationFilter = null)
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

        private async Task AddIssueTypes(Issue issue)
        {/*
            foreach (var type in issue.Types)
            {
                var existingIssueType =
                    await _dataContext.IssueTypes.SingleOrDefaultAsync(x =>
                        x.Name == type.Name && x.ProjectId == type.ProjectId).ConfigureAwait(false);
                if (existingIssueType != null)
                    continue;

                await _dataContext.IssueTypes.AddAsync(new IssueType
                { Name = type.Name, ProjectId = issue.ProjectId });
            }*/
        }

        private async Task AddIssueStages(Project project)
        {
            foreach (var stage in project.Stages)
            {
                var existingTag =
                    await _dataContext.IssueTypes.SingleOrDefaultAsync(x =>
                        x.Name == stage.Name && x.ProjectId == stage.ProjectId).ConfigureAwait(false);
                if (existingTag != null)
                    continue;

                await _dataContext.IssueStages.AddAsync(new IssueStage
                { Name = stage.Name, ProjectId = project.Id });
            }
        }

    }
}
