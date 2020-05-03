using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Repositories
{
    public class IssueStagePgRepositorycs : PgRepositoryBase<IssueStage>, IIssueStageRepository
    {
        public IssueStagePgRepositorycs(PgMainDataContext dataContext) : base(dataContext)
        {
        }
        public IQueryable<IssueStage> FindAllWithTextSearch(string searchTerms)
        {
            return this._dataContext.Set<IssueStage>().AsNoTracking()
                    .Where(istage => EF.Functions.TrigramsWordSimilarity(istage.Name, searchTerms) > 0.3); ;
        }
    }
}
