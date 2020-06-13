using DragonflyTracker.Domain;
using DragonflyTracker.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Services
{
    public class IssueTypeService : IIssueTypeService
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IIssueTypeRepository _issueTypeRepository;
        private readonly IIssueIssueTypeRepository _issueIssueTypeRepository;

        public IssueTypeService(IIssueRepository issueRepository, IIssueTypeRepository issueTypeRepository, IIssueIssueTypeRepository issueIssueTypeRepository)
        {
            _issueRepository = issueRepository;
            _issueTypeRepository = issueTypeRepository;
            _issueIssueTypeRepository = issueIssueTypeRepository;
        }

        public async Task<bool> CreateIssueTypeAsync(IssueType issueType)
        {
            if (issueType == null)
            {
                return false;
            }

            await _issueTypeRepository.CreateAsync(issueType).ConfigureAwait(false);

            var created = await _issueTypeRepository.SaveAsync().ConfigureAwait(false);
            return created > 0;
        }

        public async Task<bool> CreateIssueIssueTypeAsync(IssueIssueType issueIssueType)
        {
            if (issueIssueType == null)
            {
                return false;
            }

            await _issueIssueTypeRepository.CreateAsync(issueIssueType).ConfigureAwait(false);

            var created = await _issueIssueTypeRepository.SaveAsync().ConfigureAwait(false);
            return created > 0;
        }

        public async Task<bool> UpdateIssueTypeAsync(IssueType issueTypeToUpdate)
        {
            _issueTypeRepository.Update(issueTypeToUpdate);
            var updated = await _issueTypeRepository.SaveAsync().ConfigureAwait(false);
            return updated > 0;
        }

        public async Task<bool> DeleteIssueTypeAsync(Guid issueTypeId)
        {
            var issueType = new IssueType { Id = issueTypeId };
            _issueTypeRepository.Delete(issueType);
            var deleted = await _issueTypeRepository.SaveAsync().ConfigureAwait(false);
            return deleted > 0;
        }

        public async Task<bool> DeleteIssueIssueTypeAsync(Guid issueId, Guid issueTypeId )
        {
            var issueIssueType = new IssueIssueType { IssueTypeId = issueTypeId, IssueId = issueId };
            _issueIssueTypeRepository.Delete(issueIssueType);
            var deleted = await _issueIssueTypeRepository.SaveAsync().ConfigureAwait(false);
            return deleted > 0;
        }

        public async Task<(List<IssueType> list, int count)> GetAllIssueTypesByIssueAsync(Guid issueId, PaginationFilter paginationFilter = null)
        {
            var queryable = _issueIssueTypeRepository
                .FindByCondition(x => x.IssueId == issueId)
                .Include(iis => iis.IssueType);
            List<IssueType> issueTypes;
            var count = await queryable.CountAsync().ConfigureAwait(false);

            if (paginationFilter == null)
            {
                issueTypes = await queryable
                    .Select(iis => iis.IssueType)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
            else
            {
                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                issueTypes = await queryable
                        .Select(iis => iis.IssueType)
                        .Skip(skip)
                        .Take(paginationFilter.PageSize)
                        .ToListAsync()
                        .ConfigureAwait(false);
            }
            return (list: issueTypes, count);
        }

        public async Task<(List<IssueType> list, int count)> GetAllIssueTypesByProjectAsync(Guid projectId, PaginationFilter paginationFilter = null)
        {
            var queryable = _issueTypeRepository
                .FindByCondition(x => x.ProjectId == projectId);
            List<IssueType> issueTypes;
            var count = await queryable.CountAsync().ConfigureAwait(false);

            if (paginationFilter == null)
            {
                issueTypes = await queryable
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
            else
            {
                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                issueTypes = await queryable
                        .Skip(skip)
                        .Take(paginationFilter.PageSize)
                        .ToListAsync()
                        .ConfigureAwait(false);
            }
            return (list: issueTypes, count);
        }

        public async Task<IssueType> GetIssueTypeByProjectAsync(Guid projectId, string name)
        {
            var queryable = _issueTypeRepository
                .FindByCondition(x => x.ProjectId == projectId && x.Name == name);
            IssueType issueType;
            issueType = await queryable.SingleOrDefaultAsync().ConfigureAwait(false);
            return issueType;
        }
    }
}
