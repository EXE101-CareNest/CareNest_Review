using CareNest_Review.Application.DTOs;

namespace CareNest_Review.Application.Interfaces.Services
{
    public interface IServiceDetailService
    {
        Task<ServiceDetailDTO> GetById(string? id);

    }
}
