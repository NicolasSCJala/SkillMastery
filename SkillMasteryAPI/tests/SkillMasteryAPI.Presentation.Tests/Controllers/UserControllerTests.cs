using NSubstitute;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

using Moq;

using SkillMasteryAPI.Application.Services.Interfaces;
using SkillMasteryAPI.Application.DTOs.User;
using SkillMasteryAPI.Presentation.Controllers;

namespace SkillMasteryAPI.Presentation.Tests.Controllers;

public class UsersControllerTests
{
    private readonly IUserService _userService;
    private readonly Mock<IUserService> _mockUserService;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _userService = Substitute.For<IUserService>();
        _mockUserService = new Mock<IUserService>();
        _controller = new UsersController(_mockUserService.Object);
    }
    private UsersController GetControllerInstance() => new(_userService);

    [Fact]
    public async Task GetAllUsers_ShouldReturnOk()
    {
        // Arrange
        _userService.GetAllUsersAsync().Returns(new List<UserDTO>());
        var controller = GetControllerInstance();

        // Act
        var result = await controller.GetAllUsers();

        // Verify that the result is an ActionResult<IEnumerable<UserDTO>>
        result.Should().BeOfType<ActionResult<IEnumerable<UserDTO>>>();

        // Verify that the Result of the ActionResult is an OkObjectResult
        result.Result.Should().BeOfType<OkObjectResult>();

        // Verify the status code
        (result?.Result as OkObjectResult)?.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task CreateUser_Returns201Created_WithValidInput()
    {
        // Arrange
        var createUserDTO = new CreateUserDTO { /* Populate required properties */ };
        var createdUserDTO = new UserDTO { /* Populate with expected result */ };
        _mockUserService.Setup(service => service.CreateUserAsync(createUserDTO))
                           .ReturnsAsync(createdUserDTO);

        // Act
        var result = await _controller.CreateUser(createUserDTO);

        // Assert
        var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(201, actionResult.StatusCode);
        Assert.Equal(createdUserDTO, actionResult.Value);
    }

    [Fact]
    public async Task CreateUser_Returns400BadRequest_WithInvalidModel()
    {
        // Arrange
        _controller.ModelState.AddModelError("Error", "Sample error");

        // Act
        var result = await _controller.CreateUser(new CreateUserDTO());

        // Assert
        var actionResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(400, actionResult.StatusCode);
    }

    [Fact]
    public async Task CreateUser_HandlesException_WithInternalServerError()
    {
        // Arrange
        var createUserDTO = new CreateUserDTO { /* Populate required properties */ };
        _mockUserService.Setup(service => service.CreateUserAsync(createUserDTO))
                           .ThrowsAsync(new System.Exception("Test exception"));

        // Act
        var result = await _controller.CreateUser(createUserDTO);

        // Assert
        var actionResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, actionResult.StatusCode);
        Assert.Contains("Internal server error", actionResult?.Value?.ToString());
    }

    [Fact]
    public async Task DeleteUser_ShouldReturnOk()
    {
        // Arrange
        UserDTO sampleUser = new UserDTO
        {
            Id = 1,
            FirstName = "Ramdom Nombre",
            LastName = "Random Last name",
            Email = "Random email",
        };

        _userService.DeleteUserAsync(1).Returns(Task.FromResult<UserDTO?>(sampleUser));
        var controller = GetControllerInstance();

        // Act
        var result = await controller.DeleteUser(1);

        // Assert
        result.Should().BeOfType<ActionResult<UserDTO>>();

        result.Result.Should().BeOfType<OkObjectResult>();

        (result?.Result as OkObjectResult)?.StatusCode.Should().Be(200);

        var okResult = result?.Result as OkObjectResult;
        var userData = okResult?.Value as UserDTO;
        userData.Should().BeEquivalentTo(sampleUser);
    }


    [Fact]
    public async Task EditUser_ShouldReturnOk()
    {
        // Arrange
        UserDTO sampleUser = new UserDTO
        {
            Id = 1,
            FirstName = "Ramdom Nombre",
            LastName = "Random Last name",
            Email = "Random email",
        };

        _userService.EditUserAsync(sampleUser).Returns(Task.FromResult<UserDTO?>(sampleUser));
        var controller = GetControllerInstance();

        // Act
        var result = await controller.EditUser(sampleUser);

        // Assert
        result.Should().BeOfType<ActionResult<UserDTO>>();

        result.Result.Should().BeOfType<OkObjectResult>();

        (result?.Result as OkObjectResult)?.StatusCode.Should().Be(200);

        var okResult = result?.Result as OkObjectResult;
        var userData = okResult?.Value as UserDTO;
        userData.Should().BeEquivalentTo(sampleUser);
    }
    [Fact]
    public async Task EditUser_WhenValidData_ReturnsOk()
    {
        //Arrange
        UserDTO sampleUser = new UserDTO
        {
            Id = 1,
            FirstName = "Ramdom Nombre",
            LastName = "Random Last name",
            Email = "Random email",
        };

        _userService.EditUserAsync(sampleUser).Returns(Task.FromResult<UserDTO?>(sampleUser)!);
        var controller = GetControllerInstance();

        // Act
        var result = await controller.EditUser(sampleUser);

        // Assert
        result.Should().BeOfType<ActionResult<UserDTO>>();
        result.Result.Should().BeOfType<OkObjectResult>();

        (result?.Result as OkObjectResult)?.StatusCode.Should().Be(200);

        var okResult = result?.Result as OkObjectResult;
        var userData = okResult?.Value as UserDTO;
        userData.Should().BeEquivalentTo(sampleUser);
    }

    [Fact]
    public async Task EditUser_WhenInvalidData_ReturnsBadRequest()
    {
        // Arrange
        UserDTO invalidUser = new UserDTO(); // Datos no válidos, debería activar el ModelState.IsValid false
        var controller = GetControllerInstance();
        controller.ModelState.AddModelError("Name", "Name is required"); // Agregar un error de modelo simulado

        // Act
        var result = await controller.EditUser(invalidUser);

        // Assert
        result.Should().BeOfType<ActionResult<UserDTO>>();
        result.Result.Should().BeOfType<BadRequestObjectResult>();

        (result?.Result as BadRequestObjectResult)?.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task EditUser_WhenServiceThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        UserDTO sampleUser = new UserDTO
        {
            Id = 1,
            FirstName = "Ramdom Nombre",
            LastName = "Random Last name",
            Email = "Random email",
        };

        _userService
            .When(x => x.EditUserAsync(Arg.Any<UserDTO>()))
            .Throw(new Exception("Something went wrong in the service")); // Forzar que el servicio arroje una excepción
        var controller = GetControllerInstance();

        // Act
        var result = await controller.EditUser(sampleUser);

        // Assert
        result.Should().BeOfType<ActionResult<UserDTO>>();
        result.Result.Should().BeOfType<ObjectResult>();

        (result?.Result as ObjectResult)?.StatusCode.Should().Be(500);
        (result?.Result as ObjectResult)?.Value.Should().Be("Internal server error: Something went wrong in the service");
    }

}
