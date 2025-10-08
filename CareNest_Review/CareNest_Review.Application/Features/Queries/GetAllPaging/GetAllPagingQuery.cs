using CareNest_Review.Application.Common;
using CareNest_Review.Application.Interfaces.CQRS.Queries;


namespace CareNest_Review.Application.Features.Queries.GetAllPaging
{
    public class GetAllPagingQuery : IQuery<PageResult<AppointmentResponse>>
    {
        public int Index { get; set; }
        public int PageSize { get; set; }
        public string? SortColumn { get; set; } // "Name", "Note", "CreatedAt"
        public string? SortDirection { get; set; } // "asc" or "desc"
        public string? Status { get; set; }
        public string? CustomerId { get; set; } 
        public string? ShopId { get; set; } 
    }
}
