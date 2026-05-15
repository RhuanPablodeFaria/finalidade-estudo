using FinalidadeEstudo.Application.Common;
using MediatR;

namespace FinalidadeEstudo.Application.Users.Queries.GetAllUsers;

public sealed record GetAllUsersQuery() : IRequest<Result<IEnumerable<GetAllUsersResponse>>>;
