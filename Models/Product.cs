using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ShopOnline.Abstract;

namespace ShopOnline.Models
{
    public class Product : Auditable
    {
    
        public int ID { set; get; }       
        public string Name { set; get; }

        public string Alias { set; get; }

        public int CategoryID { set; get; }

        public string Image { set; get; }

        public string MoreImages { set; get; }

        public decimal Price { set; get; }
        public decimal OriginalPrice { set; get; }
        public decimal? PromotionPrice { set; get; }

        public string Description { set; get; }
        public string Content { set; get; }

        public bool? HomeFlag { set; get; }
        public bool? HotFlag { set; get; }
        public int? ViewCount { set; get; }

        public string Tags { set; get; }

        public int Quantity { set; get; }

        

        public virtual ProductCategory ProductCategory { set; get; }

        public virtual List<ProductTag> ProductTags { set; get; }
    }
}
