using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShowPass.Data;
using ShowPass.Models;
using ShowPass.Models.Events;

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
        public async Task<ActionResult<Event>> PostEvent(EventDTO request)
        {
            var eventExist = await _context.Events.FirstOrDefaultAsync(x => x.Name == request.Name);

            if (eventExist != null)
                return BadRequest();

            Event event1 = new(request.Name, request.Location, request.Image, request.MaxTicket, request.Date);

            await _context.Events.AddAsync(event1);
            await _context.SaveChangesAsync();

            return Ok("Created!");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEvent(Guid id)
        {
            //recupera o Evento
            var event1 = await _context.Events.FindAsync(id);

            if (event1 == null)
                return BadRequest("Event not found!");

            //deleta o Evento
            _context.Events.Remove(event1);
            await _context.SaveChangesAsync();

            return Ok("Removed");
        }
    }
}