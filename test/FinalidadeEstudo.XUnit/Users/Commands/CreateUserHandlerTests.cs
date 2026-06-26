using FinalidadeEstudo.Application.Users.Commands.CreateUser;
using FinalidadeEstudo.Domain.Entities;
using FinalidadeEstudo.Domain.Enums;
using FinalidadeEstudo.Domain.Interfaces;
using FinalidadeEstudo.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;

namespace FinalidadeEstudo.XUnit.Users.Commands;

public sealed class CreateUserHandlerTests
{
    private readonly Mock<IAppLogger> _loggerMock = new();
    private readonly Mock<IUnitOfWork> _uowMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly CreateUserHandler _handler;

    public CreateUserHandlerTests()
    {
        _uowMock.Setup(u => u.Users).Returns(_userRepositoryMock.Object);
        _handler = new CreateUserHandler(_uowMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUserIsCreated()
    {
        // Arrange
        var command = new CreateUserCommand(
            "João Silva",
            "joao@email.com",
            "123456",
            "967.674.230-92",
            DateTime.Now);

        _userRepositoryMock
            .Setup(r => r.ExistByCpfCnpjAsync(command.CpfCnpj, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _uowMock
            .Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Email.Should().Be(command.Email.ToLowerInvariant());

        _userRepositoryMock.Verify(
            r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
            Times.Once());

        _uowMock.Verify(
            u => u.CommitAsync(It.IsAny<CancellationToken>()),
            Times.Once());

        _loggerMock.Verify(
            x => x.LogInformation(It.Is<string>(msg => msg.Contains("initialized")), It.IsAny<object[]>()),
            Times.Once());
        _loggerMock.Verify(
            x => x.LogInformation(It.Is<string>(msg => msg.Contains("validated")), It.IsAny<object[]>()),
            Times.Once());
        _loggerMock.Verify(
            x => x.LogInformation(It.Is<string>(msg => msg.Contains("created successfully")), It.IsAny<object[]>()),
            Times.Once());
        _loggerMock.Verify(
            x => x.LogWarning(It.IsAny<string>(), It.IsAny<object[]>()),
            Times.Never());
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCpfCnpjAlreadyExists()
    {
        // Arrange
        var command = new CreateUserCommand(
            "João Silva",
            "joao@email.com",
            "123456",
            "36.398.138/0001-38",
            DateTime.Now);

        _userRepositoryMock
            .Setup(r => r.ExistByCpfCnpjAsync(command.CpfCnpj, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(EnumTypeResult.Conflict);

        _userRepositoryMock.Verify(
            r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
            Times.Never());

        _uowMock.Verify(
            u => u.CommitAsync(It.IsAny<CancellationToken>()),
            Times.Never());

        _loggerMock.Verify(
            x => x.LogInformation(It.Is<string>(msg => msg.Contains("initialized")), It.IsAny<object[]>()),
            Times.Once());
        _loggerMock.Verify(
            x => x.LogWarning(It.Is<string>(msg => msg.Contains("registered")), It.IsAny<object[]>()),
            Times.Once());
        _loggerMock.Verify(
            x => x.LogInformation(It.Is<string>(msg => msg.Contains("created successfully")),It.IsAny<object[]>()),
            Times.Never());
    }

    [Theory]
    [InlineData("", "joao@email.com", "123456", "162.371.560-11", "10/11/2000")]
    [InlineData("João", "", "123456", "30.509.939/0001-47", "10/11/2000")]
    [InlineData("João", "emailinvalido", "123456", "86.817.357/0001-82", "10/11/2000")]
    [InlineData("João", "joao@email.com", "123", "049.915.810-52", "10/11/2000")]
    [InlineData("João", "joao@email.com", "123456", "", "10/11/2000")]
    public async Task Handle_ShouldReturnFailure_WhenCommandIsInvalid(
        string name,
        string email,
        string password,
        string cpfCnpj,
        DateTime dateBirth)
    {
        // Arrange
        var command = new CreateUserCommand(
            name,
            email,
            password,
            cpfCnpj,
            dateBirth);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(EnumTypeResult.BadRequest);

        _userRepositoryMock.Verify(
            r => r.AddAsync( It.IsAny<User>(), It.IsAny<CancellationToken>()),
            Times.Never());

        _uowMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never());

        _loggerMock.Verify(
            x => x.LogInformation(It.Is<string>(msg => msg.Contains("initialized")),It.IsAny<object[]>()),
            Times.Once());
        _loggerMock.Verify(
            x => x.LogWarning(It.IsAny<string>(),It.IsAny<object[]>()),
            Times.Once());
        _loggerMock.Verify(
            x => x.LogInformation(It.Is<string>(msg => msg.Contains("validated")),It.IsAny<object[]>()),
            Times.Never());
        _loggerMock.Verify(
            x => x.LogInformation(It.Is<string>(msg => msg.Contains("created successfully")), It.IsAny<object[]>()),
            Times.Never());
    }
}