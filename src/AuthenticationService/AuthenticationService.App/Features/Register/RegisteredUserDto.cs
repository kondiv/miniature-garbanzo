namespace AuthenticationService.App.Features.Register;

public readonly record struct RegisteredUserDto(
    Guid Id,
    string Email,
    DateTime RegisteredAtUtc);
