using CareNest_Review.Application.Common;
using CareNest_Review.Application.DTOs;
using CareNest_Review.Application.Interfaces.CQRS.Queries;
using CareNest_Review.Application.Interfaces.Services;
using CareNest_Review.Application.Interfaces.UOW;
using CareNest_Review.Domain.Entitites;
using System.Linq.Expressions;

namespace CareNest_Review.Application.Features.Queries.GetAllPaging
{
    public class GetAllPagingQueryHandler : IQueryHandler<GetAllPagingQuery, PageResult<ReviewResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductDetailService _appointmentDetailService;
        private readonly IServiceDetailService _shopService;

        public GetAllPagingQueryHandler(IUnitOfWork unitOfWork, IProductDetailService appointmentDetailService, IServiceDetailService shopService)
        {
            _unitOfWork = unitOfWork;
            _appointmentDetailService = appointmentDetailService;
            _shopService = shopService;
        }

        public async Task<PageResult<ReviewResponse>> HandleAsync(GetAllPagingQuery query)
        {
            Expression<Func<Review, bool>>? predicate = null;
            if (!string.IsNullOrWhiteSpace(query.CustomerId))
            {
                predicate = ad => ad.CustomerId.Contains(query.CustomerId);
            }
            if (!string.IsNullOrWhiteSpace(query.ProductDetailId))
            {
                predicate = ad => ad.ItemDetailId.Contains(query.ProductDetailId) && ad.Type == 2;
            }
            if (!string.IsNullOrWhiteSpace(query.ServiceDetailId))
            {
                predicate = ad => ad.ItemDetailId.Contains(query.ServiceDetailId) && ad.Type == 1;
            }
            if (!string.IsNullOrWhiteSpace(query.Type))
            {
                predicate = ad => ad.Type.Equals(query.Type);
            }
            var selector = ObjectMapperExtensions.CreateMapExpression<Review, ReviewResponse>();

            var orderByFunc = GetOrderByFunc(query.SortColumn, query.SortDirection);

            var totalItems = await _unitOfWork.GetRepository<Review>().CountAsync(null);

            IEnumerable<ReviewResponse> appointments = await _unitOfWork.GetRepository<Review>().FindAsync(
                predicate: predicate,
                orderBy: orderByFunc,
                selector: selector,
                pageSize: query.PageSize,
                pageIndex: query.Index);

            var appointmentsList = appointments.ToList();

          
            return new PageResult<ReviewResponse>(appointmentsList, totalItems, query.Index, query.PageSize);
        }


        private Func<IQueryable<Review>, IOrderedQueryable<Review>> GetOrderByFunc(string? sortColumn, string? sortDirection)
        {
            var ascending = string.IsNullOrWhiteSpace(sortDirection) || sortDirection.ToLower() != "desc";

            return sortColumn?.ToLower() switch
            {
                "updateat" => q => ascending ? q.OrderBy(a => a.UpdatedAt) : q.OrderByDescending(a => a.UpdatedAt),
                _ => q => q.OrderBy(a => a.CreatedAt)
            };
        }
    }
}
