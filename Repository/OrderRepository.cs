using ShopOnline.Data;
using ShopOnline.Interface;
using ShopOnline.Models;

namespace ShopOnline.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context) { 
              _context= context;
        }

        public async Task<Order> AddAsync(Order order)
        {
            await _context.AddAsync(order); 
            await _context.SaveChangesAsync();
            return order;
        }

        public List<Order> GetAll()
        {
            return _context.Orders.ToList();
        }

        public List<OrderDetail> GetAllOrderDetails(int id)
        {
            return _context.OrderDetails.Where(x => x.OrderID == id).ToList();
        }

        public Order GetById(int id)
        {
            return _context.Orders.FirstOrDefault(x => x.ID == id);
        }
    }
}
