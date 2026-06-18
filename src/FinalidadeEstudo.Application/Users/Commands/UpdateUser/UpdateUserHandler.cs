using FinalidadeEstudo.Application.Common;
using FinalidadeEstudo.Domain.Enums;
using FinalidadeEstudo.Domain.Interfaces;
using FinalidadeEstudo.Domain.ValueObject;
using MediatR;

namespace FinalidadeEstudo.Application.Users.Commands.UpdateUser;

public sealed class UpdateUserHandler(IUnitOfWork unitOfWork, IAppLogger logger)
    : IRequestHandler<UpdateUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(
        UpdateUserCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Update of the user initialized.");

        var result = ValidateCommand(command);

        if (!result.IsSuccess)
        {
            logger.LogWarning(result.Error);
            return result;
        }

        logger.LogInformation("User credential validated.");

        var entityUser = await unitOfWork.Users.GetAsync(x => x.Id == command.Id, cancellationToken);

        if (entityUser is null)
        {
            var menssage = $"User with ID: {command.Id}, not found.";
            logger.LogWarning(menssage);
            return Result<string>.Failure(menssage, EnumTypeResult.BadRequest);
        }

        entityUser.Update(command.Name, command.Email, command.DateBirth);

        await unitOfWork.Users.UpdateAsync(entityUser);
        await unitOfWork.CommitAsync(cancellationToken);

        var sucessMessage = "Update completed successfully.";

        logger.LogInformation(sucessMessage);
        return Result<string>.Success(sucessMessage);
    }

    private Result<string> ValidateCommand(UpdateUserCommand command)
    {
        if (command.Email is not null && !Email.IsValid(command.Email))
            return Result<string>.Failure("Invalid Email adress.", EnumTypeResult.BadRequest);

        if (command.Name is not null && string.IsNullOrWhiteSpace(command.Name))
            return Result<string>.Failure("Invalid Name.", EnumTypeResult.BadRequest);

        return Result<string>.Success();
    }
}
