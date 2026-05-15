using FinalidadeEstudo.Application.Common;
using MediatR;

namespace FinalidadeEstudo.Application.Users.Commands.Deactive;

public sealed record DeactivateUserCommand(
    Guid Id) : IRequest<Result<string>>;
