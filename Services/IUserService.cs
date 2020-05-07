using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Services
{
    public interface IUserService
    {
        Task<DragonflyUser> GetUserAsync(string username);
        Task<(List<DragonflyUser> list, int count)> GetUsersAsync(GetAllUsersFilter filter, PaginationFilter paginationFilter = null);
    }
}
