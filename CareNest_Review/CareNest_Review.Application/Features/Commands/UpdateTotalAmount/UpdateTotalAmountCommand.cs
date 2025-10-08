using CareNest_Review.Application.Features.Queries.GetAllPaging;
using CareNest_Review.Application.Interfaces.CQRS.Commands;

namespace CareNest_Review.Application.Features.Commands.UpdateTotalAmount
{
    public class UpdateTotalAmountCommand : ICommand<AppointmentResponse>
    {
        public string Id { get; set; } = string.Empty;
        public double TotalAmount { get; set; }
    }
}
