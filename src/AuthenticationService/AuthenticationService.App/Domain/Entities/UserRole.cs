namespace AuthenticationService.App.Domain.Entities;

internal sealed class UserRole
{
    public Guid UserId { get; init; }

    public User User { get; init; } = null!;

    public int RoleId { get; init; }

    public Role Role { get; init; } = null!;
}
