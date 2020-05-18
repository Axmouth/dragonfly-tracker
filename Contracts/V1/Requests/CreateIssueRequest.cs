using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Contracts.V1.Requests
{
    public class CreateIssueRequest
    {
        [Required]
        [MaxLength(150)]
        [MinLength(3)]
        public string Title { get; set; }
        [Required]
        [MaxLength(2500)]
        [MinLength(15)]
        public string Content { get; set; }
        public virtual List<IssueType> Types { get; set; }
    }
}
