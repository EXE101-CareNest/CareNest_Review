namespace CareNest_Review.Infrastructure.ApiEndpoints
{
    public class CustomerEndpoints
    {
        public static string GetById(string id) => $"/api/admin/accounts/{id}";
    }
}
