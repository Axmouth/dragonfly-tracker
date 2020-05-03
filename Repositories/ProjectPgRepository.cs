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
                    .Where(p => EF.Functions.ToTsVector("english", p.Name + ' ' + p.Description).Matches(searchTerms));
        }
    }
}
