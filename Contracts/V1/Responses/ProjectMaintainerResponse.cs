using AutoMapper;
using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Contracts.V1.Responses
{
    [AutoMap(typeof(ProjectMaintainer))]
    public class ProjectMaintainerResponse
    {
        public DragonflyUserResponse Maintainer { get; set; }
    }
}
