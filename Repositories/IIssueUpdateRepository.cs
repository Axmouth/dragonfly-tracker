using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Repositories
{
    public interface IIssueUpdateRepository : IRepositoryBase<IssueUpdate>
    {
        public IQueryable<IssueUpdate> FindAllWithTextSearch(string searchTerms);
    }
}
