using FinalidadeEstudo.Application.Common;
using MediatR;

namespace FinalidadeEstudo.Application.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(
        Guid Id,
        DateTime? DateBirth,
        string? Name,
        string? Email) : IRequest<Result<string>>;
