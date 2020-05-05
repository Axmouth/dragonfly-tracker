using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Repositories
{
    public interface IIssueIssueTypeRepository : IRepositoryBase<IssueIssueType>
    {
        Task AddRangeAsync(List<IssueIssueType> list);
    }
}
