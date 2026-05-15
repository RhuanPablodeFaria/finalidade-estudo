namespace FinalidadeEstudo.Application.Users.Commands.UpdateUser;

public sealed record UpdateUserRequest(
        DateTime? DateBirth,
        string? Name,
        string? Email);
