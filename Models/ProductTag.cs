

namespace ShopOnline.Models
{
    public class ProductTag
    {
        public int ProductID { set; get; }

        
        public string TagID { set; get; }

        public virtual Product Product { set; get; }

        public virtual Tag Tag { set; get; }
    }
}
