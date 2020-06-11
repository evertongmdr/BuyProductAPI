using BuyProductAPI.Contexts;
using BuyProductAPI.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BuyProductAPI.Services.OrderService.OrderTypes;

namespace BuyProductAPI.Services.OrderService
{
    public class OrderRepository : IOrderRepository
    {
        private readonly BuyProductContext _orderContext;
        public OrderRepository(BuyProductContext orderContext)
        {
            _orderContext = orderContext;
        }

        public async Task<bool> CreateOrder(ICollection<ProductProps> products, Order order)
        {

            foreach (var product in products)
            {
                var orderProduct = new OrderProduct()
                {
                    ProductId = product.Id,
                    Quantity = product.Quantity,
                    Price = product.price ?? product.price.Value,
                    Order = order
                   
                };

                await _orderContext.AddAsync(orderProduct);
            }

            return await Save();
        }

        public async Task<Order> GetOrder(Guid orderId)
        {
            if(orderId== Guid.Empty)
                throw new ArgumentNullException(nameof(orderId));

            return await _orderContext.Oders.Where(o => o.Id == orderId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Order>> GetOrderByUser(Guid userId)
        {
           return await _orderContext.Oders.Include(o => o.OrderProducts).AsNoTracking().Where(o => o.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
           return await _orderContext.Oders.ToListAsync();
        }

        public async Task<bool> Save()
        {
            var saved = await _orderContext.SaveChangesAsync();
            return saved >= 0 ? true : false;
        }
    }
}
