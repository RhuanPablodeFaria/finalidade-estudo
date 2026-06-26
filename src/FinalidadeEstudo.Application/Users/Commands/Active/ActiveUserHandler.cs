using FinalidadeEstudo.Application.Common;
using FinalidadeEstudo.Domain.Enums;
using FinalidadeEstudo.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinalidadeEstudo.Application.Users.Commands.Active;

public sealed class ActiveUserHandler(IUnitOfWork unitOfWork, IAppLogger logger)
    : IRequestHandler<ActivateUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(
        ActivateUserCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("User activation initialized.");

        var entityUser = await unitOfWork.Users.GetAsync(x => x.Id == command.Id, cancellationToken);

        if (entityUser is null)
        {
            var message = $"User with ID: {command.Id}, not found.";
            logger.LogWarning(message);
            return Result<string>.Failure(message, EnumTypeResult.BadRequest);
        }

        if (entityUser.IsActive)
        {
            var message = "User is already activated.";
            logger.LogInformation(message);
            return Result<string>.Success(message);
        }

        entityUser.Activate();

        await unitOfWork.CommitAsync(cancellationToken);

        var sucessMessage = "User successfully activated.";

        logger.LogInformation(sucessMessage);
        return Result<string>.Success(sucessMessage);
    }
}
