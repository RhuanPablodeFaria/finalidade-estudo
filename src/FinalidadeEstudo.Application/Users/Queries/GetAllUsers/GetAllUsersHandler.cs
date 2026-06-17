using FinalidadeEstudo.Application.Common;
using FinalidadeEstudo.Application.Extension;
using FinalidadeEstudo.Domain.Interfaces.Queries;
using FinalidadeEstudo.Domain.Projecao;
using MediatR;

namespace FinalidadeEstudo.Application.Users.Queries.GetAllUsers;

public sealed class GetAllUsersHandler(IUserQuery userQuery)
    : IRequestHandler<GetAllUsersQuery, Result<ProjectionResponse>>
{
    public async Task<Result<ProjectionResponse>> Handle(
        GetAllUsersQuery query,
        CancellationToken cancellationToken)
    {
        var configGrid = query.configGrid;
        var users = await userQuery.GetAllAsync();

        var selectedQuery = users.Select(u => new GetAllUsersResponse
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email.Value,
            CpfCnpj = u.CpfCnpj.ToString(),
            DateBirth = u.DateBirth,
            IsActive = u.IsActive
        });

        if (!string.IsNullOrWhiteSpace(configGrid.SearchValue))
            selectedQuery = selectedQuery.Where(u =>
                u.Name.ToLower().Contains(configGrid.SearchValue.ToLower()) ||
                u.Email.ToLower().Contains(configGrid.SearchValue.ToLower()) ||
                u.CpfCnpj.Equals(configGrid.SearchValue));

        selectedQuery = configGrid.OrderColumn switch
        {
            "0" => selectedQuery.SortBankInquiry(configGrid.OrderDir, x => x.Id),
            "1" => selectedQuery.SortBankInquiry(configGrid.OrderDir, x => x.Name),
            "2" => selectedQuery.SortBankInquiry(configGrid.OrderDir, x => x.Email),
            "3" => selectedQuery.SortBankInquiry(configGrid.OrderDir, x => x.CpfCnpj),
            "4" => selectedQuery.SortBankInquiry(configGrid.OrderDir, x => x.DateBirth),
            "5" => selectedQuery.SortBankInquiry(configGrid.OrderDir, x => x.IsActive),
            _ => selectedQuery.SortBankInquiry("asc", x => x.Id)
        };

        var projectionRequest = new ProjectionRequest<GetAllUsersResponse>(selectedQuery!, configGrid.Start, configGrid.OffSet);
        var result = await userQuery.PageAsync(projectionRequest, cancellationToken);
        return Result<ProjectionResponse>.Success(result);
    }
}
