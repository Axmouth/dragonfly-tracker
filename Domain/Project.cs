using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Domain
{
    public class Project
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Public { get; set; }

        [Column(TypeName = "Date")]
        public DateTime CreatedAt { get; set; }

        public Guid CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Project ParentCompany { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public IdentityUser Creator { get; set; }

        public virtual List<IdentityUser> Admins { get; set; }

        public virtual List<IdentityUser> Maintainers { get; set; }

        public virtual List<Issue> Issues { get; set; }

        public virtual List<IssueType> Types { get; set; }

        public virtual List<IssueStage> Stages { get; set; }
    }
}
