using FinalidadeEstudo.Application.Common;
using FinalidadeEstudo.Domain.Enums;
using FinalidadeEstudo.Domain.Interfaces;
using MediatR;

namespace FinalidadeEstudo.Application.Users.Commands.Deactive;

public sealed class DeactiveUserHandler(IUnitOfWork unitOfWork, IAppLogger logger)
    : IRequestHandler<DeactivateUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(
        DeactivateUserCommand command,
        CancellationToken ct)
    {
        logger.LogInformation("User deactivation initialized.");

        var entityUser = await unitOfWork.Users.GetAsync(x => x.Id == command.Id, ct);

        if (entityUser is null)
        {
            var message = $"User with ID: {command.Id}, not found.";
            logger.LogWarning(message);
            return Result<string>.Failure(message, EnumTypeResult.BadRequest);
        }

        if (!entityUser.IsActive)
        {
            var menssage = "User is already deactivated.";
            logger.LogInformation(menssage);
            return Result<string>.Success(menssage);
        }

        entityUser.Deactivate();
        await unitOfWork.CommitAsync(ct);

        var successMessage = "User successfully deactivated.";

        logger.LogInformation(successMessage);
        return Result<string>.Success(successMessage);
    }
}
