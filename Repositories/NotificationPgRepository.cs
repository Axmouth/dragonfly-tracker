using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Repositories
{
    public class NotificationPgRepository : PgRepositoryBase<Notification>, INotificationRepository
    {
        public NotificationPgRepository(PgMainDataContext dataContext) : base(dataContext)
        {
        }
    }
}
