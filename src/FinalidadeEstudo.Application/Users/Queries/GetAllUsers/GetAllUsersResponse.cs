namespace FinalidadeEstudo.Application.Users.Queries.GetAllUsers;

public sealed class GetAllUsersResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string CpfCnpj { get; set; }
    public DateTime DateBirth { get; set; }
    public bool IsActive { get; set; }
}
