using System;
using DragonflyTracker.Contracts.V1.Requests.Queries;

namespace DragonflyTracker.Services
{
    public interface IUriService
    {
        Uri GetPostUri(string postId);

        Uri GetAllPostsUri(PaginationQuery pagination = null);
    }
}