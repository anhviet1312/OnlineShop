using ShopOnline.Models.CreateModels;

namespace ShopOnline.Models.ViewModels
{
    public class ViewTagModel
    {
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public Tag? CreateOrUpdate { get; set; }

        public int? PageNumber { get; set; }
        public int? TotalPages { get; set; }
    }
}
