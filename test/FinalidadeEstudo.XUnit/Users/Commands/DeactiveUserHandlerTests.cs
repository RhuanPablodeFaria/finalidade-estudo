using FinalidadeEstudo.Application.Users.Commands.Deactive;
using FinalidadeEstudo.Domain.Entities;
using FinalidadeEstudo.Domain.Enums;
using FinalidadeEstudo.Domain.Interfaces;
using FinalidadeEstudo.Domain.Interfaces.Repositories;
using FinalidadeEstudo.Domain.ValueObject;
using FluentAssertions;
using Moq;

namespace FinalidadeEstudo.UnitTests.Users.Commands;

public sealed class DeactiveUserHandlerTests
{
    private readonly Mock<IUnitOfWork> _uowMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly DeactiveUserHandler _handler;

    public DeactiveUserHandlerTests()
    {
        _uowMock.Setup(u => u.Users).Returns(_userRepositoryMock.Object);
        _handler = new DeactiveUserHandler(_uowMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUserIsDeactivated()
    {
        // Arrange
        var user = User.Create("João", Email.Create("joao@email.com"), "hash", CpfCnpj.Create("049.915.810-52"), DateTime.Parse("10/11/2000"));

        _userRepositoryMock
            .Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _uowMock
            .Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(new DeactivateUserCommand(Guid.NewGuid()), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be("User successfully deactivated.");
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUserIsAlreadyDeactivated()
    {
        // Arrange
        var user = User.Create("João", Email.Create("joao@email.com"), "hash", CpfCnpj.Create("049.915.810-52"), DateTime.Parse("10/11/2000"));
        user.Deactivate();

        _userRepositoryMock
            .Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(new DeactivateUserCommand(Guid.NewGuid()), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be("User is already deactivated.");
    }

    [Fact]
    public async Task Handle_ShouldReturnBadRequest_WhenUserNotFound()
    {
        // Arrange
        _userRepositoryMock
            .Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _handler.Handle(new DeactivateUserCommand(Guid.NewGuid()), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(EnumTypeResult.BadRequest);
    }
}

