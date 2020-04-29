using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Services
{
    public class UserService:IUserService
    {
        private readonly PgMainDataContext _dataContext;
        private readonly UserManager<DragonflyUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<DragonflyUser> userManager, RoleManager<IdentityRole> roleManager, PgMainDataContext dataContext)
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
