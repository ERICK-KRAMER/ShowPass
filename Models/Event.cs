namespace ShowPass.Models
{
    public class Event
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public string Location { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}