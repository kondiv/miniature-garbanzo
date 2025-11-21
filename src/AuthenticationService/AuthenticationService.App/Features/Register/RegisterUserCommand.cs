using Ardalis.Result;
using MediatR;

namespace AuthenticationService.App.Features.Register;

internal sealed record RegisterUserCommand(
    string Surname,
    string Name,
    string? Patronymic,
    string Email,
    string Password,
    string Role) : IRequest<Result<RegisteredUserDto>>;
