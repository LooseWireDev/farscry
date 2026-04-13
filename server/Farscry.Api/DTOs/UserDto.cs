using Farscry.Api.Models;

namespace Farscry.Api.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public string? Name { get; set; }
    public required string FriendId { get; set; }
    public string? ProfileImageUrl { get; set; }
    public required string Email { get; set; }
    public List<string>? ContactNames { get; }
}

public class CreateUserDto
{
    public required string Username { get; set; }
    public string? Name { get; set; }
    public required string FriendId { get; set; }
    public string? ProfileImageUrl { get; set; }
    public required string Email { get; set; }
}

public class UpdateUserDto
{
    public string? Name { get; set; }
    public string? ProfileImageUrl { get; set; }
}

public class UpdateUserNameDto
{
    public string? Name { get; set; }
}
