using FinalidadeEstudo.Application.Common.Results;
using FinalidadeEstudo.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FinalidadeEstudo.API.Extensions;

public static class HttpResultExtension
{
    public static IActionResult ToHttpResult<T>(this Result<T> result) =>
        BuildResult<T>(result);

    private static IActionResult BuildResult<T>(Result<T> result)
    {
        return result.StatusCode switch
        {
            EnumTypeResult.Ok
                => new OkObjectResult(result.Data),

            EnumTypeResult.Created
                => new CreatedResult(string.Empty, result.Data),

            EnumTypeResult.NoContent
                => new NoContentResult(),

            EnumTypeResult.BadRequest
                => new BadRequestObjectResult(result),

            EnumTypeResult.Unauthorized
                => new UnauthorizedObjectResult(result),

            EnumTypeResult.Forbidden
                => new ObjectResult(result)
                {
                    StatusCode = StatusCodes.Status403Forbidden
                },

            EnumTypeResult.NotFound
                => new NotFoundObjectResult(result),

            EnumTypeResult.RequestTimeout
                => new ObjectResult(result)
                {
                    StatusCode = StatusCodes.Status408RequestTimeout
                },

            EnumTypeResult.Conflict
                => new ConflictObjectResult(result),

            EnumTypeResult.InternalServerError
                => new ObjectResult(result)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                },

            EnumTypeResult.NotImplemented
                => new ObjectResult(result)
                {
                    StatusCode = StatusCodes.Status501NotImplemented
                },

            _ => new ObjectResult(result)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            }
        };
    }
};
