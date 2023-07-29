namespace ShopOnline.Abstract
{
    public abstract class Auditable : IAuditable
    {
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get ; set ; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set ; }
    }
}
