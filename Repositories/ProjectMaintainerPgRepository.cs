using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Repositories
{
    public class ProjectMaintainerPgRepository : PgRepositoryBase<ProjectMaintainer>, IProjectMaintainerRepository
    {
        public ProjectMaintainerPgRepository(PgMainDataContext dataContext) : base(dataContext)
        {
        }
    }
}
