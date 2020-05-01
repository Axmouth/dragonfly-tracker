using System;
using DragonflyTracker.Contracts.V1.Requests.Queries;

namespace DragonflyTracker.Services
{
    public interface IUriService
    {

        Uri GetUri(string idAttribute);

        Uri GetPagedUri(PaginationQuery pagination = null);
    }
}