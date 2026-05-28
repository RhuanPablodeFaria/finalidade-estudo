using FinalidadeEstudo.Application.Common;
using FinalidadeEstudo.Domain.Projecao;
using MediatR;

namespace FinalidadeEstudo.Application.Users.Queries.GetAllUsers;

public sealed record GetAllUsersQuery(PaginatedGridConfiguration configGrid) : IRequest<Result<ProjectionResponse>>;
