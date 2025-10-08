using CareNest_Review.Application.Common;
using CareNest_Review.Application.Interfaces.Services;
using CareNest_Review.Domain.Commons.Base;
using CareNest_Review.Domain.Commons.Constant;
using CareNest_Review.Infrastructure.ApiEndpoints;
using Shared.Contracts;

namespace CareNest_Review.Infrastructure.Services
{
    public class ShopService : IShopService
    {
        private readonly IAPIService _apiService;

        public ShopService(IAPIService apiService)
        {
            _apiService = apiService;
        }
        public async Task<ResponseResult<ShopResponse>> GetShopById(string? id)
        {
            var shop = await _apiService.GetAsync<ShopResponse>("shop", ServiceDetailEndpoints.GetById(id));
            if (!shop.IsSuccess)
            {
                throw BaseException.BadRequestBadRequestResponse("Shop Id " + MessageConstant.NotFound);
            }
            return shop;
        }
    }
}
