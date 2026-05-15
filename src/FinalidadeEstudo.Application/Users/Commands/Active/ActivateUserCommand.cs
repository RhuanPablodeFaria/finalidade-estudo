using FinalidadeEstudo.Application.Common;
using MediatR;

namespace FinalidadeEstudo.Application.Users.Commands.Active;

public sealed record ActivateUserCommand(
    Guid Id) : IRequest<Result<string>>;
