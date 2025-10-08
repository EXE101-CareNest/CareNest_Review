using CareNest_Review.Application.Features.Queries.GetAllPaging;
using CareNest_Review.Application.Interfaces.CQRS.Queries;

namespace CareNest_Review.Application.Features.Queries.GetById
{
    public class GetByIdQuery : IQuery<AppointmentResponse>
    {
        public required string Id { get; set; }
    }
}
