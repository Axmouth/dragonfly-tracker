using System.Collections.Generic;
using System.Linq;
using DragonflyTracker.Contracts.V1.Requests.Queries;
using DragonflyTracker.Contracts.V1.Responses;
using DragonflyTracker.Domain;
using DragonflyTracker.Services;

namespace DragonflyTracker.Helpers
{
    public class PaginationHelpers
    {
        public static PagedResponse<T> CreatePaginatedResponse<T>(IUriService uriService, PaginationFilter pagination, List<T> response)
        {
             var nextPage = pagination.PageNumber >= 1
                ? uriService.GetPagedUri(new PaginationQuery(pagination.PageNumber + 1, pagination.PageSize)).ToString()
                : null;
            
            var previousPage = pagination.PageNumber - 1 >= 1
                ? uriService.GetPagedUri(new PaginationQuery(pagination.PageNumber - 1, pagination.PageSize)).ToString()
                : null;

            return new PagedResponse<T>
            {
                Data = response,
                PageNumber = pagination.PageNumber >= 1 ? pagination.PageNumber : (int?)null,
                PageSize = pagination.PageSize >= 1 ? pagination.PageSize : (int?)null,
                NextPage = response.Any() ? nextPage : null,
                PreviousPage = previousPage
            };
        }
        public static PagedResponse<T> CreatePaginatedResponse<T>(IUriService uriService, PaginationFilter pagination, List<T> response, int count)
        {
            var nextPage = pagination.PageNumber >= 1
               ? uriService.GetPagedUri(new PaginationQuery(pagination.PageNumber + 1, pagination.PageSize)).ToString()
               : null;

            var previousPage = pagination.PageNumber - 1 >= 1
                ? uriService.GetPagedUri(new PaginationQuery(pagination.PageNumber - 1, pagination.PageSize)).ToString()
                : null;

            return new PagedResponse<T>
            {
                Data = response,
                PageNumber = pagination.PageNumber >= 1 ? pagination.PageNumber : (int?)null,
                PageSize = pagination.PageSize >= 1 ? pagination.PageSize : (int?)null,
                NextPage = response.Any() ? nextPage : null,
                PreviousPage = previousPage,
                Total = count
            };
        }
    }
}