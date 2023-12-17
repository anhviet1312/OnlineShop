using ShopOnline.Data;
using ShopOnline.Interface;
using ShopOnline.Models;

namespace ShopOnline.Repository
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<OrderDetail> AddAsync(OrderDetail orderDetail)
        {
            await _context.OrderDetails.AddAsync(orderDetail);
            await _context.SaveChangesAsync();
            return orderDetail;
        }

        public OrderDetail Delete(OrderDetail orderDetail)
        {
             _context.OrderDetails.Remove(orderDetail);
            _context.SaveChanges();
            return orderDetail;
        }

        public OrderDetail Update(OrderDetail orderDetail)
        {
            _context.OrderDetails.Update(orderDetail);
            _context.SaveChanges();
            return orderDetail;
        }

        public List<OrderDetail> GetOrderDetalsByProductId(int id)
        {
            return _context.OrderDetails.Where(x => x.ProductID == id).ToList();
        }
    }
}
