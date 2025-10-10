using CareNest_Review.Application.Exceptions;
using CareNest_Review.Application.Interfaces.CQRS.Commands;
using CareNest_Review.Application.Interfaces.UOW;
using CareNest_Review.Domain.Commons.Constant;
using CareNest_Review.Domain.Entitites;

namespace CareNest_Review.Application.Features.Commands.Delete
{
    public class DeleteCommandHandler : ICommandHandler<DeleteCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(DeleteCommand command)
        {
            // Lấy review theo ID
            Review? review = await _unitOfWork.GetRepository<Review>().GetByIdAsync(command.Id)
                                              ?? throw new BadRequestException("Id: " + MessageConstant.NotFound);

            _unitOfWork.GetRepository<Review>().Delete(review);

            await _unitOfWork.SaveAsync();
        }
    }
}
