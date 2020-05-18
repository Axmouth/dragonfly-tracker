using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace DragonflyTracker.Extensions
{
    public static class GeneralExtensions
    {
        public static Guid GetUserId(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return Guid.Empty;
            }

            return new Guid(httpContext.User.Claims.Single(x => x.Type == "id").Value);
        }
    }
}