namespace ShowPass.Models
{
    public class Ticket
    {
        public Guid Id { get; init; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Ticket(Guid eventId, Guid userId)
        {
            Id = Guid.NewGuid();
            EventId = eventId;
            UserId = userId;
        }
    }
}