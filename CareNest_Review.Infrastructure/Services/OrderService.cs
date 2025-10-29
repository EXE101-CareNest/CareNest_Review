using CareNest_Review.Application.DTOs;
using CareNest_Review.Application.Interfaces.Services;
using CareNest_Review.Domain.Commons.Base;
using CareNest_Review.Domain.Commons.Constant;
using CareNest_Review.Infrastructure.ApiEndpoints;

namespace CareNest_Review.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IAPIService _apiService;

        public OrderService(IAPIService apiService)
        {
            _apiService = apiService;
        }

        public async Task<OrderDetailDTO> GetById(string id)
        {
            var order = await _apiService.GetAsync<OrderDetailDTO>("order", OrderEndpoints.GetById(id));
            if (!order.IsSuccess)
            {
                throw BaseException.BadRequestBadRequestResponse("Order Id " + MessageConstant.NotFound);
            }
            return order.Data!.Data!;
        }
    }
}


