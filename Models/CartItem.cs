namespace ShopOnline.Models
{
    public class CartItem
    {
        public int ProductID { set; get; }

        public string ProductName { set; get; }

        public int Quantity { set; get; }

        public decimal Price { set; get; }

        public string Image { set; get; }

        public int CategoryID { set; get; }

        public string CategoryName { set; get; }


    }
}
