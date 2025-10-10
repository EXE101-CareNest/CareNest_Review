using CareNest_Review.Application.Features.Queries.GetAllPaging;
using CareNest_Review.Application.Interfaces.CQRS.Commands;

namespace CareNest_Review.Application.Features.Commands.Create
{
    public class CreateCommand : ICommand<ReviewResponse>
    {
        public string? CustomerId { get; set; }
        public string? ItemDetailId { get; set; }

        public double Rating { get; set; }
        public string? Contents { get; set; }
        public string? ImgUrl { get; set; } // ghi chú
        public int Type { get; set; }
    }
}
