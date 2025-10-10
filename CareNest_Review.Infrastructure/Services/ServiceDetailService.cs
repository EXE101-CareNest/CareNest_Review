using CareNest_Review.Application.DTOs;
using CareNest_Review.Application.Interfaces.Services;
using CareNest_Review.Domain.Commons.Base;
using CareNest_Review.Domain.Commons.Constant;
using CareNest_Review.Infrastructure.ApiEndpoints;

namespace CareNest_Review.Infrastructure.Services
{
    public class ServiceDetailService : IServiceDetailService
    {
        private readonly IAPIService _apiService;

        public ServiceDetailService(IAPIService apiService)
        {
            _apiService = apiService;
        }
        public async Task<ServiceDetailDTO> GetById(string id)
        {
            var service = await _apiService.GetAsync<ServiceDetailDTO>("service", ServiceDetailEndpoints.GetById(id));
            if (!service.IsSuccess)
            {
                throw BaseException.BadRequestBadRequestResponse("ServiceDetail Id " + MessageConstant.NotFound);
            }
            return service.Data!.Data!;
        }
    }
}
