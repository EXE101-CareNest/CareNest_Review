using CareNest_Review.Domain.Commons.Base;

namespace CareNest_Review.Domain.Entitites
{
    public class Review : BaseEntity
    {
        public string? CustomerId { get; set; }
        public string? ItemDetailId { get; set; }

        public double Rating { get; set; } // tổng tiền
        public string? Contents { get; set; } // phương thức thanh toán
        public string? ImgUrl { get; set; } // ghi chú
        public int Type { get; set; }
    }
}
