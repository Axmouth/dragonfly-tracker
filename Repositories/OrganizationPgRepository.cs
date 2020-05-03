using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Repositories
{
    public class OrganizationPgRepository : PgRepositoryBase<Organization>, IOrganizationRepository
    {
        public OrganizationPgRepository(PgMainDataContext dataContext) : base(dataContext)
        {
        }
    }
}
