using NSubstitute;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

using Moq;

using SkillMasteryAPI.Application.Services.Interfaces;
using SkillMasteryAPI.Application.DTOs.UserSkill;
using SkillMasteryAPI.Presentation.Controllers;

namespace SkillMasteryAPI.Presentation.Tests.Controllers;

public class UserSkillsControllerTests
{
    private readonly IUserSkillService _userskillService;
    private readonly Mock<IUserSkillService> _mockUserSkillService;
    private readonly UserSkillsController _controller;

    public UserSkillsControllerTests()
    {
        _userskillService = Substitute.For<IUserSkillService>();
        _mockUserSkillService = new Mock<IUserSkillService>();
        _controller = new UserSkillsController(_mockUserSkillService.Object);
    }
    private UserSkillsController GetControllerInstance() => new(_userskillService);

    [Fact]
    public async Task GetAllUserSkills_ShouldReturnOk()
    {
        // Arrange
        _userskillService.GetAllUserSkillsAsync().Returns(new List<UserSkillDTO>());
        var controller = GetControllerInstance();

        // Act
        var result = await controller.GetAllUserSkills();

        // Verify that the result is an ActionResult<IEnumerable<UserSkillDTO>>
        result.Should().BeOfType<ActionResult<IEnumerable<UserSkillDTO>>>();

        // Verify that the Result of the ActionResult is an OkObjectResult
        result.Result.Should().BeOfType<OkObjectResult>();

        // Verify the status code
        (result?.Result as OkObjectResult)?.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task CreateUserSkill_Returns201Created_WithValidInput()
    {
        // Arrange
        var createUserSkillDTO = new CreateUserSkillDTO { /* Populate required properties */ };
        var createdUserSkillDTO = new UserSkillDTO { /* Populate with expected result */ };
        _mockUserSkillService.Setup(service => service.CreateUserSkillAsync(createUserSkillDTO))
                           .ReturnsAsync(createdUserSkillDTO);

        // Act
        var result = await _controller.CreateUserSkill(createUserSkillDTO);

        // Assert
        var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(201, actionResult.StatusCode);
        Assert.Equal(createdUserSkillDTO, actionResult.Value);
    }

    [Fact]
    public async Task CreateUserSkill_Returns400BadRequest_WithInvalidModel()
    {
        // Arrange
        _controller.ModelState.AddModelError("Error", "Sample error");

        // Act
        var result = await _controller.CreateUserSkill(new CreateUserSkillDTO());

        // Assert
        var actionResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(400, actionResult.StatusCode);
    }

    [Fact]
    public async Task CreateUserSkill_HandlesException_WithInternalServerError()
    {
        // Arrange
        var createUserSkillDTO = new CreateUserSkillDTO { /* Populate required properties */ };
        _mockUserSkillService.Setup(service => service.CreateUserSkillAsync(createUserSkillDTO))
                           .ThrowsAsync(new System.Exception("Test exception"));

        // Act
        var result = await _controller.CreateUserSkill(createUserSkillDTO);

        // Assert
        var actionResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, actionResult.StatusCode);
        Assert.Contains("Internal server error", actionResult?.Value?.ToString());
    }

    [Fact]
    public async Task DeleteUserSkill_ShouldReturnOk()
    {
        // Arrange
        UserSkillDTO sampleUserSkill = new UserSkillDTO
        {
            Id = 1,
            Status = true,
            UserId = 1,
            SkillId = 1,
        };

        _userskillService.DeleteUserSkillAsync(1).Returns(Task.FromResult<UserSkillDTO?>(sampleUserSkill));
        var controller = GetControllerInstance();

        // Act
        var result = await controller.DeleteUserSkill(1);

        // Assert
        result.Should().BeOfType<ActionResult<UserSkillDTO>>();

        result.Result.Should().BeOfType<OkObjectResult>();

        (result?.Result as OkObjectResult)?.StatusCode.Should().Be(200);

        var okResult = result?.Result as OkObjectResult;
        var userskillData = okResult?.Value as UserSkillDTO;
        userskillData.Should().BeEquivalentTo(sampleUserSkill);
    }


    [Fact]
    public async Task EditUserSkill_ShouldReturnOk()
    {
        // Arrange
        UserSkillDTO sampleUserSkill = new UserSkillDTO
        {
            Id = 1,
            Status = true,
            UserId = 1,
            SkillId = 1,
        };

        _userskillService.EditUserSkillAsync(sampleUserSkill).Returns(Task.FromResult<UserSkillDTO?>(sampleUserSkill));
        var controller = GetControllerInstance();

        // Act
        var result = await controller.EditUserSkill(sampleUserSkill);

        // Assert
        result.Should().BeOfType<ActionResult<UserSkillDTO>>();

        result.Result.Should().BeOfType<OkObjectResult>();

        (result?.Result as OkObjectResult)?.StatusCode.Should().Be(200);

        var okResult = result?.Result as OkObjectResult;
        var userskillData = okResult?.Value as UserSkillDTO;
        userskillData.Should().BeEquivalentTo(sampleUserSkill);
    }
    [Fact]
    public async Task EditUserSkill_WhenValidData_ReturnsOk()
    {
        //Arrange
        UserSkillDTO sampleUserSkill = new UserSkillDTO
        {
            Id = 1,
            Status = true,
            UserId = 1,
            SkillId = 1,
        };

        _userskillService.EditUserSkillAsync(sampleUserSkill).Returns(Task.FromResult<UserSkillDTO?>(sampleUserSkill)!);
        var controller = GetControllerInstance();

        // Act
        var result = await controller.EditUserSkill(sampleUserSkill);

        // Assert
        result.Should().BeOfType<ActionResult<UserSkillDTO>>();
        result.Result.Should().BeOfType<OkObjectResult>();

        (result?.Result as OkObjectResult)?.StatusCode.Should().Be(200);

        var okResult = result?.Result as OkObjectResult;
        var userskillData = okResult?.Value as UserSkillDTO;
        userskillData.Should().BeEquivalentTo(sampleUserSkill);
    }

    [Fact]
    public async Task EditUserSkill_WhenInvalidData_ReturnsBadRequest()
    {
        // Arrange
        UserSkillDTO invalidUserSkill = new UserSkillDTO(); // Datos no válidos, debería activar el ModelState.IsValid false
        var controller = GetControllerInstance();
        controller.ModelState.AddModelError("Name", "Name is required"); // Agregar un error de modelo simulado

        // Act
        var result = await controller.EditUserSkill(invalidUserSkill);

        // Assert
        result.Should().BeOfType<ActionResult<UserSkillDTO>>();
        result.Result.Should().BeOfType<BadRequestObjectResult>();

        (result?.Result as BadRequestObjectResult)?.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task EditUserSkill_WhenServiceThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        UserSkillDTO sampleUserSkill = new UserSkillDTO
        {
            Id = 1,
            Status = true,
            UserId = 1,
            SkillId = 1,
        };

        _userskillService
            .When(x => x.EditUserSkillAsync(Arg.Any<UserSkillDTO>()))
            .Throw(new Exception("Something went wrong in the service")); // Forzar que el servicio arroje una excepción
        var controller = GetControllerInstance();

        // Act
        var result = await controller.EditUserSkill(sampleUserSkill);

        // Assert
        result.Should().BeOfType<ActionResult<UserSkillDTO>>();
        result.Result.Should().BeOfType<ObjectResult>();

        (result?.Result as ObjectResult)?.StatusCode.Should().Be(500);
        (result?.Result as ObjectResult)?.Value.Should().Be("Internal server error: Something went wrong in the service");
    }

}
