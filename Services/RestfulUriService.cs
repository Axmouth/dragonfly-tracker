using System;
using Microsoft.AspNetCore.WebUtilities;
using DragonflyTracker.Contracts.V1;
using DragonflyTracker.Contracts.V1.Requests.Queries;
using Microsoft.AspNetCore.Http;

namespace DragonflyTracker.Services
{
    public class RestfulUriService : IUriService
    {
        private readonly string _baseUri;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public RestfulUriService(string baseUri, IHttpContextAccessor httpContextAccessor)
        {
            _baseUri = baseUri;
            _httpContextAccessor = httpContextAccessor;
        }

        public Uri GetUri(string idAttribute)
        {
            string resourcePath = _httpContextAccessor.HttpContext.Request.Path;
            return new Uri(_baseUri + resourcePath + "/" + idAttribute);
        }

        public Uri GetPagedUri(PaginationQuery pagination = null)
        {
            string resourcePath = _httpContextAccessor.HttpContext.Request.Path;
            var uri = new Uri(_baseUri + resourcePath);

            if (pagination == null)
            {
                return uri;
            }

            var modifiedUri = QueryHelpers.AddQueryString(_baseUri + resourcePath, "pageNumber", pagination.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", pagination.PageSize.ToString());

            return new Uri(modifiedUri);
        }
    }
}