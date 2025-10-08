using CareNest_Review.Application.Common;
using Shared.Contracts;

namespace CareNest_Review.Application.Interfaces.Services
{
    public interface IShopService
    {
        Task<ResponseResult<ShopResponse>> GetShopById(string? id);

    }
}
