using FinalidadeEstudo.Application.Common;
using FinalidadeEstudo.Domain.Interfaces.Queries;
using MediatR;

namespace FinalidadeEstudo.Application.Users.Queries.GetAllUsers;

public sealed class GetAllUsersHandler(IUserQuery userQuery)
    : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<GetAllUsersResponse>>>
{
    public async Task<Result<IEnumerable<GetAllUsersResponse>>> Handle(
        GetAllUsersQuery query,
        CancellationToken ct)
    {
        var users = await userQuery.GetAllAsync(ct: ct);

        var response = users.Select(u => new GetAllUsersResponse(
            u.Id,
            u.Name,
            u.Email.Value,
            u.CpfCnpj.ToString(),
            u.DateBirth,
            u.IsActive));

        return Result<IEnumerable<GetAllUsersResponse>>.Success(response);
    }
}
