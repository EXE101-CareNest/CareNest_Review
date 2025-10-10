namespace CareNest_Review.Infrastructure.ApiEndpoints
{
    public class ProductDetailEndpoints
    {
        public static string GetById(string id) => $"/api/ProductDetails/{id}";
    }
}
