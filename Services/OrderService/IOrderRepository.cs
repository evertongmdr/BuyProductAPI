using BuyProductAPI.Entites;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static BuyProductAPI.Services.OrderService.OrderTypes;

namespace BuyProductAPI.Services.OrderService
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrders();
        Task<Order> GetOrder(Guid orderId);
        Task<IEnumerable<Order>> GetOrderByUser(Guid userId);
        Task<bool> CreateOrder(ICollection<ProductProps> products, Order order);
        Task<bool> Save();

    }
}
