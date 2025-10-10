
using CareNest_Review.Application.Interfaces.CQRS.Commands;

namespace CareNest_Review.Application.Features.Commands.Delete
{
    public class DeleteCommand : ICommand
    {
        public required string Id { get; set; }
    }
}
