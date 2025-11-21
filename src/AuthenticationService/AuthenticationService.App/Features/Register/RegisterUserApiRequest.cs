namespace AuthenticationService.App.Features.Register;

public sealed record RegisterUserApiRequest(
    string Surname,
    string Name,
    string? Patronymic,
    string Email,
    string Password,
    string Role);
