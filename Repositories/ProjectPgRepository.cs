using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Repositories
{
    public class ProjectPgRepository : PgRepositoryBase<Project>, IProjectRepository
    {
        public ProjectPgRepository(PgMainDataContext dataContext) : base(dataContext)
        {
        }
        public IQueryable<Project> FindAllWithTextSearch(string searchTerms)
        {
            return this._dataContext.Set<Project>().AsNoTracking()
                    // .Where(i => EF.Functions.ToTsVector("english", i.Title).Matches(filter.SearchText));
                    // .Where(i => EF.Functions.FuzzyStringMatchLevenshteinLessEqual(i.Title, filter.SearchText, 5) <= 5);
                    .Where(p => EF.Functions.TrigramsWordSimilarity(p.Name, searchTerms) > 0.0)
                    .OrderByDescending(p => EF.Functions.TrigramsWordSimilarity(p.Name, searchTerms));
        }
    }
}
