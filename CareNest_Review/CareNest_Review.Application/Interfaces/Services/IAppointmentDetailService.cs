using CareNest_Review.Application.Features.Commands.Create;
using CareNest_Review.Application.DTOs;

namespace CareNest_Review.Application.Interfaces.Services
{
    public interface IAppointmentDetailService
    {
        Task<AppointmentDetailDto> CreateAppointmentDetailAsync(string appointmentId, AppointmentDetailInput detail);
        Task<List<AppointmentDetailDto>> GetAppointmentDetailsAsync(string appointmentId);
    }
}
