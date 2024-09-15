using ShowPass.Models;

namespace ShowPass.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        public Task<IEnumerable<Order>> GetAll();
        public Task<bool> Save(Guid eventId, Guid userId, int quantity, Models.Type type);
        public Task<bool> Canceled(Guid id, Status status);
        public Task<IEnumerable<Order>> GetAllOrderByEvent(Guid eventId);
    }
}