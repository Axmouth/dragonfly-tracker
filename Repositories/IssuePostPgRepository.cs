using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Repositories
{
    public class IssuePostPgRepository : PgRepositoryBase<IssuePost>, IIssuePostRepository
    {
        public IssuePostPgRepository(PgMainDataContext dataContext) : base(dataContext)
        {
        }
        public IQueryable<IssuePost> FindAllWithTextSearch(string searchTerms)
        {
            return this._dataContext.Set<IssuePost>().AsNoTracking()
                    .Where(ip => EF.Functions.TrigramsWordSimilarity(ip.Content, searchTerms) > 0.3); ;
        }
    }
}
