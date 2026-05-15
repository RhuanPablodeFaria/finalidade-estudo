using FinalidadeEstudo.Application.Common;
using FinalidadeEstudo.Domain.Entities;
using FinalidadeEstudo.Domain.Enums;
using FinalidadeEstudo.Domain.Interfaces;
using FinalidadeEstudo.Domain.ValueObject;
using MediatR;

namespace FinalidadeEstudo.Application.Users.Commands.CreateUser;

public sealed class CreateUserHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateUserCommand, Result<CreateUserResponse>>
{
    public async Task<Result<CreateUserResponse>> Handle(
        CreateUserCommand command,
        CancellationToken ct)
    {
        var result = await ValidateCommand(command, ct);

        if (!result.IsSuccess)
            return result;

        var email = Email.Create(command.Email);
        var cpfCnpj = CpfCnpj.Create(command.CpfCnpj);

        var user = User.Create(command.Name, email, command.Password, cpfCnpj, command.DateBirth);

        await unitOfWork.Users.AddAsync(user, ct);
        await unitOfWork.CommitAsync(ct);

        var response = new CreateUserResponse(
            user.Id,
            user.Name,
            user.Email.Value,
            user.CreatedAt,
            user.IsActive,
            user.CpfCnpj.ToString(),
            user.DateBirth
        );

        return Result<CreateUserResponse>.Success(response);
    }

    private async Task<Result<CreateUserResponse>> ValidateCommand(CreateUserCommand command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            return Result<CreateUserResponse>.Failure("Name is required.", EnumTypeResult.BadRequest);

        if (!Email.IsValid(command.Email))
            return Result<CreateUserResponse>.Failure("Invalid Email adress.", EnumTypeResult.BadRequest);

        if (!CpfCnpj.IsValid(command.CpfCnpj))
            return Result<CreateUserResponse>.Failure("Invalid CPF/CNPJ.", EnumTypeResult.BadRequest);

        if (string.IsNullOrWhiteSpace(command.Password) || command.Password.Length < 6)
            return Result<CreateUserResponse>.Failure("Password must contain at least 6 characters.", EnumTypeResult.BadRequest);

        var exists = await unitOfWork.Users.ExistByCpfCnpjAsync(command.CpfCnpj, ct);
        if (exists)
            return Result<CreateUserResponse>.Failure("Email address and CPF/CNPJ are already registered.", EnumTypeResult.Conflict);

        return Result<CreateUserResponse>.Success();
    }
}
