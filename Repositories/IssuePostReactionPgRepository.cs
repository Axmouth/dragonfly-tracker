using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Repositories
{
    public class IssuePostReactionPgRepository : PgRepositoryBase<IssuePostReaction>, IIssuePostReactionRepository
    {
        public IssuePostReactionPgRepository(PgMainDataContext dataContext) : base(dataContext)
        {
        }
    }
}
