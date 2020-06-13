using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Contracts.V1.Requests
{
    public class CreateIssueTypeRequest
    {
        [Required]
        [MaxLength(30)]
        [MinLength(2)]
        public string Name { get; set; }
    }
}
