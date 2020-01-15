using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Domain
{
    public class Company
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        [Column(TypeName = "Date")]
        public DateTime CreatedAt { get; set; }

        public virtual List<Project> Projects { get; set; }

        public virtual List<Issue> Issues { get; set; }
    }
}
