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

    }

    public enum Type
    {
        Camarote,
        Pista
    }
}