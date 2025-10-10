using CareNest_Review.Application.Features.Queries.GetAllPaging;
using CareNest_Review.Application.Interfaces.CQRS.Commands;

namespace CareNest_Review.Application.Features.Commands.Update
{
    public class UpdateCommand : ICommand<ReviewResponse>
    {
        public string Id { get; set; } = string.Empty;
        public double Rating { get; set; }
        public string? Contents { get; set; }
        public string? ImgUrl { get; set; }
    }
}
