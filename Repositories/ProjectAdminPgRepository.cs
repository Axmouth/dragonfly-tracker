using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Repositories
{
    public class ProjectAdminPgRepository : PgRepositoryBase<ProjectAdmin>, IProjectAdminRepository
    {
        public ProjectAdminPgRepository(PgMainDataContext dataContext) : base(dataContext)
        {
        }
    }
}
