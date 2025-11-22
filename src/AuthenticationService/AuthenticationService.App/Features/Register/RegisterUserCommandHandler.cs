using Ardalis.Result;
using AuthenticationService.App.Common.Security;
using AuthenticationService.App.Domain.Entities;
using AuthenticationService.App.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace AuthenticationService.App.Features.Register;

internal sealed class RegisterUserCommandHandler(
    ServiceContext context,
    ILogger<RegisterUserCommandHandler> logger,
    IValidator<RegisterUserCommand> validator, 
    IPasswordHasher passwordHasher)
    : IRequestHandler<RegisterUserCommand, Result<RegisteredUserDto>>
{
    public async Task<Result<RegisteredUserDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        using var _ = logger.BeginScope("Registering new user. Email {email}", request.Email);

        validator.ValidateAndThrow(request);

        bool isEmailUnique = !await context
            .Users
            .AnyAsync(u => EF.Functions.ILike(u.Email, request.Email), cancellationToken);

        if(!isEmailUnique)
        {
            logger.LogError("User with the same email already exists");
            return Result<RegisteredUserDto>.Conflict("Email already taken");
        }

        var role = await context
            .Roles
            .FirstOrDefaultAsync(r => EF.Functions.ILike(r.Name, request.Role), cancellationToken);

        if(role is null)
        {
            logger.LogError("Cannot find role with name {providedName}", request.Role);
            return Result<RegisteredUserDto>.NotFound("Role not found");
        }

        if(role.Name.Equals("admin", StringComparison.OrdinalIgnoreCase))
        {
            logger.LogError("Trying to register admin without permissions");
            return Result<RegisteredUserDto>.Forbidden("Cannot register an admin without permissions");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            Surname = request.Surname,
            Name = request.Name,
            Patronymic = request.Patronymic,
            PasswordHash = passwordHasher.HashPassword(request.Password),
        };

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await context
                .Users
                .AddAsync(user, cancellationToken);

            await context
                .UserRoles
                .AddAsync(new UserRole
                {
                    Role = role,
                    User = user
                }, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            var userDto = new RegisteredUserDto(user.Id, user.Email, DateTime.UtcNow);
            
            logger.LogInformation("User has been successfully registered\nUser -- {user}", userDto);
            
            return Result<RegisteredUserDto>.Success(userDto);
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is NpgsqlException { SqlState:PostgresErrorCodes.UniqueViolation })
        {
            await transaction.RollbackAsync(cancellationToken);
            logger.LogError(ex, "User with the same email already exists");
            return Result<RegisteredUserDto>.Conflict("Email already taken");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            logger.LogError(ex, "UnhandledException");
            return Result<RegisteredUserDto>.Error("Unhandled exception");
        }
    }
}
