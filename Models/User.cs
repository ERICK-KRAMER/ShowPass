namespace ShowPass.Models
{
    public class User
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    }
}