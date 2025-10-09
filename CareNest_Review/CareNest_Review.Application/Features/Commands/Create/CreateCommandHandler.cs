using CareNest_Review.Application.DTOs;
using CareNest_Review.Application.Features.Queries.GetAllPaging;
using CareNest_Review.Application.Interfaces.CQRS.Commands;
using CareNest_Review.Application.Interfaces.Services;
using CareNest_Review.Application.Interfaces.UOW;
using CareNest_Review.Domain.Entitites;
using Microsoft.AspNetCore.Http;
using Shared.Helper;

namespace CareNest_Review.Application.Features.Commands.Create
{
    public class CreateCommandHandler : ICommandHandler<CreateCommand, ReviewResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceDetailService _serviceDetailService;
        private readonly IProductDetailService _productDetailService;
        private readonly ICustomerService _customerService;

        public CreateCommandHandler(IUnitOfWork unitOfWork, IServiceDetailService serviceDetailService, IProductDetailService productDetailService, ICustomerService customerService)
        {
            _unitOfWork = unitOfWork;
            _serviceDetailService = serviceDetailService;
            _productDetailService = productDetailService;
            _customerService = customerService;
        }

        public async Task<ReviewResponse> HandleAsync(CreateCommand command)
        {
            var product = new ProductDetailDTO();
            var service = new ServiceDetailDTO();
            if(command.CustomerId != null)
            {
                var customer = await _customerService.GetbyId(command.CustomerId);
            }
            if (command.Type == 1)
            {
                service = await _serviceDetailService.GetById(command.ItemDetailId!);
            } else if(command.Type == 2)
            {
                product = await _productDetailService.GetbyId(command.ItemDetailId!);
            }
            else
            {
                throw new BadHttpRequestException("Type is invalid. Please select 1: service or 2:product");
            }

                Review review = new()
                {
                    ImgUrl = command.ImgUrl,
                    Type = command.Type,
                    Rating = command.Rating,
                    Contents = command.Contents,
                    ItemDetailId = command.Type == 2 ? product.Id : service.Id,
                    CreatedAt = TimeHelper.GetUtcNow(),
                };
            await _unitOfWork.GetRepository<Review>().AddAsync(review);
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
