using FinalidadeEstudo.Application.Users.Commands.CreateUser;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace FinalidadeEstudo.IntegrationTests.Users;

public sealed class UsersControllerTests(WebAppFactory factory)
    : IClassFixture<WebAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();


    [Fact]
    public async Task Post_ShouldReturn201_WhenUserIsCreated()
    {
        var command = new CreateUserCommand("João Silva", "joao@email.com", "123456", "049.915.810-52", DateTime.Parse("10/11/2000"));

        var response = await _client.PostAsJsonAsync("/api/v1/users", command);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_ShouldReturn409_WhenCpfCnpjAlreadyExists()
    {
        var command = new CreateUserCommand("João Silva", "duplicado@email.com", "123456", "162.371.560-11", DateTime.Parse("10/11/2000"));
        await _client.PostAsJsonAsync("/api/v1/users", command);

        var response = await _client.PostAsJsonAsync("/api/v1/users", command);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Post_ShouldReturn400_WhenCommandIsInvalid()
    {
        var command = new CreateUserCommand("", "", "123", "", DateTime.Parse("10/11/2000"));

        var response = await _client.PostAsJsonAsync("/api/v1/users", command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


    [Fact]
    public async Task GetAll_ShouldReturn200_WithListOfUsers()
    {
        var url = "/api/v1/users?OrderColumn=Name&OrderDir=asc&Start=0&OffSet=20&";

        var response = await _client.GetAsync(url);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_ShouldReturn200_WhenUserExists()
    {
        var command = new CreateUserCommand("Maria Silva", "maria@email.com", "123456", "945.366.340-96", DateTime.Parse("10/11/2000"));
        var created = await _client.PostAsJsonAsync("/api/v1/users", command);
        var user = await created.Content.ReadFromJsonAsync<CreateUserResponse>();

        var response = await _client.GetAsync($"/api/v1/users/{user!.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_ShouldReturn400_WhenUserNotFound()
    {
        var response = await _client.GetAsync($"/api/v1/users/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Put_ShouldReturn200_WhenUserIsUpdated()
    {
        var command = new CreateUserCommand("Carlos Silva", "carlos@email.com", "123456", "50.259.130/0001-11", DateTime.Parse("10/11/2000"));
        var created = await _client.PostAsJsonAsync("/api/v1/users", command);
        var user = await created.Content.ReadFromJsonAsync<CreateUserResponse>();

        var updateRequest = new { Name = "Carlos Atualizado", Email = "carlos.novo@email.com" };
        var response = await _client.PutAsJsonAsync($"/api/v1/users/{user!.Id}", updateRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Patch_ShouldReturn200_WhenUserIsDeactivated()
    {
        var command = new CreateUserCommand("Ana Silva", "ana@email.com", "123456", "14.241.256/0001-03", DateTime.Parse("10/11/2000"));
        var created = await _client.PostAsJsonAsync("/api/v1/users", command);
        var user = await created.Content.ReadFromJsonAsync<CreateUserResponse>();

        var response = await _client.PatchAsync($"/api/v1/users/deactivate/{user!.Id}", null);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Patch_ShouldReturn200_WhenUserIsActivated()
    {
        var command = new CreateUserCommand("Pedro Silva", "pedro@email.com", "123456", "966.029.300-36", DateTime.Parse("10/11/2000"));
        var created = await _client.PostAsJsonAsync("/api/v1/users", command);
        var user = await created.Content.ReadFromJsonAsync<CreateUserResponse>();

        await _client.PatchAsync($"/api/users/deactivate/{user!.Id}", null);
        var response = await _client.PatchAsync($"/api/v1/users/activate/{user!.Id}", null);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}