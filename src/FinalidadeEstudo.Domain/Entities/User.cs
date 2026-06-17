using FinalidadeEstudo.Domain.ValueObject;

namespace FinalidadeEstudo.Domain.Entities;

public sealed class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public string Password { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; }
    public CpfCnpj CpfCnpj { get; private set; }
    public DateTime DateBirth { get; private set; }

    private User(string name, Email email, string password, CpfCnpj cpfCnpj, DateTime dateBirth)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        Password = password;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
        CpfCnpj = cpfCnpj;
        DateBirth = dateBirth;
    }

    public static User Create(string name, Email email, string password, CpfCnpj cpfCnpj, DateTime dateBirth) =>
        new(name, email, password, cpfCnpj, dateBirth);

    public void Update(string? name, string? email, DateTime? dateBirth)
    {
        if (name is not null)
            Name = name;

        if (email is not null && Email.IsValid(email))
            Email = Email.Create(email);

        if (dateBirth.HasValue)
            DateBirth = dateBirth.Value;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
