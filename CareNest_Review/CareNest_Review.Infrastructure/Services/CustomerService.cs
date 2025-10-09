using CareNest_Review.Application.DTOs;
using CareNest_Review.Application.Interfaces.Services;
using CareNest_Review.Domain.Commons.Base;
using CareNest_Review.Domain.Commons.Constant;
using CareNest_Review.Infrastructure.ApiEndpoints;

namespace CareNest_Review.Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IAPIService _apiService;

        public CustomerService(IAPIService apiService)
        {
            _apiService = apiService;
        }

        public async Task<CustomerDto> GetbyId(string id)
        {
            var customer = await _apiService.GetAsync<CustomerDto>("customer", CustomerEndpoints.GetById(id));
            if (!customer.IsSuccess)
            {
                throw BaseException.BadRequestBadRequestResponse("Customer Id " + MessageConstant.NotFound);
            }
            return customer.Data!.Data!;
        }
    }

}
