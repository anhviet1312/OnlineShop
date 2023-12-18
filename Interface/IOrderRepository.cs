using ShopOnline.Models;

namespace ShopOnline.Interface
{
    public interface IOrderRepository
    {
        List<Order> GetAll();
        Order GetById(int id);

        Task<Order> AddAsync(Order order);
        List<OrderDetail> GetAllOrderDetails(int id);
        List<Order> GetAllOrderByUserId(string id);

        Order Update(Order order);  
        
    }
}
