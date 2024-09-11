using ShowPass.Services;

namespace ShowPass.Models
{
    public class User
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<Ticket> Tickets { get; set; }

        public User(string name, string email, string password)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            Password = PasswordHash(password);
            Tickets = new List<Ticket>();
        }

        public string PasswordHash(string password)
        {
            PasswordHashService hash = new PasswordHashService();
            return hash.Hash(password);
        }
    }
}