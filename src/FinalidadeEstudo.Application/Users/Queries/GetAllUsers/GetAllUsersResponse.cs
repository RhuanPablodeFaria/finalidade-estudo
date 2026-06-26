namespace FinalidadeEstudo.Application.Users.Queries.GetAllUsers;

public sealed class GetAllUsersResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string CpfCnpj { get; set; }
    public DateTime DateBirth { get; set; }
    public bool IsActive { get; set; }
}
