using FinalidadeEstudo.Application.Common;
using MediatR;

namespace FinalidadeEstudo.Application.Users.Commands.CreateUser;

public sealed record CreateUserCommand(
    string Name,
    string Email,
    string Password,
    string CpfCnpj,
    DateTime DateBirth) : IRequest<Result<CreateUserResponse>>;
