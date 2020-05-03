using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Repositories
{
    public interface IIssueTypeRepository : IRepositoryBase<IssueType>
    {
        public IQueryable<IssueType> FindAllWithTextSearch(string searchTerms);
    }
}
