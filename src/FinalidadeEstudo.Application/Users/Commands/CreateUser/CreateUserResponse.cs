namespace FinalidadeEstudo.Application.Users.Commands.CreateUser;

public sealed record CreateUserResponse(
    Guid Id,
    string Name,
    string Email,
    DateTime CreatedAt,
    bool IsActive,
    string CpfCnpj,
    DateTime DataNascimento);
