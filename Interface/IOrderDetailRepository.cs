using ShopOnline.Models;

namespace ShopOnline.Interface
{
    public interface IOrderDetailRepository
    {
        Task<OrderDetail> AddAsync(OrderDetail orderDetail);

        OrderDetail Update(OrderDetail orderDetail);

        OrderDetail Delete(OrderDetail orderDetail);
    }

}
