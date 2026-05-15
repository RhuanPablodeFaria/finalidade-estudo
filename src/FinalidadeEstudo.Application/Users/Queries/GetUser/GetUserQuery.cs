using FinalidadeEstudo.Application.Common;
using MediatR;

namespace FinalidadeEstudo.Application.Users.Queries.GetUser;

public sealed record GetUserQuery(Guid Id) : IRequest<Result<GetUserResponse>>;
