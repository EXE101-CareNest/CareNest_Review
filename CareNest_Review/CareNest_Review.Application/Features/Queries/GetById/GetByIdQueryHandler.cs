using CareNest_Review.Application.Features.Queries.GetAllPaging;
using CareNest_Review.Application.Interfaces.CQRS.Queries;
using CareNest_Review.Application.Interfaces.Services;
using CareNest_Review.Application.Interfaces.UOW;
using CareNest_Review.Domain.Commons.Constant;
using CareNest_Review.Domain.Entitites;

namespace CareNest_Review.Application.Features.Queries.GetById
{
    public class GetByIdQueryHandler : IQueryHandler<GetByIdQuery, ReviewResponse>
    {
        private readonly IUnitOfWork _unitOfWork;


        public GetByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public async Task<ReviewResponse> HandleAsync(GetByIdQuery query)
        {
            Review? review = await _unitOfWork.GetRepository<Review>().GetByIdAsync(query.Id);

            if (review == null)
            {
                throw new Exception(MessageConstant.NotFound);
            }

            var response = new ReviewResponse
            {
                Id = review.Id,
                CustomerId = review.CustomerId,
                Type = review.Type,
                ItemDetailId = review.ItemDetailId,
                Rating = review.Rating,
                Contents = review.Contents,
                ImgUrl = review.ImgUrl
            };

            return response;
        }
    }
}
