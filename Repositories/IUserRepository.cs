using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Repositories
{
    public interface IUserRepository: IRepositoryBase<DragonflyUser>
    {
        public IQueryable<DragonflyUser> FindAllWithTextSearch(string searchTerms);
    }
}
