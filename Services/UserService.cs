using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Services
{
    public class UserService
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<DragonflyUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<DragonflyUser> userManager, RoleManager<IdentityRole> roleManager, DataContext dataContext)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<DragonflyUser> GetProjectByUserAsync(string username)
        {
            return await _userManager
                .FindByNameAsync(username)
                .ConfigureAwait(false);
        }
    }
}
