using AutoMapper;
using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Contracts.V1.Responses
{
    [AutoMap(typeof(ProjectAdmin))]
    public class ProjectAdminResponse
    {
        public DragonflyUserResponse Admin { get; set; }
    }
}
