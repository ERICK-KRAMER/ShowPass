namespace ShowPass.Models
{
    public class Order
    {
        public Guid Id { get; init; }
        public Type Type { get; set; }
        public decimal Price => Type.GetPrice();
        public int Quantity { get; set; }
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }

        public Order(Guid userId, Guid eventId, int quantity, Type type, Event Eevent)
        {
            Id = Guid.NewGuid();
            EventId = eventId;
            Quantity = quantity;
            Type = type;
            UserId = userId;
            Event = Eevent;
        }
    }
}