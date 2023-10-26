namespace ShopOnline.Models.ViewModels
{
    public class ViewProductDetailModel
    {
        public int ID { get; set; } 
        public string Name { set; get; }

        public string Alias { set; get; }

        public int CategoryID { set; get; }

        public string? Image { set; get; }

        public string? MoreImages { set; get; }

        public decimal Price { set; get; }
        public decimal OriginalPrice { set; get; }
        public decimal? PromotionPrice { set; get; }

        public string Description { set; get; }
        public string? Content { set; get; }

        public bool? HomeFlag { set; get; }
        public bool? HotFlag { set; get; }
        public int? ViewCount { set; get; }

        public string? Tags { set; get; }

        public int Quantity { set; get; }


        public int ChoosenQuantity { set; get; } = 1;  
    }
}
