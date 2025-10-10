namespace CareNest_Review.Application.Features.Queries.GetAllPaging
{
    public class ReviewResponse
    {
        public string? Id { get; set; }
        public string? CustomerId { get; set; }
        public string? ItemDetailId { get; set; }

        public double Rating { get; set; }
        public string? Contents { get; set; }
        public string? ImgUrl { get; set; } // ghi chú
        public int Type { get; set; }
    }
}
