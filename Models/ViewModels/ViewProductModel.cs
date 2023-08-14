using ShopOnline.Models.CreateModels;

namespace ShopOnline.Models.ViewModels
{
    public class ViewProductModel
    {
        public List<Product> Products { get; set;} = new List<Product>();
        public CreateProductDto? CreateOrUpdate { get; set; }

        public int ? TotalPages { get; set; }
        public int ? PageNumber { get; set; }
    }
}
