using AutoMapper;
using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Contracts.V1.Responses
{
    [AutoMap(typeof(DragonflyUser))]
    public class DragonflyUserResponse
    {
        public string UserName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        /*
        public virtual List<IssuePostReactionResponse> Reactions { get;  }
        public virtual List<IssueResponse> Issues { get;  }
        public virtual List<IssuePostResponse> IssuePosts { get;  }
        public virtual List<ProjectAdminResponse> AdminedProjects { get;  }
        public virtual List<ProjectMaintainerResponse> MaintainedProjects { get;  }
        public List<ProjectResponse> CreatedProjects { get;  }
        public List<ProjectResponse> OwnedProjects { get;  }
        */
    }
}
