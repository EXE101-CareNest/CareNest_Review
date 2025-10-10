namespace CareNest_Review.Application.Features.Commands.Update
{
    public class UpdateRequest
    {
        public double Rating { get; set; }
        public string? Contents { get; set; }
        public string? ImgUrl { get; set; }
    }
}
