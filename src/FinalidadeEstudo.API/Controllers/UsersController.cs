using FinalidadeEstudo.API.Controllers.Base;
using FinalidadeEstudo.API.Extensions;
using FinalidadeEstudo.Application.Users.Commands.Active;
using FinalidadeEstudo.Application.Users.Commands.CreateUser;
using FinalidadeEstudo.Application.Users.Commands.Deactive;
using FinalidadeEstudo.Application.Users.Commands.UpdateUser;
using FinalidadeEstudo.Application.Users.Queries.GetAllUsers;
using FinalidadeEstudo.Application.Users.Queries.GetUser;
using FinalidadeEstudo.Domain.Projecao;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinalidadeEstudo.API.Controllers;

public sealed class UsersController(ISender sender) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Create(
    [FromBody] CreateUserCommand command,
    CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);

        return result.ToHttpResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] PaginatedGridConfiguration configGrid,
        CancellationToken cancellationToken)
    {
        var query = new GetAllUsersQuery(configGrid);
        var result = await sender.Send(query, cancellationToken);

        return result.ToHttpResult();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateUserCommand(id, request.Name, request.Email, request.DateBirth);
        var result = await sender.Send(command, cancellationToken);

        return result.ToHttpResult();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(
    [FromRoute] Guid id,
    CancellationToken cancellationToken)
    {
        var query = new GetUserQuery(id);
        var result = await sender.Send(query, cancellationToken);

        return result.ToHttpResult();
    }

    [HttpPatch("deactivate/{id:guid}")]
    public async Task<IActionResult> Deactivate(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeactivateUserCommand(id);
        var result = await sender.Send(command, cancellationToken);

        return result.ToHttpResult();
    }

    [HttpPatch("activate/{id:guid}")]
    public async Task<IActionResult> Activate(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {

        var command = new ActivateUserCommand(id);
        var result = await sender.Send(command, cancellationToken);

        return result.ToHttpResult();
    }
}
