
using ShopOnline.Abstract;

namespace ShopOnline.Models
{
    public class Tag : Auditable
    {
        
        public string ID { set; get; }

        
        public string Name { set; get; }

        public string? Type { set; get; }
    }
}
