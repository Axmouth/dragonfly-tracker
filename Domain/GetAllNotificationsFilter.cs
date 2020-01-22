using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Domain
{
    //[AutoMap(typeof(GetAllNotificationsQuery))]
    public class GetAllNotificationsFilter
    {
        public Guid UserId { get; set; }

        public string Username { get; set; }

        public string UserEmail { get; set; }
    }
}
