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
            // Auto-infer Type=3 when Order filters are provided without Type
            if ((query.OrderIds != null && query.OrderIds.Any()) || !string.IsNullOrWhiteSpace(query.OrderId))
            {
                if (query.Type == null)
                {
                    query.Type = 3;
                }
            }

            // Prepare OrderId list if needed
            List<string>? orderIdList = null;
            if (!string.IsNullOrWhiteSpace(query.OrderId) || (query.OrderIds != null && query.OrderIds.Any()))
            {
                var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                if (query.OrderIds != null)
                {
                    foreach (var id in query.OrderIds.Where(x => !string.IsNullOrWhiteSpace(x)))
                    {
                        set.Add(id);
                    }
                }
                if (!string.IsNullOrWhiteSpace(query.OrderId))
                {
                    set.Add(query.OrderId);
                }
                orderIdList = set.ToList();
            }

            // Build predicate with all conditions combined
            Expression<Func<Review, bool>> predicate = ad =>
                // Type filter
                (query.Type == null || ad.Type == query.Type.Value) &&
                // CustomerId filter
                (string.IsNullOrWhiteSpace(query.CustomerId) || (ad.CustomerId != null && ad.CustomerId.Contains(query.CustomerId))) &&
                // ProductDetailId filter (Type must be 2)
                (string.IsNullOrWhiteSpace(query.ProductDetailId) || (ad.ItemDetailId != null && ad.ItemDetailId.Contains(query.ProductDetailId) && ad.Type == 2)) &&
                // ServiceDetailId filter (Type must be 1)
                (string.IsNullOrWhiteSpace(query.ServiceDetailId) || (ad.ItemDetailId != null && ad.ItemDetailId.Contains(query.ServiceDetailId) && ad.Type == 1)) &&
                // Order filters (Type must be 3)
                (orderIdList == null || (ad.ItemDetailId != null && orderIdList.Contains(ad.ItemDetailId) && ad.Type == 3));

            var selector = ObjectMapperExtensions.CreateMapExpression<Review, ReviewResponse>();

            var orderByFunc = GetOrderByFunc(query.SortColumn, query.SortDirection);

            var totalItems = await _unitOfWork.GetRepository<Review>().CountAsync(predicate);

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
