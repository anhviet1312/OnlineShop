namespace ShopOnline.Models
{
    public class Cart
    {
        public List<CartItem>? ListItems { get; set; } = new List<CartItem>();

        public string Name { get; set; }    
        public string? Message { get; set; }  
        
        public string Address { get; set; }
        public string Phone { get; set; }       
    }
}
