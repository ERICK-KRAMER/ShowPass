using Microsoft.AspNetCore.Mvc;
using ShowPass.Models;
using ShowPass.Repositories.Interfaces;

namespace ShowPass.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _orderRepository.GetAll();
            return Ok(orders);
        }

        [HttpPost("Canceled")]
        public async Task<ActionResult> CancelOrder(Guid id, Status status)
        {
            var order = await _orderRepository.Canceled(id, status);

            if (!order)
                return BadRequest("algo de errado aconteceu, tente novamente!");

            return Ok("Pedido cancelado com sucesso!");

        }

        [HttpPost]
        public async Task<ActionResult> Post(Guid eventId, Guid userId, int quantity, Models.Type type)
        {
            var order = await _orderRepository.Save(eventId, userId, quantity, type);

            if (!order)
                return BadRequest("Algo de errado aconteceu, tente novamente!");

            return Ok("Order Created!");
        }

        [HttpPost("{eventId}")]
        public async Task<ActionResult> GetOrderById(Guid eventId)
        {
            var order = await _orderRepository.GetAllOrderByEvent(eventId);

            if (order == null)
                return NotFound("Order not found");

            return Ok(order);
        }
    }
}
