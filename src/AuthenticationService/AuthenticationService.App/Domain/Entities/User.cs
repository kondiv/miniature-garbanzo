namespace AuthenticationService.App.Domain.Entities;

internal sealed class User
{
    public required Guid Id { get; init; }

    public required string Surname { get; set; }

    public required string Name { get; set; }

    public string? Patronymic { get; set; }

    public required string Email { get; set; }

    public required string PasswordHash { get; set; }

    public bool EmailConfirmed { get; set; } = false;

    public DateTime CreatedAtUtc { get; init; }

    public DateTime? UpdatedAtUtc { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = [];
}
