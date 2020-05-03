using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Repositories
{
    public interface IProjectRepository: IRepositoryBase<Project>
    {
        public IQueryable<Project> FindAllWithTextSearch(string searchTerms);
    }
}
