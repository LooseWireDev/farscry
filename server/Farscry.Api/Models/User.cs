namespace Farscry.Api.Models;

public class User
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public string? Name { get; set; }
    public required string FriendId { get; set; }
    public string? ProfileImageUrl { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public Family? Family { get; set; }
    public Guid? FamilyId { get; set; }
    public ICollection<Call>? InitiatedCalls { get; set; }
    public ICollection<Call>? ReceivedCalls { get; set; }
    public ICollection<UserContact>? Contacts { get; set; }
    public string? PolarCustomerId { get; set; }
    public string? PolarSubscriptionId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
