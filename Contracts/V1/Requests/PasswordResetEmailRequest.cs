using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Contracts.V1.Requests
{
    public class PasswordResetEmailRequest
    {
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
