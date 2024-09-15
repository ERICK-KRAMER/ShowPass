using Microsoft.EntityFrameworkCore;
using ShowPass.Data;
using ShowPass.Models;
using ShowPass.Models.EmailService;
using ShowPass.Repositories.Interfaces;

namespace ShowPass.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ShowPassDbContext _context;
        private readonly IEmailService _emailService;
        public OrderRepository(ShowPassDbContext showPassDbContext, IEmailService emailService)
        {
            _context = showPassDbContext;
            _emailService = emailService;
        }
        public async Task<IEnumerable<Order>> GetAll()
        {
            var orders = await _context.Orders
                .Include(x => x.Event)
                .ToListAsync();

            return orders;
        }

        public async Task<bool> Canceled(Guid id, Status status)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(
                x => x.Id == id
            ) ?? throw new Exception("Not found!");

            order.ChangeStatus(status);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Save(Guid eventId, Guid userId, int quantity, Models.Type type)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return false;

            var findEvent = await _context.Events.FirstOrDefaultAsync(
               x => x.Id == eventId
            );

            if (findEvent == null)
                return false;

            if (findEvent.MaxTicket < quantity)
                return false;

            Order order = new(userId, eventId, quantity, type);

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            Ticket ticket = new(eventId, userId);

            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();

            findEvent.ChageMaxTicket(quantity);
            await _context.SaveChangesAsync();

            var send = new EmailPrompt()
                .GeneratePurchaseConfirmationEmail(user.Name, order.Id.ToString(), order.Price);
            _emailService.SendEmail(user.Email, send.Subject, send.Body);

            return true;

        }

        public async Task<IEnumerable<Order>> GetAllOrderByEvent(Guid eventId)
        {
            var order = await _context.Orders
                .Where(x => x.EventId == eventId)
                .ToListAsync();

            return order;
        }
    }
}