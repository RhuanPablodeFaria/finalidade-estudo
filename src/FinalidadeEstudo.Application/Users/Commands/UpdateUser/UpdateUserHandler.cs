using FinalidadeEstudo.Application.Common;
using FinalidadeEstudo.Domain.Enums;
using FinalidadeEstudo.Domain.Interfaces;
using FinalidadeEstudo.Domain.ValueObject;
using MediatR;

namespace FinalidadeEstudo.Application.Users.Commands.UpdateUser;

public sealed class UpdateUserHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(
        UpdateUserCommand command,
        CancellationToken ct)
    {
        var result = ValidateCommand(command);

        if (!result.IsSuccess)
            return result;

        var entityUser = await unitOfWork.Users.GetAsync(x => x.Id == command.Id, ct);

        if (entityUser is null)
            return Result<string>.Failure($"User with ID: {command.Id}, not found.", EnumTypeResult.BadRequest);

        entityUser.Update(command.Name, command.Email, command.DateBirth);

        await unitOfWork.Users.UpdateAsync(entityUser, ct);
        await unitOfWork.CommitAsync(ct);

        return Result<string>.Success("Update completed successfully.");
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
