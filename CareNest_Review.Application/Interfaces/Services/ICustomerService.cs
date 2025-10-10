
using CareNest_Review.Application.DTOs;

namespace CareNest_Review.Application.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<CustomerDto> GetbyId(string id);
    }
}
