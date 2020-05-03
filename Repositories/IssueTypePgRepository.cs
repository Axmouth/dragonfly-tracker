using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Repositories
{
    public class IssueTypePgRepository : PgRepositoryBase<IssueType>, IIssueTypeRepository
    {
        public IssueTypePgRepository(PgMainDataContext dataContext) : base(dataContext)
        {
        }
        public IQueryable<IssueType> FindAllWithTextSearch(string searchTerms)
        {
            return this._dataContext.Set<IssueType>().AsNoTracking()
                    .Where(it => EF.Functions.TrigramsWordSimilarity(it.Name, searchTerms) > 0.3); ;
        }
    }
}
