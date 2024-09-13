using Microsoft.EntityFrameworkCore;
using ShowPass.Data;
using ShowPass.Models;
using ShowPass.Models.Events;
using ShowPass.Repositories.Interfaces;

namespace ShowPass.Repositories
{
    public class EventRespository : IEventRepository
    {
        private readonly ShowPassDbContext _context;
        public EventRespository(ShowPassDbContext showPassDbContext)
        {
            _context = showPassDbContext;
        }

        public async Task<Event> Get(Guid id)
        {
            var findEvent = await _context.Events
                .Include(x => x.Tickets)
                .FirstOrDefaultAsync(
                x => x.Id == id
            );

            return findEvent;
        }

        public async Task<IEnumerable<Event>> GetAll()
        {
            return await _context.Events.ToListAsync();
        }

        public async Task<bool> Remove(Guid id)
        {
            var findEvent = await _context.Events.FirstOrDefaultAsync(
                x => x.Id == id
            );

            if (findEvent == null)
                return false;

            _context.Events.Remove(findEvent);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Save(EventDTO request)
        {
            var findEvent = await _context.Events.FirstOrDefaultAsync(
                x => x.Name == request.Name
            );

            if (findEvent != null)
                return false;

            await _context.Events.AddAsync(new Event(request.Name, request.Location, request.Image, request.MaxTicket, request.Date));
            await _context.SaveChangesAsync();

            return true;
        }
    }
}