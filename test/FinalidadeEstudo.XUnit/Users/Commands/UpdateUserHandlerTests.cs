using FinalidadeEstudo.Application.Users.Commands.UpdateUser;
using FinalidadeEstudo.Domain.Entities;
using FinalidadeEstudo.Domain.Enums;
using FinalidadeEstudo.Domain.Interfaces;
using FinalidadeEstudo.Domain.Interfaces.Repositories;
using FinalidadeEstudo.Domain.ValueObject;
using FluentAssertions;
using Moq;

namespace FinalidadeEstudo.UnitTests.Users.Commands;

public sealed class UpdateUserHandlerTests
{
    private readonly Mock<IUnitOfWork> _uowMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly UpdateUserHandler _handler;

    public UpdateUserHandlerTests()
    {
        _uowMock.Setup(u => u.Users).Returns(_userRepositoryMock.Object);
        _handler = new UpdateUserHandler(_uowMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUserIsUpdated()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new UpdateUserCommand(userId, "João Atualizado", "novo@email.com", DateTime.Parse("10/11/2000"));
        var existingUser = User.Create("João", Email.Create("joao@email.com"), "hash", CpfCnpj.Create("049.915.810-52"), DateTime.Parse("10/11/2000"));

        _userRepositoryMock
            .Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        _uowMock
            .Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be("Update completed successfully.");
    }

    [Fact]
    public async Task Handle_ShouldReturnBadRequest_WhenUserNotFound()
    {
        // Arrange
        var command = new UpdateUserCommand(Guid.NewGuid(), "João", "joao@email.com", DateTime.Now);

        _userRepositoryMock
            .Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(EnumTypeResult.BadRequest);
    }

    [Theory]
    [InlineData("João", "emailinvalido")]
    [InlineData("  ", "joao@email.com")]
    public async Task Handle_ShouldReturnBadRequest_WhenCommandIsInvalid(
        string name, string email)
    {
        // Arrange
        var command = new UpdateUserCommand(Guid.NewGuid(), name, email, DateTime.Now);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(EnumTypeResult.BadRequest);
    }
}