using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Repositories
{
    public class UserPgRepository : PgRepositoryBase<DragonflyUser>, IUserRepository
    {
        public UserPgRepository(PgMainDataContext dataContext) : base(dataContext)
        {
        }
        public IQueryable<DragonflyUser> FindAllWithTextSearch(string searchTerms)
        {
            return this._dataContext.Set<DragonflyUser>().AsNoTracking();
        }
    }
}
