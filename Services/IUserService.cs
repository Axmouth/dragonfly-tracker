using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Services
{
    public interface IUserService
    {
        Task<(List<DragonflyUser> list, int count)> GetUsersAsync(GetAllUsersFilter filter, PaginationFilter paginationFilter = null);
        Task<DragonflyUser> GetUserByUserNameAsync(string username);
        Task<DragonflyUser> GetUserByEmailAsync(string email);
        Task<DragonflyUser> GetUserByIdAsync(string userId);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> UpdateUserAsync(DragonflyUser userToUpdate);
        Task<bool> UpdateEmailAsync(DragonflyUser userToUpdate, string oldEmail, string newEmail);

    }
}
