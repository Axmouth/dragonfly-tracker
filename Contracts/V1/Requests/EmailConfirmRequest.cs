﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Contracts.V1.Requests
{
    public class EmailConfirmRequest
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
