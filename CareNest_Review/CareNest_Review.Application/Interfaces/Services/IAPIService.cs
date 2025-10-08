using CareNest_Review.Application.Common;

namespace CareNest_Review.Application.Interfaces.Services
{
    public interface IAPIService
    {
        Task<ResponseResult<T>> GetAsync<T>(string serviceType, string url);

        Task<ResponseResult<T>> PostAsync<T>(string serviceType, string url, object data);

        Task<ResponseResult<T>> PutAsync<T>(string serviceType, string url, object data);

        Task<ResponseResult<T>> DeleteAsync<T>(string serviceType, string url);

    }
}
