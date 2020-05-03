using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Repositories
{
    public interface IIssueRepository: IRepositoryBase<Issue>
    {
        public IQueryable<Issue> FindAllWithTextSearch(string searchTerms);
    }
}
