using CareNest_Review.Application.Common;

namespace CareNest_Review.Application.DTOs
{
    public class AppointmentDetailResponse
    {
        public List<AppointmentDetailDto> Items { get; set; } = new List<AppointmentDetailDto>();
        public int TotalItems { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}