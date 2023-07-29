

namespace ShopOnline.Models
{
    
    public class PostTag
    {
        
        
        public int PostID { set; get; }

        
        
        public string TagID { set; get; }

        
        public virtual Post Post { set; get; }

        public virtual Tag Tag { set; get; }
    }
}
