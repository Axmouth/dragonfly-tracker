using DragonflyTracker.Domain;
using DragonflyTracker.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Services
{
    public class IssueStageService : IIssueStageService
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IIssueStageRepository _issueStageRepository;

        public IssueStageService(IIssueRepository issueRepository, IIssueStageRepository issueStageRepository)
        {
            _issueRepository = issueRepository;
            _issueStageRepository = issueStageRepository;
        }

        public async Task<bool> CreateIssueStageAsync(IssueStage issueStage)
        {
            if (issueStage == null)
            {
                return false;
            }

            await _issueStageRepository.CreateAsync(issueStage).ConfigureAwait(false);

            var created = await _issueStageRepository.SaveAsync().ConfigureAwait(false);
            return created > 0;
        }

        public async Task<bool> UpdateIssueStageAsync(IssueStage issueStageToUpdate)
        {
            _issueStageRepository.Update(issueStageToUpdate);
            var updated = await _issueStageRepository.SaveAsync().ConfigureAwait(false);
            return updated > 0;
        }

        public async Task<bool> DeleteIssueStageAsync(Guid issueStageId)
        {
            var issueStage = new IssueStage { Id = issueStageId };
            _issueStageRepository.Delete(issueStage);
            var deleted = await _issueStageRepository.SaveAsync().ConfigureAwait(false);
            return deleted > 0;
        }

        public async Task<IssueStage> GetIssueStageByIssueAsync(Guid issueId)
        {
            var queryable = _issueRepository
                .FindByCondition(i => i.Id == issueId)
                .Include(i => i.CurrentStage)
                .Select(i => i.CurrentStage);
            return await queryable.SingleOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<(List<IssueStage> list, int count)> GetIssueStagesByProjectAsync(Guid projectId, PaginationFilter paginationFilter = null)
        {
            var queryable = _issueStageRepository
                .FindByCondition(x => x.ProjectId == projectId);
            List<IssueStage> issueStages;
            var count = await queryable.CountAsync().ConfigureAwait(false);

            if (paginationFilter == null)
            {
                issueStages = await queryable
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
            else
            {
                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                issueStages = await queryable
                        .Skip(skip)
                        .Take(paginationFilter.PageSize)
                        .ToListAsync()
                        .ConfigureAwait(false);
            }
            return (list: issueStages, count);
        }

        public async Task<IssueStage> GetIssueStageByProjectAsync(Guid projectId, string name)
        {
            var queryable = _issueStageRepository
                .FindByCondition(x => x.ProjectId == projectId && x.Name == name);
            return await queryable.SingleOrDefaultAsync().ConfigureAwait(false);
        }
    }
}
