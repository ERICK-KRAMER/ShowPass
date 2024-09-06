using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShowPass.Data;
using ShowPass.Models;

namespace ShowPass.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ShowPassDbContext _context;
        public OrderController(ShowPassDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.Include(x => x.Event).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Guid eventId, Guid userId, int quantity, Models.Type type)
        {
            // Recupera o usuario
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return BadRequest("User not found!");

            // Recupera o Evento
            var findEvent = await _context.Events.FirstOrDefaultAsync(
                x => x.Id == eventId
            );

            if (findEvent == null)
                return BadRequest("Event not found!");

            // verifica se o envento tem ingressos disponiveis
            if (findEvent.MaxTicket < quantity)
            {
                return BadRequest("Ingressos insuficientes!");
            }

            // Criação de pedido
            Order order = new(userId, eventId, quantity, type);

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            //Criaçao do Ticket
            Ticket ticket = new(eventId, userId);

            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();

            findEvent.ChageMaxTicket(quantity);
            await _context.SaveChangesAsync();

            return Ok("Order created!");
        }
    }
}
