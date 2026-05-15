namespace FinalidadeEstudo.Application.Users.Queries.GetUser;

public sealed record GetUserResponse(
    Guid Id,
    string Name,
    string Email,
    string CpfCnpj,
    DateTime DateBirth,
    bool IsActive);
