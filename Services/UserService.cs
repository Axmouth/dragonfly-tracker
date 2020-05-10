using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using DragonflyTracker.Migrations;
using DragonflyTracker.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Services
{
    public class UserService : IUserService
    {
        private readonly PgMainDataContext _dataContext;
        private readonly UserManager<DragonflyUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserRepository _userRepository;

        public UserService(UserManager<DragonflyUser> userManager, RoleManager<IdentityRole> roleManager, PgMainDataContext dataContext, IUserRepository userRepository)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _userRepository = userRepository;
        }

        public async Task<DragonflyUser> GetProjectByUserAsync(string username)
        {
            return await _userManager
                .FindByNameAsync(username)
                .ConfigureAwait(false);
        }

        public async Task<DragonflyUser> GetUserAsync(string username)
        {
            return await _userManager.FindByNameAsync(username).ConfigureAwait(false);
        }

        public async Task<DragonflyUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
        }

        public async Task<(List<DragonflyUser> list, int count)> GetUsersAsync(GetAllUsersFilter filter, PaginationFilter paginationFilter = null)
        {
            var queryable = _userRepository
                .FindAll();
            if (!string.IsNullOrEmpty(filter?.SearchText))
            {
                queryable = _userRepository
                    .FindAllWithTextSearch(filter.SearchText);
            }
            List<DragonflyUser> users;
            int count = 0;
            if (!string.IsNullOrEmpty(filter?.MaintainedProjectName))
            {
                queryable = queryable.Where(u => u.MaintainedProjects.Any(p => p.Project.Name == filter.MaintainedProjectName));
            }
            if (!string.IsNullOrEmpty(filter?.AdminedProjectName))
            {
                queryable = queryable.Where(u => u.AdminedProjects.Any(p => p.Project.Name == filter.AdminedProjectName));
            }
            if (!string.IsNullOrEmpty(filter?.OwnedProjectName))
            {
                queryable = queryable.Where(u => u.OwnedProjects.Any(p => p.Name == filter.OwnedProjectName));
            }
            if (!string.IsNullOrEmpty(filter?.UnderOrgName))
            {
                queryable = queryable.Where(u =>
                u.OwnedProjects.Any(p => p.ParentOrganization.Name == filter.UnderOrgName) ||
                u.AdminedProjects.Any(p => p.Project.ParentOrganization.Name == filter.UnderOrgName) ||
                u.MaintainedProjects.Any(p => p.Project.ParentOrganization.Name == filter.UnderOrgName)
                );
            }
            if (!string.IsNullOrEmpty(filter?.UnderUsername))
            {
                queryable = queryable.Where(u => u.OwnedProjects.Any(p =>
                p.Creator.UserName == filter.UnderUsername) ||
                u.AdminedProjects.Any(p => p.Project.Creator.UserName == filter.UnderUsername) ||
                u.MaintainedProjects.Any(p => p.Project.Creator.UserName == filter.UnderUsername)
                );
            }
            count = await queryable.CountAsync().ConfigureAwait(false);
            if (paginationFilter == null)
            {
                users = await queryable
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
            else
            {
                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                users = await queryable
                    .Skip(skip)
                    .Take(paginationFilter.PageSize)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
            return (users, count);
        }
        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = new DragonflyUser { Id = userId };
            var result = await _userManager.DeleteAsync(user).ConfigureAwait(false);
            var deleted = result.Succeeded;
            return deleted;
        }

        public async Task<bool> UpdateUserAsync(DragonflyUser userToUpdate)
        {
            if (userToUpdate == null)
            {
                return false;
            }
            var result = await _userManager.UpdateAsync(userToUpdate).ConfigureAwait(false);
            var updated = result.Succeeded;
            return updated;
        }

        public async Task<bool> UpdateEmailAsync(DragonflyUser userToUpdate, string oldEmail, string newEmail)
        {
            var token = await _userManager.GenerateChangeEmailTokenAsync(userToUpdate, newEmail).ConfigureAwait(false);
            var result = await _userManager.ChangeEmailAsync(userToUpdate, newEmail, token).ConfigureAwait(false);
            return result.Succeeded;
        }
    }
}
