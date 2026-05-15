using FinalidadeEstudo.Application.Common;
using FinalidadeEstudo.Domain.Enums;
using FinalidadeEstudo.Domain.Interfaces;
using MediatR;

namespace FinalidadeEstudo.Application.Users.Commands.Active;

public sealed class ActiveUserHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<ActivateUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(
        ActivateUserCommand command,
        CancellationToken ct)
    {
        var entityUser = await unitOfWork.Users.GetAsync(x => x.Id == command.Id, ct);

        if (entityUser is null)
            return Result<string>.Failure($"User with ID: {command.Id}, not found.", EnumTypeResult.BadRequest);

        if (entityUser.IsActive)
            return Result<string>.Success("User is already activated.");

        entityUser.Activate();

        await unitOfWork.CommitAsync(ct);

        return Result<string>.Success("User successfully activated.");
    }
}
