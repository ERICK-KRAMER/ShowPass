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
        public async Task<ActionResult> Post(Guid eventId)
        {
            // Criação de usuário
            User user = new()
            {
                Id = Guid.NewGuid(),
                Name = "ERICK",
                Email = "ERICKKRAMER@OUTLOOK.COM",
                Password = "123456",
                Tickets = new List<Ticket>(),
            };

            var userAlreadyExist = await _context.Users.FirstOrDefaultAsync(x => x.Name == user.Name);

            if (userAlreadyExist != null)
            {
                return BadRequest("User already exists.");
            }

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Criação de pedido

            var findEvent = await _context.Events.FirstOrDefaultAsync(
                x => x.Id == eventId
            );

            if (findEvent == null)
                return BadRequest();

            Order order = new()
            {
                Id = Guid.NewGuid(),
                EventId = eventId,
                Quantity = 1,
                Type = 0,
                UserId = user.Id,
                Event = findEvent
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            //Criaçao do Ticket
            Ticket ticket = new()
            {
                Id = Guid.NewGuid(),
                EventId = eventId,
                Event = findEvent,
                User = user,
                UserId = user.Id
            };

            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();

            return Ok("Order created!");
        }
    }
}
