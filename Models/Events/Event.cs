namespace ShowPass.Models
{
    public class Event
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Location { get; set; }
        public int MaxTicket { get; private set; }
        public DateTime CratedAt { get; init; }
        public DateTime Date { get; init; }
        public ICollection<Ticket> Tickets { get; set; }

        public Event(string name, string location, string image, int maxTicket, DateTime date)
        {
            Id = Guid.NewGuid();
            Name = name;
            Location = location;
            Image = image;
            MaxTicket = maxTicket;
            Tickets = new List<Ticket>();
            CratedAt = DateTime.UtcNow;
            Date = date;
        }

        public void ChageMaxTicket(int quantity)
        {
            MaxTicket -= quantity;
        }

    }
}