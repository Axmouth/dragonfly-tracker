using System.Linq;

namespace DragonflyTracker.Domain
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}