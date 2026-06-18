namespace FinalidadeEstudo.Domain.ValueObject;

using System;
using System.Linq;

public sealed class CpfCnpj
{
    public string Value { get; }

    private CpfCnpj(string value) => Value = value;

    public static CpfCnpj Create(string input)
    {
        var normalized = Normalize(input);

        return new CpfCnpj(normalized);
    }
    public override int GetHashCode() => Value.GetHashCode();
    public override bool Equals(object? obj) =>
        obj is CpfCnpj other && Value == other.Value;

    public static bool IsValid(string value)
    {
        value = Normalize(value);

        return value.Length switch
        {
            11 => IsValidCpf(value),
            14 => IsValidCnpj(value),
            _ => false
        };
    }

    private static string Normalize(string value) =>
        new string(value.Where(char.IsDigit).ToArray());

    private static bool IsValidCpf(string cpf)
    {
        if (cpf.Length != 11)
            return false;

        if (cpf.Distinct().Count() == 1)
            return false;

        var numbers = cpf.Select(c => c - '0').ToArray();

        var sum = 0;
        for (int i = 0; i < 9; i++)
            sum += numbers[i] * (10 - i);

        var remainder = sum % 11;
        var firstDigit = remainder < 2 ? 0 : 11 - remainder;

        if (numbers[9] != firstDigit)
            return false;

        sum = 0;
        for (int i = 0; i < 10; i++)
            sum += numbers[i] * (11 - i);

        remainder = sum % 11;
        var secondDigit = remainder < 2 ? 0 : 11 - remainder;

        return numbers[10] == secondDigit;
    }

    private static bool IsValidCnpj(string cnpj)
    {
        if (cnpj.Length != 14)
            return false;

        if (cnpj.Distinct().Count() == 1)
            return false;

        var numbers = cnpj.Select(c => c - '0').ToArray();

        int[] firstWeights = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] secondWeights = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        var sum = 0;
        for (int i = 0; i < 12; i++)
            sum += numbers[i] * firstWeights[i];

        var remainder = sum % 11;
        var firstDigit = remainder < 2 ? 0 : 11 - remainder;

        if (numbers[12] != firstDigit)
            return false;

        sum = 0;
        for (int i = 0; i < 13; i++)
            sum += numbers[i] * secondWeights[i];

        remainder = sum % 11;
        var secondDigit = remainder < 2 ? 0 : 11 - remainder;

        return numbers[13] == secondDigit;
    }

    public override string ToString()
    {
        return Value.Length switch
        {
            11 => Convert.ToUInt64(Value).ToString(@"000\.000\.000\-00"),
            14 => Convert.ToUInt64(Value).ToString(@"00\.000\.000\/0000\-00"),
            _ => Value
        };
    }
}
