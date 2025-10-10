using CareNest_Review.Application.DTOs;
using CareNest_Review.Application.Interfaces.Services;
using CareNest_Review.Domain.Commons.Base;
using CareNest_Review.Domain.Commons.Constant;
using CareNest_Review.Infrastructure.ApiEndpoints;

namespace CareNest_Review.Infrastructure.Services
{
    public class ProductDetailService : IProductDetailService
    {
        private readonly IAPIService _apiService;

        public ProductDetailService(IAPIService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ProductDetailDTO> GetbyId(string id)
        {
            var product = await _apiService.GetAsync<ProductDetailDTO>("product", ProductDetailEndpoints.GetById(id));
            if (!product.IsSuccess)
            {
                throw BaseException.BadRequestBadRequestResponse("ProductDetail Id " + MessageConstant.NotFound);
            }
            return product.Data!.Data!;
        }


    }
}
