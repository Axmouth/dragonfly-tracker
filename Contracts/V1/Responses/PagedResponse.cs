using System.Collections.Generic;

namespace DragonflyTracker.Contracts.V1.Responses
{
    public class PagedResponse<T>
    {
        public PagedResponse(){}

        public PagedResponse(IEnumerable<T> data)
        {
            Data = data;
        }

        public PagedResponse(IEnumerable<T> data, int total)
        {
            Data = data;
            Total = total;
        }

        public IEnumerable<T> Data { get; set; }

        public int? PageNumber { get; set; }

        public int? Total { get; set; }

        public int? PageSize { get; set; }

        public string NextPage { get; set; }

        public string PreviousPage { get; set; }
    }
}