namespace ShowPass.Models
{
    public record UserDTO(Guid Id, string Name, string Email, List<TicketDTO> Tickets);
}