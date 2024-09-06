using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShowPass.Data;
using ShowPass.Models;

namespace ShowPass.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        private readonly ShowPassDbContext _context;
        public EventController(ShowPassDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            return await _context.Events.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent()
        {

            Event event1 = new()
            {
                Id = Guid.NewGuid(),
                Name = "Bruno Mars & Lady Gaga",
                Location = "Recife - Pe, Centro de Convencões",
                Tickets = new List<Ticket>(),
            };

            var eventExist = await _context.Events.FirstOrDefaultAsync(x => x.Name == eventItem.Name);

            if (eventExist != null)
                return BadRequest();

            await _context.Events.AddAsync(event1);
            await _context.SaveChangesAsync();

            return Ok("Created!");
        }
    }
}