using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Repositories
{
    public class IssueIssueTypePgRepository : PgRepositoryBase<IssueIssueType>, IIssueIssueTypeRepository
    {
        public IssueIssueTypePgRepository(PgMainDataContext dataContext) : base(dataContext)
        {
        }
        public async Task AddRangeAsync(List<IssueIssueType> list)
        {
            await _dataContext.AddRangeAsync(list).ConfigureAwait(false);
        }
    }
}
