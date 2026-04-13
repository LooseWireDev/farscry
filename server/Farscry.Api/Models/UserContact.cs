namespace Farscry.Api.Models;

public enum ContactStatus
{
    Pending,
    Accepted,
    Blocked
}

public class UserContact
{
    public Guid Id { get; set; }
    public required User User { get; set; }
    public Guid UserId { get; set; }
    public required User Contact { get; set; }
    public Guid ContactId { get; set; }
    public ContactStatus Status { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
}
