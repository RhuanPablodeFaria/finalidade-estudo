using FinalidadeEstudo.Application.Common;
using MediatR;

namespace FinalidadeEstudo.Application.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(
        Guid Id,
        string? Name,
        string? Email,
        DateTime? DateBirth) : IRequest<Result<string>>;
