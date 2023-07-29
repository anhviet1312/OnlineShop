using System.ComponentModel.DataAnnotations;

namespace ShopOnline.Models
{
    public class VisitorStatistics
    {
        
        public Guid ID { set; get; }

        
        public DateTime VisitedDate { set; get; }

        
        public string IPAddress { set; get; }
    }
}
