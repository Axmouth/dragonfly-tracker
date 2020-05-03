﻿using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using DragonflyTracker.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Services
{
    public class IssueService : IIssueService
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IIssueStageRepository _issueStageRepository;
        private readonly IIssueTypeRepository _issueTypeRepository;
        private readonly IIssueUpdateRepository _issueUpdateRepository;
        private readonly IIssuePostService _issuePostService;
        private readonly IProjectRepository _projectRepository;

        public IssueService(IProjectRepository projectRepository, IIssuePostService issuePostService, IIssueRepository issueRepository,
            IIssueUpdateRepository issueUpdateRepository, IIssueStageRepository issueStageRepository, IIssueTypeRepository issueTypeRepository)
        {
            _issueRepository = issueRepository;
            _issuePostService = issuePostService;
            _issueStageRepository = issueStageRepository;
            _issueTypeRepository = issueTypeRepository;
            _issueUpdateRepository = issueUpdateRepository;
            _projectRepository = projectRepository;
        }

        public async Task<Issue> GetIssueByIdAsync(Guid issueId)
        {
            return await _issueRepository
                .FindByCondition(x => x.Id == issueId)
                .SingleOrDefaultAsync()
                .ConfigureAwait(false);
        }

        public async Task<Issue> GetIssueByUserAsync(string username, string projectName, int issueNumber)
        {
            return await _issueRepository
                .FindByCondition(x => x.ParentProject.Name == projectName && x.Author.UserName == username && x.Number == issueNumber)
                .SingleOrDefaultAsync()
                .ConfigureAwait(false);
        }

        public async Task<Issue> GetIssueByOrgAsync(string organizationName, string projectName, int issueNumber)
        {
            return await _issueRepository
                .FindByCondition(x => x.ParentProject.Name == projectName && x.ParentOrganization.Name == organizationName && x.Number == issueNumber)
                .SingleOrDefaultAsync()
                .ConfigureAwait(false);
        }

        public async Task<bool> CreateIssueUpdateAsync(IssueUpdate issueUpdate)
        {
            // post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            // await AddNewTags(post).ConfigureAwait(false);
            await _issueUpdateRepository.CreateAsync(issueUpdate).ConfigureAwait(false);

            var created = await _issueUpdateRepository.SaveAsync().ConfigureAwait(false);
            return created > 0;
        }

        public async Task<bool> CreateIssueStageAsync(IssueStage issueStage)
        {
            // post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            // await AddNewTags(post).ConfigureAwait(false);
            await _issueStageRepository.CreateAsync(issueStage).ConfigureAwait(false);

            var created = await _issueStageRepository.SaveAsync().ConfigureAwait(false);
            return created > 0;
        }

        public async Task<bool> CreateIssueTypeAsync(IssueType issueType)
        {
            // post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            // await AddNewTags(post).ConfigureAwait(false);
            await _issueTypeRepository.CreateAsync(issueType).ConfigureAwait(false);

            var created = await _issueTypeRepository.SaveAsync().ConfigureAwait(false);
            return created > 0;
        }

        public async Task<bool> CreateIssueByUserAsync(Issue issue, List<IssueType> types, string username, string projectName)
        {
            // post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            // await AddNewTags(post).ConfigureAwait(false);
            if (issue == null)
            {
                return false;
            }
            issue.Open = true;
            issue.ProjectId = _projectRepository.FindByCondition(p => p.Name == projectName && p.Creator.UserName == username).SingleOrDefault().Id;
            issue.Number = _issueRepository.FindByCondition(i => i.ProjectId == issue.ProjectId).Count();
            await _issueRepository.CreateAsync(issue).ConfigureAwait(false);

            // var created = await _issuePostService.CreateIssuePostAsync(new IssuePost { Id = new Guid(), AuthorId = issue.AuthorId, Content = issue.Content, Number = 0, IssueId = issue.Id, CreatedAt = DateTime.UtcNow }).ConfigureAwait(false);

            var created = await _issueRepository.SaveAsync().ConfigureAwait(false);

            return created > 0;
        }

        public async Task<bool> CreateIssueByOrgAsync(Issue issue, List<IssueType> types, string organizationName, string projectName)
        {
            // post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            // await AddNewTags(post).ConfigureAwait(false);
            if (issue == null)
            {
                return false;
            }
            issue.Open = true;
            issue.ProjectId = _projectRepository.FindByCondition(p => p.Name == projectName && p.ParentOrganization.Name == organizationName).SingleOrDefault().Id;
            issue.Number = _issueRepository.FindByCondition(i => i.ProjectId == issue.ProjectId).Count();
            await _issueRepository.CreateAsync(issue).ConfigureAwait(false);

            // var created = await _issuePostService.CreateIssuePostAsync(new IssuePost { Id = new Guid(), AuthorId = issue.AuthorId, Content = postContent, Number = 0, IssueId = issue.Id, CreatedAt = DateTime.UtcNow }).ConfigureAwait(false);

            var created = await _issueRepository.SaveAsync().ConfigureAwait(false);

            return created > 0;
        }

        public async Task<bool> CreateIssueAsync(Issue issue, List<IssueType> types)
        {
            // post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            // await AddNewTags(post).ConfigureAwait(false);
            // issue.ProjectId = _dataContext.Projects.Where(p => p.Name == projectName && p.ParentOrganization.Name == organizationName).SingleOrDefault().Id;
            if (issue == null)
            {
                return false;
            }
            issue.Open = true;
            issue.Number = _issueRepository.FindByCondition(i => i.ProjectId == issue.ProjectId).Count();
            await _issueRepository.CreateAsync(issue).ConfigureAwait(false);

            // var created = await _issuePostService.CreateIssuePostAsync(new IssuePost { Id = new Guid(), AuthorId = issue.AuthorId, Content = postContent, Number = 0, IssueId = issue.Id, CreatedAt = DateTime.UtcNow }).ConfigureAwait(false);

            var created = await _issueRepository.SaveAsync().ConfigureAwait(false);

            return created > 0;
        }

        public async Task<bool> UpdateIssueAsync(Issue issueToUpdate)
        {
            // postToUpdate.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());
            // await AddNewTags(postToUpdate).ConfigureAwait(false);
            _issueRepository.Update(issueToUpdate);
            var updated = await _issueRepository.SaveAsync().ConfigureAwait(false);
            return updated > 0;
        }

        public async Task<bool> UpdateIssueStageAsync(IssueStage issueStageToUpdate)
        {
            // postToUpdate.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());
            // await AddNewTags(postToUpdate).ConfigureAwait(false);
            _issueStageRepository.Update(issueStageToUpdate);
            var updated = await _issueStageRepository.SaveAsync().ConfigureAwait(false);
            return updated > 0;
        }

        public async Task<bool> UpdateIssueTypeAsync(IssueType issueTypeToUpdate)
        {
            // postToUpdate.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());
            // await AddNewTags(postToUpdate).ConfigureAwait(false);
            _issueTypeRepository.Update(issueTypeToUpdate);
            var updated = await _issueTypeRepository.SaveAsync().ConfigureAwait(false);
            return updated > 0;
        }

        public async Task<bool> DeleteIssueAsync(Guid issueId)
        {
            var issue = new Issue { Id = issueId };
            _issueRepository.Delete(issue);
            var deleted = await _issueRepository.SaveAsync().ConfigureAwait(false);
            return deleted > 0;
        }

        public async Task<bool> DeleteIssueStageAsync(Guid issueStageId)
        {
            var issueStage = new IssueStage { Id = issueStageId };
            _issueStageRepository.Delete(issueStage);
            var deleted = await _issueStageRepository.SaveAsync().ConfigureAwait(false);
            return deleted > 0;
        }

        public async Task<bool> DeleteIssueTypeAsync(Guid issueTypeId)
        {
            var issueType = new IssueType { Id = issueTypeId };
            _issueTypeRepository.Delete(issueType);
            var deleted = await _issueTypeRepository.SaveAsync().ConfigureAwait(false);
            return deleted > 0;
        }

        public async Task<bool> UserOwnsIssueAsync(Guid issueId, string userId)
        {
            var issue = await _issueRepository
                .FindByCondition(x => x.Id == issueId)
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

        public async Task<(List<Issue> list, int count)> GetIssuesByProjectIdAsync(Guid projectId, PaginationFilter paginationFilter = null)
        {
            var queryable = _issueRepository
                .FindByCondition(x => x.ProjectId == projectId)
                .Include(x => x.Author);
            List<Issue> issues;
            var count = await queryable.CountAsync().ConfigureAwait(false);

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
            return (list: issues, count);

        }

        public async Task<(List<Issue> list, int count)> GetIssuesAsync(GetAllIssuesFilter filter, PaginationFilter paginationFilter = null)
        {
            var queryable = _issueRepository.FindAll();
            List<Issue> issues;

            if (!string.IsNullOrEmpty(filter?.SearchText))
            {

                queryable = _issueRepository.FindAllWithTextSearch(filter.SearchText);
            }

            if (filter == null)
            {
                return (list: new List<Issue>() { }, count: 0);
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
            }
            else
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
            return (list: issues, count);
        }

        public async Task<(List<Issue> list, int count)> GetIssuesByOrganizationAndProjectNameAsync(string organizationName, string projectName, PaginationFilter paginationFilter = null)
        {
            var queryable = _issueRepository
                    .FindByCondition(x => x.ParentOrganization.Name == organizationName && x.ParentProject.Name == projectName)
                    .Include(x => x.Author);
            List<Issue> issues;
            var count = await queryable.CountAsync().ConfigureAwait(false);

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
            return (list: issues, count);

        }

        public async Task<(List<Issue> list, int count)> GetIssuesByAuthorIdAsync(string authortId, PaginationFilter paginationFilter = null)
        {
            var queryable = _issueRepository
                    .FindByCondition(x => x.AuthorId == authortId)
                    .Include(x => x.Author);
            List<Issue> issues;
            var count = await queryable.CountAsync().ConfigureAwait(false);

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
            return (list: issues, count);
        }

        public async Task<(List<Issue> list, int count)> GetIssuesByAuthorUsernameAsync(string authortName, PaginationFilter paginationFilter = null)
        {
            var queryable = _issueRepository
                .FindByCondition(x => x.Author.UserName == authortName)
                .Include(x => x.Author);
            List<Issue> issues;
            var count = await queryable.CountAsync().ConfigureAwait(false);

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
            return (list: issues, count);

        }

        public async Task<(List<IssueUpdate> list, int count)> GetIssueUpdatesInTimePeriodAsync(Guid issueId, DateTime start, DateTime end, PaginationFilter paginationFilter = null)
        {
            var queryable = _issueUpdateRepository
                .FindByCondition(x => x.IssueId == issueId && end < x.CreatedAt && x.CreatedAt <= start);
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
            return (list: issueUpdates, count);

        }

        public async Task<(List<Issue> list, int count)> GetIssuesByProjectIdByTextSearchAsync(string authortId, PaginationFilter paginationFilter = null)
        {
            var queryable = _issueRepository
                .FindByCondition(x => x.AuthorId == authortId);
                    // .Include(x => x.Tags)
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

            return (list: issues, count);
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
                    await _issueTypeRepository
                    .FindByCondition(x =>
                        x.Name == stage.Name && x.ProjectId == stage.ProjectId)
                    .SingleOrDefaultAsync().ConfigureAwait(false);
                if (existingTag != null)
                    continue;

                await _issueStageRepository.CreateAsync(new IssueStage
                { Name = stage.Name, ProjectId = project.Id }).ConfigureAwait(false);
            }
            await _issueStageRepository.SaveAsync().ConfigureAwait(false);
        }

    }
}
