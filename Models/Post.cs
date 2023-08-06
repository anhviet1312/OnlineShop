
using ShopOnline.Abstract;


namespace ShopOnline.Models
{
   
    public class Post : Auditable
    {
        
        public int ID { set; get; }

        
        public string Name { set; get; }

        
        public string Alias { set; get; }

        public int CategoryID { set; get; }

        public string? Image { set; get; }

        public string Description { set; get; }

        public string? Content { set; get; }

        public bool? HomeFlag { set; get; }
        public bool? HotFlag { set; get; }
        public int? ViewCount { set; get; }

        
        public virtual PostCategory PostCategory { set; get; }

        public virtual List<PostTag> PostTags { set; get; }
    }
}
