using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Domain
{
    public class DragonflyUser : IdentityUser
    {
        [MaxLength(30)]
        public string Title { get; set; }

        [Required]
        [MaxLength(450)]
        [MinLength(2)]
        public string Description { get; set; }

        public virtual List<IssuePostReaction> Reactions { get; set; }

        public virtual List<Notification> Notifications { get; set; }

        public virtual List<Issue> Issues { get; set; }

        public virtual List<IssuePost> IssuePosts { get; set; }

        public virtual List<ProjectAdmin> AdminedProjects { get; set; }

        public virtual List<ProjectMaintainer> MaintainedProjects { get; set; }

        public virtual List<Project> CreatedProjects { get; set; }
        public virtual List<Project> OwnedProjects { get; set; }
    }
}
