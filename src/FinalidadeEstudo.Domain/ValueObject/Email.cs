namespace FinalidadeEstudo.Domain.ValueObject;

public sealed class Email
{
    public string Value { get; }

    private Email(string value) => Value = value;

    public static bool IsValid(string? email) =>
        !string.IsNullOrWhiteSpace(email) &&
        email.Length <= 150 &&
        email.Contains('@') &&
        email.Contains('.');

    public static Email Create(string email) =>
        new(email.Trim().ToLowerInvariant());

    public override bool Equals(object? obj) =>
        obj is Email other && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
}
