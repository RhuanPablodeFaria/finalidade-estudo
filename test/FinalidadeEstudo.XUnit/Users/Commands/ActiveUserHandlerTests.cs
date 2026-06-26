using FinalidadeEstudo.Application.Users.Commands.Active;
using FinalidadeEstudo.Domain.Entities;
using FinalidadeEstudo.Domain.Enums;
using FinalidadeEstudo.Domain.Interfaces;
using FinalidadeEstudo.Domain.Interfaces.Repositories;
using FinalidadeEstudo.Domain.ValueObject;
using FluentAssertions;
using Moq;

namespace FinalidadeEstudo.XUnit.Users.Commands;

public sealed class ActiveUserHandlerTests
{
    private readonly Mock<IAppLogger> _loggerMock = new();
    private readonly Mock<IUnitOfWork> _uowMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly ActiveUserHandler _handler;

    public ActiveUserHandlerTests()
    {
        _uowMock.Setup(u => u.Users).Returns(_userRepositoryMock.Object);
        _handler = new ActiveUserHandler(_uowMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUserIsActivated()
    {
        // Arrange
        var user = User.Create("João", Email.Create("joao@email.com"), "hash", CpfCnpj.Create("049.915.810-52"), DateTime.Parse("10/11/2000"));
        user.Deactivate();

        _userRepositoryMock
            .Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _uowMock
            .Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(new ActivateUserCommand(Guid.NewGuid()), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be("User successfully activated.");

        _loggerMock.Verify(
            x => x.LogError(It.Is<string>(str => str.Contains("not found"))),
            Times.Never);
        _loggerMock.Verify(
            x => x.LogInformation("User activation initialized.", It.IsAny<object[]>()),
            Times.Once);
        _loggerMock.Verify(
            x => x.LogInformation("User successfully activated.", It.IsAny<object[]>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUserIsAlreadyActive()
    {
        // Arrange
        var user = User.Create("João", Email.Create("joao@email.com"), "hash", CpfCnpj.Create("049.915.810-52"), DateTime.Parse("10/11/2000"));
        var guid = Guid.NewGuid();


        _userRepositoryMock
            .Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(new ActivateUserCommand(guid), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be("User is already activated.");

        _loggerMock.Verify(
            x => x.LogError(It.Is<string>(str => str.Contains("not found"))),
            Times.Never);
        _loggerMock.Verify(
            x => x.LogInformation("User activation initialized.", It.IsAny<object[]>()),
            Times.Once);
        _loggerMock.Verify(
            x => x.LogInformation("User is already activated.", It.IsAny<object[]>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnBadRequest_WhenUserNotFound()
    {
        // Arrange
        _userRepositoryMock
            .Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _handler.Handle(new ActivateUserCommand(Guid.NewGuid()), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(EnumTypeResult.BadRequest);

        _loggerMock.Verify(
            x => x.LogInformation("User activation initialized.", It.IsAny<object[]>()),
            Times.Once);
        _loggerMock.Verify(
            x => x.LogWarning(It.Is<string>(str => str.Contains("not found")), It.IsAny<object[]>()),
            Times.Once);
        _loggerMock.Verify(
            x => x.LogInformation("User successfully activated.", It.IsAny<object[]>()),
            Times.Never);
    }
}