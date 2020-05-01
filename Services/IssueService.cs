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
    public class IssueService: IIssueService
    {
        private readonly PgMainDataContext _pgMainDataContext;
        private readonly IIssuePostService _issuePostService;

        public IssueService(PgMainDataContext pgMainDataContext, IIssuePostService issuePostService)
        {
            _pgMainDataContext = pgMainDataContext;
            _issuePostService = issuePostService;
        }

        public async Task<Issue> GetIssueByIdAsync(Guid issueId)
        {
            return await _pgMainDataContext.Issues
                .SingleOrDefaultAsync(x => x.Id == issueId).ConfigureAwait(false);
        }

        public async Task<Issue> GetIssueByUserAsync(string username, string projectName, int issueNumber)
        {
            return await _pgMainDataContext.Issues
                .SingleOrDefaultAsync(x => x.ParentProject.Name == projectName && x.Author.UserName == username && x.Number == issueNumber).ConfigureAwait(false);
        }

        public async Task<Issue> GetIssueByOrgAsync(string organizationName, string projectName, int issueNumber)
        {
            return await _pgMainDataContext.Issues
                .SingleOrDefaultAsync(x => x.ParentProject.Name == projectName && x.ParentOrganization.Name == organizationName && x.Number == issueNumber).ConfigureAwait(false);
        }

        public async Task<bool> CreateIssueUpdateAsync(IssueUpdate issueUpdate)
        {
            // post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            // await AddNewTags(post).ConfigureAwait(false);
            await _pgMainDataContext.IssueUpdates.AddAsync(issueUpdate);

            var created = await _pgMainDataContext.SaveChangesAsync().ConfigureAwait(false);
            return created > 0;
        }

        public async Task<bool> CreateIssueStageAsync(IssueStage issueStage)
        {
            // post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            // await AddNewTags(post).ConfigureAwait(false);
            await _pgMainDataContext.IssueStages.AddAsync(issueStage);

            var created = await _pgMainDataContext.SaveChangesAsync().ConfigureAwait(false);
            return created > 0;
        }

        public async Task<bool> CreateIssueTypeAsync(IssueType issueType)
        {
            // post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            // await AddNewTags(post).ConfigureAwait(false);
            await _pgMainDataContext.IssueTypes.AddAsync(issueType);

            var created = await _pgMainDataContext.SaveChangesAsync().ConfigureAwait(false);
            return created > 0;
        }

        public async Task<bool> CreateIssueByUserAsync(Issue issue, string postContent, List<IssueType> types, string username, string projectName)
        {
            // post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            // await AddNewTags(post).ConfigureAwait(false);
            issue.Open = true;
            issue.ProjectId = _pgMainDataContext.Projects.Where(p => p.Name == projectName && p.Creator.UserName == username).SingleOrDefault().Id;
            issue.Number = _pgMainDataContext.Issues.Where(i => i.ProjectId == issue.ProjectId).Count();
            await _pgMainDataContext.Issues.AddAsync(issue);

            var created = await _issuePostService.CreateIssuePostAsync(new IssuePost { Id = new Guid(), AuthorId = issue.AuthorId, Content = postContent, Number = 0, IssueId = issue.Id, CreatedAt = DateTime.UtcNow }).ConfigureAwait(false);

            // var created = await _dataContext.SaveChangesAsync().ConfigureAwait(false);

            return created;
        }

        public async Task<bool> CreateIssueByOrgAsync(Issue issue, string postContent, List<IssueType> types, string organizationName, string projectName)
        {
            // post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            // await AddNewTags(post).ConfigureAwait(false);
            issue.Open = true;
            issue.ProjectId = _pgMainDataContext.Projects.Where(p => p.Name == projectName && p.ParentOrganization.Name == organizationName).SingleOrDefault().Id;
            issue.Number = _pgMainDataContext.Issues.Where(i => i.ProjectId == issue.ProjectId).Count();
            await _pgMainDataContext.Issues.AddAsync(issue);

            var created = await _issuePostService.CreateIssuePostAsync(new IssuePost { Id = new Guid(), AuthorId = issue.AuthorId, Content = postContent, Number = 0, IssueId = issue.Id, CreatedAt = DateTime.UtcNow }).ConfigureAwait(false);

            // var created = await _dataContext.SaveChangesAsync().ConfigureAwait(false);

            return created;
        }

        public async Task<bool> CreateIssueAsync(Issue issue, string postContent, List<IssueType> types)
        {
            // post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            // await AddNewTags(post).ConfigureAwait(false);
            // issue.ProjectId = _dataContext.Projects.Where(p => p.Name == projectName && p.ParentOrganization.Name == organizationName).SingleOrDefault().Id;
            issue.Open = true;
            issue.Number = _pgMainDataContext.Issues.Where(i => i.ProjectId == issue.ProjectId).Count();
            await _pgMainDataContext.Issues.AddAsync(issue);

            var created = await _issuePostService.CreateIssuePostAsync(new IssuePost { Id = new Guid(), AuthorId = issue.AuthorId, Content = postContent, Number = 0, IssueId = issue.Id, CreatedAt = DateTime.UtcNow }).ConfigureAwait(false);

            // var created = await _dataContext.SaveChangesAsync().ConfigureAwait(false);

            return created;
        }

        public async Task<bool> UpdateIssueAsync(Issue issueToUpdate)
        {
            // postToUpdate.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());
            // await AddNewTags(postToUpdate).ConfigureAwait(false);
            _pgMainDataContext.Issues.Update(issueToUpdate);
            var updated = await _pgMainDataContext.SaveChangesAsync().ConfigureAwait(false);
            return updated > 0;
        }

        public async Task<bool> UpdateIssueStageAsync(IssueStage issueStageToUpdate)
        {
            // postToUpdate.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());
            // await AddNewTags(postToUpdate).ConfigureAwait(false);
            _pgMainDataContext.IssueStages.Update(issueStageToUpdate);
            var updated = await _pgMainDataContext.SaveChangesAsync().ConfigureAwait(false);
            return updated > 0;
        }

        public async Task<bool> UpdateIssueTypeAsync(IssueType issueTypeToUpdate)
        {
            // postToUpdate.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());
            // await AddNewTags(postToUpdate).ConfigureAwait(false);
            _pgMainDataContext.IssueTypes.Update(issueTypeToUpdate);
            var updated = await _pgMainDataContext.SaveChangesAsync().ConfigureAwait(false);
            return updated > 0;
        }

        public async Task<bool> DeleteIssueAsync(Guid issueId)
        {
            var issue = new Issue { Id = issueId };

            _pgMainDataContext.Issues.Attach(issue);


            _pgMainDataContext.Issues.Remove(issue);
            var deleted = await _pgMainDataContext.SaveChangesAsync().ConfigureAwait(false);
            return deleted > 0;
        }

        public async Task<bool> DeleteIssueStageAsync(Guid issueStageId)
        {
            var issueStage = new IssueStage { Id = issueStageId };

            _pgMainDataContext.IssueStages.Attach(issueStage);


            _pgMainDataContext.IssueStages.Remove(issueStage);
            var deleted = await _pgMainDataContext.SaveChangesAsync().ConfigureAwait(false);
            return deleted > 0;
        }

        public async Task<bool> DeleteIssueTypeAsync(Guid issueTypeId)
        {
            var issueType = new IssueType { Id = issueTypeId };

            _pgMainDataContext.IssueTypes.Attach(issueType);


            _pgMainDataContext.IssueTypes.Remove(issueType);
            var deleted = await _pgMainDataContext.SaveChangesAsync().ConfigureAwait(false);
            return deleted > 0;
        }

        public async Task<bool> UserOwnsIssueAsync(Guid issueId, string userId)
        {
            var issue = await _pgMainDataContext.Issues
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

        public async Task<Tuple<List<Issue>, int>> GetIssuesByProjectIdAsync(Guid projectId, PaginationFilter paginationFilter = null)
        {
            var queryable = _pgMainDataContext.Issues.AsQueryable()
                    .Where(x => x.ProjectId == projectId)
                    .Include(x => x.Author);
            List<Issue> issues;
            var count = await queryable.CountAsync().ConfigureAwait(false);

            if (paginationFilter == null)
            {
                issues = await queryable
                    .ToListAsync()
                    .ConfigureAwait(false);
            } else
            {
                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                issues = await queryable
                    .Skip(skip)
                    .Take(paginationFilter.PageSize)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
            return Tuple.Create(issues, count);

        }

        public async Task<Tuple<List<Issue>, int>> GetIssuesAsync(GetAllIssuesFilter filter, PaginationFilter paginationFilter = null)
        {
            var queryable = _pgMainDataContext.Issues.AsQueryable();
            List<Issue> issues;

            if (filter == null)
            {
                return Tuple.Create(new List<Issue>(){ }, 0);
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
                    // .Where(i => EF.Functions.ToTsVector("english", i.Title).Matches(filter.SearchText));
                    // .Where(i => EF.Functions.FuzzyStringMatchLevenshteinLessEqual(i.Title, filter.SearchText, 5) <= 5);
                    .Where(i => EF.Functions.TrigramsWordSimilarity (i.Title, filter.SearchText) > 0.0)
                    .OrderByDescending(i => EF.Functions.TrigramsWordSimilarity(i.Title, filter.SearchText));
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

            var count = await queryable.CountAsync().ConfigureAwait(false);

            if (paginationFilter == null)
            {
                issues = await queryable
                    .Include(x => x.Author)
                    .Include(x => x.ParentProject)
                    .ToListAsync()
                    .ConfigureAwait(false);
            } else
            {
                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                issues = await queryable
                        .Include(x => x.Author)
                        .Include(x => x.ParentProject)
                        .Skip(skip)
                        .Take(paginationFilter.PageSize)
                        .ToListAsync()
                        .ConfigureAwait(false);
            }
            return Tuple.Create(issues, count);
        }

        public async Task<Tuple<List<Issue>, int>> GetIssuesByOrganizationAndProjectNameAsync(string organizationName, string projectName, PaginationFilter paginationFilter = null)
        {
            var queryable = _pgMainDataContext.Issues.AsQueryable()
                    .Include(x => x.Author)
                    .Where(x => x.ParentOrganization.Name == organizationName && x.ParentProject.Name == projectName);
            List<Issue> issues;
            var count = await queryable.CountAsync().ConfigureAwait(false);

            if (paginationFilter == null)
            {
                issues = await queryable
                    .ToListAsync()
                    .ConfigureAwait(false);
            } else
            {
                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                issues = await queryable
                        .Skip(skip)
                        .Take(paginationFilter.PageSize)
                        .ToListAsync()
                        .ConfigureAwait(false);
            }
            return Tuple.Create(issues, count);

        }

        public async Task<Tuple<List<Issue>, int>> GetIssuesByAuthorIdAsync(string authortId, PaginationFilter paginationFilter = null)
        {
            var queryable = _pgMainDataContext.Issues.AsQueryable()
                    .Include(x => x.Author)
                    .Where(x => x.AuthorId == authortId);
            List<Issue> issues;
            var count = await queryable.CountAsync().ConfigureAwait(false);

            if (paginationFilter == null)
            {
                issues = await queryable
                    .ToListAsync()
                    .ConfigureAwait(false);
            } else
            {
                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                issues = await queryable
                        .Skip(skip)
                        .Take(paginationFilter.PageSize)
                        .ToListAsync()
                        .ConfigureAwait(false);
            }
            return Tuple.Create(issues, count);
        }

        public async Task<Tuple<List<Issue>, int>> GetIssuesByAuthorUsernameAsync(string authortName, PaginationFilter paginationFilter = null)
        {
            var queryable = _pgMainDataContext.Issues.AsQueryable()
                    .Include(x => x.Author)
                    .Where(x => x.Author.UserName == authortName);
            List<Issue> issues;
            var count = await queryable.CountAsync().ConfigureAwait(false);

            if (paginationFilter == null)
            {
                issues =  await queryable
                    .ToListAsync()
                    .ConfigureAwait(false);
            } else
            {
                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                issues = await queryable
                        .Skip(skip)
                        .Take(paginationFilter.PageSize)
                        .ToListAsync()
                        .ConfigureAwait(false);
            }
            return Tuple.Create(issues, count);

        }

        public async Task<Tuple<List<IssueUpdate>, int>> GetIssueUpdatesInTimePeriodAsync(Guid issueId, DateTime start, DateTime end, PaginationFilter paginationFilter = null)
        {
            var queryable = _pgMainDataContext.IssueUpdates.AsQueryable()
                    .Where(x => x.IssueId == issueId && end < x.CreatedAt && x.CreatedAt <= start);
            List<IssueUpdate> issueUpdates;
            var count = await queryable.CountAsync().ConfigureAwait(false);

            if (paginationFilter == null)
            {
                issueUpdates = await queryable
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
            else
            {
                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                issueUpdates = await queryable
                    .Skip(skip)
                    .Take(paginationFilter.PageSize)
                    .ToListAsync()
                    .ConfigureAwait(false);

            }
            return Tuple.Create(issueUpdates, count);

        }

        public async Task<Tuple<List<Issue>, int>> GetIssuesByProjectIdByTextSearchAsync(string authortId, PaginationFilter paginationFilter = null)
        {
            var queryable = _pgMainDataContext.Issues.AsQueryable()
                    // .Include(x => x.Tags)
                    .Where(x => x.AuthorId == authortId);
            List<Issue> issues;
            var count = await queryable.CountAsync().ConfigureAwait(false);

            throw new NotImplementedException("Derp");

            if (paginationFilter == null)
            {
                issues = await queryable
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
            else
            {
                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                issues = await queryable
                        .Skip(skip)
                        .Take(paginationFilter.PageSize)
                        .ToListAsync()
                        .ConfigureAwait(false);
            }

            return Tuple.Create(issues, count);
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
                    await _pgMainDataContext.IssueTypes.SingleOrDefaultAsync(x =>
                        x.Name == stage.Name && x.ProjectId == stage.ProjectId).ConfigureAwait(false);
                if (existingTag != null)
                    continue;

                await _pgMainDataContext.IssueStages.AddAsync(new IssueStage
                { Name = stage.Name, ProjectId = project.Id });
            }
        }

    }
}
