using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.EmailTemplates.ViewModels
{
    public class PasswordResetViewModel
    {
        public string ResetLink { get; set; }
        public string UserName { get; set; }

        public PasswordResetViewModel(string resetLink, string username)
        {
            ResetLink = resetLink;
            UserName = username;
        }
    }
}
