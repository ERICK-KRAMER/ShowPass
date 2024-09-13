using Microsoft.AspNetCore.Mvc;
using ShowPass.Models;
using ShowPass.Models.Events;
using ShowPass.Repositories.Interfaces;

namespace ShowPass.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        public EventController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            var allEvents = await _eventRepository.GetAll();
            return Ok(allEvents);
        }

        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(EventDTO request)
        {
            var save = await _eventRepository.Save(request);

            if (!save)
                return BadRequest("Algo de errado acontece, tente novamente!");

            return Ok("Created!");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEvent(Guid id)
        {
            var removed = await _eventRepository.Remove(id);

            if (!removed)
                return BadRequest("Algo de errado aconteceu, tente novamente!");

            return Ok("Removed");
        }
    }
}