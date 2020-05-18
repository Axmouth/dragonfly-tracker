using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Contracts.V1.Requests
{
    public class CreateIssuePostRequest
    {
        [Required]
        [MaxLength(1000)]
        [MinLength(3)]
        public string Content { get; set; }
    }
}
