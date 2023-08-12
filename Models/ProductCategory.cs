using ShopOnline.Abstract;

namespace ShopOnline.Models
{
    
    public class ProductCategory : Auditable
    {
        public int ID { set; get; }

        
        public string Name { set; get; }

        
        public string Alias { set; get; }

        public string Description { set; get; }
        public int? ParentID { set; get; }
        public int? DisplayOrder { set; get; }

        public string? Image { set; get; }

        public bool? HomeFlag { set; get; }

        public virtual List<Product> Products { set; get; }
    }
}
