namespace ShopOnline.Models.ViewModels
{
    public class ViewProductCategoryModel
    {
        public List<ProductCategory> ? ProductCategories { get; set; } = new List<ProductCategory> { };
        public CreateProductCategoryDto CreateOrUpdate { get; set; }
    }
}
