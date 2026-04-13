namespace Farscry.Api.Models;

public enum CallStatus
{
    Missed,
    Answered,
    Declined,
    Ended
}

public class Call
{
    public Guid Id { get; set; }
    public required User Initiator { get; set; }
    public required Guid InitiatorId { get; set; }
    public required User Receiver { get; set; }
    public required Guid ReceiverId { get; set; }
    public CallStatus Status { get; set; }
    public DateTime? EndedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
