namespace FinalidadeEstudo.Application.Users.Queries.GetAllUsers;

public sealed record GetAllUsersResponse(
    Guid Id,
    string Name,
    string Email,
    string CpfCnpj,
    DateTime DateBirth,
    bool IsActive);
