namespace Farscry.Api.Models;

public class Family
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required User Owner { get; set; }
    public Guid OwnerId { get; set; }
    public ICollection<User>? Users { get; set; }
    public string? PolarSubscriptionId { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
}
