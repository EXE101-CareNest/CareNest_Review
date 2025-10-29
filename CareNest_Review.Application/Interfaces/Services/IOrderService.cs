using CareNest_Review.Application.DTOs;

namespace CareNest_Review.Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<OrderDetailDTO> GetById(string id);
    }
}


