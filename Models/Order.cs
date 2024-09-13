namespace ShowPass.Models
{
    public class Order
    {
        public Guid Id { get; init; }
        public Type Type { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        public Status Status { get; private set; }

        public Order(Guid userId, Guid eventId, int quantity, Type type)
        {
            Id = Guid.NewGuid();
            EventId = eventId;
            Quantity = quantity;
            Type = type;
            Price = type.GetPrice();
            UserId = userId;
            Status = Status.Pending;
        }

        public void ChangeStatus(Status status)
        {
            Status = status;
        }
    }
}