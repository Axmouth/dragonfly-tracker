using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Repositories
{
    public class IssuePgRepository : PgRepositoryBase<Issue>, IIssueRepository
    {
        public IssuePgRepository(PgMainDataContext dataContext) : base(dataContext)
        {
        }
        public IQueryable<Issue> FindAllWithTextSearch(string searchTerms)
        {
            return this._dataContext.Set<Issue>().AsNoTracking()
                    .Where(i => EF.Functions.ToTsVector("english", i.Title + ' ' + i.Content).Matches(searchTerms));
        }
    }
}
