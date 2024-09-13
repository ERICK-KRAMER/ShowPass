using ShowPass.Models;
using ShowPass.Models.Events;

namespace ShowPass.Repositories.Interfaces
{
    public interface IEventRepository
    {
        public Task<IEnumerable<Event>> GetAll();
        public Task<Event> Get(Guid id);
        public Task<bool> Save(EventDTO request);
        public Task<bool> Remove(Guid id);
    }
}