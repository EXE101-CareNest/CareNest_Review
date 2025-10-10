
namespace CareNest_Review.Infrastructure.ApiEndpoints
{
    public class ServiceDetailEndpoints
    {
        public static string GetById(string? id) => $"/api/servicedetail/{id}";
    }
}
