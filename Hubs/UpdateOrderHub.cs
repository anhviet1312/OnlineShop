using Microsoft.AspNetCore.SignalR;
using ShopOnline.Interface;

namespace ShopOnline.Hubs
{
    
    public class UpdateOrderHub: Hub
    {
        private readonly IOrderRepository _orderRepository;
        public UpdateOrderHub(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task UpdateStatusOrder(int id)
        {
            var order = _orderRepository.GetById(id);
            order.Status = !order.Status;
            _orderRepository.Update(order);
            await Clients.All.SendAsync("UpdateStatusOrder", id, order.Status);
        }
    }
}
