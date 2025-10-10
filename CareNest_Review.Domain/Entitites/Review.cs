using CareNest_Review.Domain.Commons.Base;

namespace CareNest_Review.Domain.Entitites
{
    public class Review : BaseEntity
    {
        public string? CustomerId { get; set; }
        public string? ItemDetailId { get; set; }

        public double Rating { get; set; }
        public string? Contents { get; set; }
        public string? ImgUrl { get; set; } // ghi chú
        public int Type { get; set; }
    }
}
