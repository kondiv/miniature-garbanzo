using AuthenticationService.App.Features.Register;
using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace AuthenticationService.App.Controllers;

[ApiController]
[Route("api/auth/")]
public sealed class AuthenticationController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<RegisteredUserDto>> RegisterAsync(
        RegisterUserApiRequest request, CancellationToken cancellationToken = default)
    {
        var command = new RegisterUserCommand(request.Surname, request.Name, request.Patronymic, request.Email,
            request.Password, request.Role);

        var registrationResult = await mediator.Send(command, cancellationToken);

        if (registrationResult.IsSuccess)
        {
            return Ok(registrationResult.Value);
        }

        return registrationResult.Status switch
        {
            ResultStatus.Conflict => Problem(
                type: "Conflict",
                title: "Registration Failure",
                detail: string.Join(',', registrationResult.Errors),
                statusCode: StatusCodes.Status409Conflict
                ),
            _ => Problem(
                type: "Bad Request",
                title: "Registration Failure",
                detail: string.Join(',', registrationResult.Errors),
                statusCode: StatusCodes.Status400BadRequest)
        };
    }
}
