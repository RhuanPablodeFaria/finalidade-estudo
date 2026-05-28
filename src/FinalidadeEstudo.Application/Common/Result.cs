using FinalidadeEstudo.Domain.Enums;

namespace FinalidadeEstudo.Application.Common;

public class Result
{
    public bool IsSuccess { get; }
    public EnumTypeResult StatusCode { get; }
    public string Error { get; }

    public Result(bool isSuccess,
                  EnumTypeResult statusCode,
                  string erro)
    {
        this.StatusCode = statusCode;
        this.IsSuccess = isSuccess;
        this.Error = erro;
    }

    public static Result Success() =>
        new(true, EnumTypeResult.Ok, string.Empty);

    public static Result Failure(string error, EnumTypeResult statusCode) =>
        new(false, statusCode, error);
}

public sealed class Result<T> : Result
{
    public T? Data { get; }
    public Result(bool isSuccess,
                  EnumTypeResult statusCode,
                  string erro,
                  T? data)
        : base(isSuccess, statusCode, erro)
    {
        this.Data = data;
    }

    public static Result<T> Success(T? data = default) =>
        new(true, EnumTypeResult.Ok, string.Empty, data);

    public static Result<T> Created(T? data = default) =>
        new(true, EnumTypeResult.Created, string.Empty, data);

    public static Result<T> Failure(string error, EnumTypeResult statusCode, T? data = default) =>
        new(false, statusCode, error, data);
}

