using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Repositories
{
    public class IssueUpdatePgRepository : PgRepositoryBase<IssueUpdate>, IIssueUpdateRepository
    {
        public IssueUpdatePgRepository(PgMainDataContext dataContext) : base(dataContext)
        {
        }
        public IQueryable<IssueUpdate> FindAllWithTextSearch(string searchTerms)
        {
            return this._dataContext.Set<IssueUpdate>().AsNoTracking()
                    .Where(iu => EF.Functions.TrigramsWordSimilarity(iu.Content, searchTerms) > 0.3); ;
        }
    }
}
