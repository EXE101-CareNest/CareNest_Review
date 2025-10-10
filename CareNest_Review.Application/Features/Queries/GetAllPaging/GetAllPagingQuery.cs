using CareNest_Review.Application.Common;
using CareNest_Review.Application.Interfaces.CQRS.Queries;


namespace CareNest_Review.Application.Features.Queries.GetAllPaging
{
    public class GetAllPagingQuery : IQuery<PageResult<ReviewResponse>>
    {
        public int Index { get; set; }
        public int PageSize { get; set; }
        public string? SortColumn { get; set; } 
        public string? SortDirection { get; set; }
        public int? Type { get; set; }
        public string? CustomerId { get; set; }
        public string? ProductDetailId { get; set; }
        public string? ServiceDetailId { get; set; }
    }
}
