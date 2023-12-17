using ShopOnline.Models.CreateModels;

namespace ShopOnline.Models.ViewModels
{
    public class ViewCustomerProductModel
    {
        public List<Product> Products { get; set;} = new List<Product>();

        public List<Product> Top6NewestProducts { get; set; } = new List<Product>();

        public List<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
        public int ? TotalPages { get; set; }
        public int ? PageNumber { get; set; }

        public int ? CategoryId { get; set; }

        public decimal? MinPrice { get; set; } = 0;

        public decimal? MaxPrice { get; set; } = 2000;

        public string? SearchKey { get; set; }

        public string? OrderBy { get; set; }    

    }
}
