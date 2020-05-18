using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DragonflyTracker.Contracts.V1.Requests
{
    public class UserRegistrationRequest
    {
        [EmailAddress]
        [Required]
        [Display(Name = "email address")]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        [MaxLength(45)]
        [Display(Name = "password")]
        public string Password { get; set; }
        [MinLength(1)]
        [MaxLength(25)]
        [Required]
        [Display(Name = "userame")]
        public string UserName { get; set; }

    }
}