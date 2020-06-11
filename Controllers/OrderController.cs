using BuyProductAPI.Entites;
using BuyProductAPI.Service.UserService;
using BuyProductAPI.Services.OrderService;
using BuyProductAPI.Services.ProductService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BuyProductAPI.Services.OrderService.OrderTypes;

namespace BuyProductAPI.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;

        public OrderController(
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            IProductRepository productRepository
            )
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {

            var orders = await _orderRepository.GetOrders();

            return Ok(orders);
        }

        [HttpGet("{orderId}", Name = "GetOrder")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Order>> GetOrder(Guid orderId)
        {
            var orderEntity = await _orderRepository.GetOrder(orderId);

            if (orderEntity == null)
                return NotFound();

            return Ok(orderEntity);
        }

        //api/orders/users/{userid}
        [HttpGet("users/{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrderByUser(Guid userId)
        {

            if (!_userRepository.UserExists(userId))
                return NotFound();

            var orders = await _orderRepository.GetOrderByUser(userId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(orders);
        }

        private async Task<(StatusCodeResult, ICollection<Product>)> ValidateProduct(InputOrderCreating order)
        {
            StatusCodeResult statusCodeResult = null;
            var pIds = new List<Guid>();

            foreach (var item in order.ProductsAddedOrder)
                pIds.Add(item.Id);

            var products = await _productRepository.GetProductsByIds(pIds);

            if (pIds.Count != products.Count)
            {
                ModelState.AddModelError("", "O produto não existe");
                statusCodeResult = StatusCode(404);
            }

            return (statusCodeResult, products);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> CreateOrder([FromBody] InputOrderCreating orderFCD)
        {
            var user = await _userRepository.GetUser(orderFCD.userId ?? orderFCD.userId.Value);
            if (user == null)
                return NotFound();

            var (statusCode, productEntities) = await ValidateProduct(orderFCD);

            if (!ModelState.IsValid)
                return StatusCode(statusCode.StatusCode);

            Product p;
            decimal total = 0;


                /* talves da para melhorar esse trecho, ficou assim devido não saber direito a qual sera a forma de dados
                   que sera enviada pelo cliente-side*/
            var arrayProduct = orderFCD.ProductsAddedOrder.ToArray();
            
            for (int i = 0; i < arrayProduct.Length; i++)
            {
                p = productEntities.First(p => p.Id == arrayProduct[i].Id);

                if (p.Quantity - arrayProduct[i].Quantity >= 0)
                {
                    p.Quantity -= arrayProduct[i].Quantity;
                    total += arrayProduct[i].Quantity * p.Price;

                    arrayProduct[i].price = p.Price;
                }
            }

            var products = arrayProduct.ToList();

            var order = new Order()
            {
                User = user,
                Date = DateTime.Now,
                Total = total
            };

            if (!await _orderRepository.CreateOrder(products, order) || !await _productRepository.Save())
            {
                ModelState.AddModelError("", "Ocorreu um erro ao salvar o Pedido");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetOrder",new {orderId = order.Id}, order);
        }
    }
}
