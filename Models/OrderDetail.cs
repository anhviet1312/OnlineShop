

namespace ShopOnline.Models
{
    
    public class OrderDetail
    {
        public int OrderID { set; get; }

        public int ProductID { set; get; }

        public int Quantity { set; get; }

        public decimal Price { set; get; }

        public virtual Order Order { set; get; }

        
        public virtual Product Product { set; get; }
    }
}
