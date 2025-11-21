namespace AuthenticationService.App.Domain.Entities;

internal sealed class Role
{
    public int Id { get; init; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = [];
}
