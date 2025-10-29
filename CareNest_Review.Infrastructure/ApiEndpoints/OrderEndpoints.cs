namespace CareNest_Review.Infrastructure.ApiEndpoints
{
    public class OrderEndpoints
    {
        public static string GetById(string id) => $"/api/Order/{id}";
    }
}


