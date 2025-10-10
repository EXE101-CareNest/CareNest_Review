using CareNest_Review.Application.DTOs;

namespace CareNest_Review.Application.Interfaces.Services
{
    public interface IProductDetailService
    {
        Task<ProductDetailDTO> GetbyId(string id);
    }
}
