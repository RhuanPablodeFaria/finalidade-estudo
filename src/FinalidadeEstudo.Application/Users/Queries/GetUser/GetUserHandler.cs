using FinalidadeEstudo.Application.Common;
using FinalidadeEstudo.Domain.Enums;
using FinalidadeEstudo.Domain.Interfaces.Queries;
using MediatR;

namespace FinalidadeEstudo.Application.Users.Queries.GetUser;

public sealed class GetUserHandler(IUserQuery userQuery)
    : IRequestHandler<GetUserQuery, Result<GetUserResponse>>
{
    public async Task<Result<GetUserResponse>> Handle(
        GetUserQuery getUserQuery,
        CancellationToken ct)
    {
        var user = await userQuery.GetAsync(x => x.Id == getUserQuery.Id, ct);

        if (user is null)
            return Result<GetUserResponse>.Failure($"User with ID: {getUserQuery.Id}, not found.", EnumTypeResult.BadRequest);

        var response = new GetUserResponse(
            user.Id,
            user.Name,
            user.CpfCnpj.ToString(),
            user.Email.Value,
            user.DateBirth,
            user.IsActive);

        return Result<GetUserResponse>.Success(response);
    }
}
