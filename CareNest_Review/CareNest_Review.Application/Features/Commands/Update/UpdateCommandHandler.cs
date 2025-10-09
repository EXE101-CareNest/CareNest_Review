using CareNest_Review.Application.Exceptions;
using CareNest_Review.Application.Features.Queries.GetAllPaging;
using CareNest_Review.Application.Interfaces.CQRS.Commands;
using CareNest_Review.Application.Interfaces.Services;
using CareNest_Review.Application.Interfaces.UOW;
using CareNest_Review.Domain.Commons.Constant;
using CareNest_Review.Domain.Entitites;
using Microsoft.AspNetCore.Http;
using Shared.Helper;

namespace CareNest_Review.Application.Features.Commands.Update
{
    public class UpdateCommandHandler : ICommandHandler<UpdateCommand, ReviewResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public async Task<ReviewResponse> HandleAsync(UpdateCommand command)
        {
            // Tìm để cập nhật
            Review? review = await _unitOfWork.GetRepository<Review>().GetByIdAsync(command.Id)
               ?? throw new BadRequestException("Id: " + MessageConstant.NotFound);
            
            review.UpdatedAt = TimeHelper.GetUtcNow();
            review.Contents = command.Contents ?? review.Contents;
            review.ImgUrl = command.ImgUrl ?? review.ImgUrl;
            review.Rating = command.Rating;

            _unitOfWork.GetRepository<Review>().Update(review);
            await _unitOfWork.SaveAsync();

            return new ReviewResponse
            {
                Id = review.Id,
                CustomerId = review.CustomerId,
                Contents = review.Contents,
                ImgUrl = review.ImgUrl,
                ItemDetailId = review.ItemDetailId,
                Rating = review.Rating,
                Type = review.Type
            };

        }
    }
}
