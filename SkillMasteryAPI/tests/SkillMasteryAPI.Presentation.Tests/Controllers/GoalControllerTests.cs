using NSubstitute;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

using Moq;

using SkillMasteryAPI.Application.Services.Interfaces;
using SkillMasteryAPI.Application.DTOs.Goal;
using SkillMasteryAPI.Presentation.Controllers;

namespace SkillMasteryAPI.Presentation.Tests.Controllers;

public class GoalsControllerTests
{
    private readonly IGoalService _goalService;
    private readonly Mock<IGoalService> _mockGoalService;
    private readonly GoalsController _controller;

    public GoalsControllerTests()
    {
        _goalService = Substitute.For<IGoalService>();
        _mockGoalService = new Mock<IGoalService>();
        _controller = new GoalsController(_mockGoalService.Object);
    }
    private GoalsController GetControllerInstance() => new(_goalService);

    [Fact]
    public async Task GetAllGoals_ShouldReturnOk()
    {
        // Arrange
        _goalService.GetAllGoalsAsync().Returns(new List<GoalDTO>());
        var controller = GetControllerInstance();

        // Act
        var result = await controller.GetAllGoals();

        // Verify that the result is an ActionResult<IEnumerable<GoalDTO>>
        result.Should().BeOfType<ActionResult<IEnumerable<GoalDTO>>>();

        // Verify that the Result of the ActionResult is an OkObjectResult
        result.Result.Should().BeOfType<OkObjectResult>();

        // Verify the status code
        (result?.Result as OkObjectResult)?.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task CreateGoal_Returns201Created_WithValidInput()
    {
        // Arrange
        var createGoalDTO = new CreateGoalDTO { /* Populate required properties */ };
        var createdGoalDTO = new GoalDTO { /* Populate with expected result */ };
        _mockGoalService.Setup(service => service.CreateGoalAsync(createGoalDTO))
                           .ReturnsAsync(createdGoalDTO);

        // Act
        var result = await _controller.CreateGoal(createGoalDTO);

        // Assert
        var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(201, actionResult.StatusCode);
        Assert.Equal(createdGoalDTO, actionResult.Value);
    }

    [Fact]
    public async Task CreateGoal_Returns400BadRequest_WithInvalidModel()
    {
        // Arrange
        _controller.ModelState.AddModelError("Error", "Sample error");

        // Act
        var result = await _controller.CreateGoal(new CreateGoalDTO());

        // Assert
        var actionResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(400, actionResult.StatusCode);
    }

    [Fact]
    public async Task CreateGoal_HandlesException_WithInternalServerError()
    {
        // Arrange
        var createGoalDTO = new CreateGoalDTO { /* Populate required properties */ };
        _mockGoalService.Setup(service => service.CreateGoalAsync(createGoalDTO))
                           .ThrowsAsync(new System.Exception("Test exception"));

        // Act
        var result = await _controller.CreateGoal(createGoalDTO);

        // Assert
        var actionResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, actionResult.StatusCode);
        Assert.Contains("Internal server error", actionResult?.Value?.ToString());
    }

    [Fact]
    public async Task DeleteGoal_ShouldReturnOk()
    {
        // Arrange
        GoalDTO sampleGoal = new GoalDTO
        {
            Id = 1,
            Name = "Goalmer goal",
            Finish_Date = new DateOnly(2025, 1, 23),
            UserSkillId = 3
        };

        _goalService.DeleteGoalAsync(1).Returns(Task.FromResult<GoalDTO?>(sampleGoal));
        var controller = GetControllerInstance();

        // Act
        var result = await controller.DeleteGoal(1);

        // Assert
        result.Should().BeOfType<ActionResult<GoalDTO>>();

        result.Result.Should().BeOfType<OkObjectResult>();

        (result?.Result as OkObjectResult)?.StatusCode.Should().Be(200);

        var okResult = result?.Result as OkObjectResult;
        var goalData = okResult?.Value as GoalDTO;
        goalData.Should().BeEquivalentTo(sampleGoal);
    }


    [Fact]
    public async Task EditGoal_ShouldReturnOk()
    {
        // Arrange
        GoalDTO sampleGoal = new GoalDTO
        {
            Id= 1,
            Name = "Goalmer goal",
            Finish_Date = new DateOnly(2025, 1, 23),
            UserSkillId = 3
        };

        _goalService.EditGoalAsync(sampleGoal).Returns(Task.FromResult<GoalDTO?>(sampleGoal));
        var controller = GetControllerInstance();

        // Act
        var result = await controller.EditGoal(sampleGoal);

        // Assert
        result.Should().BeOfType<ActionResult<GoalDTO>>();

        result.Result.Should().BeOfType<OkObjectResult>();

        (result?.Result as OkObjectResult)?.StatusCode.Should().Be(200);

        var okResult = result?.Result as OkObjectResult;
        var goalData = okResult?.Value as GoalDTO;
        goalData.Should().BeEquivalentTo(sampleGoal);
    }
    [Fact]
    public async Task EditGoal_WhenValidData_ReturnsOk()
    {
        //Arrange
        GoalDTO sampleGoal = new GoalDTO
        {
            Id = 1,
            Name = "Goalmer goal",
            Finish_Date = new DateOnly(2025, 1, 23),
            UserSkillId = 3
        };

        _goalService.EditGoalAsync(sampleGoal).Returns(Task.FromResult<GoalDTO?>(sampleGoal)!);
        var controller = GetControllerInstance();

        // Act
        var result = await controller.EditGoal(sampleGoal);

        // Assert
        result.Should().BeOfType<ActionResult<GoalDTO>>();
        result.Result.Should().BeOfType<OkObjectResult>();

        (result?.Result as OkObjectResult)?.StatusCode.Should().Be(200);

        var okResult = result?.Result as OkObjectResult;
        var goalData = okResult?.Value as GoalDTO;
        goalData.Should().BeEquivalentTo(sampleGoal);
    }

    [Fact]
    public async Task EditGoal_WhenInvalidData_ReturnsBadRequest()
    {
        // Arrange
        GoalDTO invalidGoal = new GoalDTO(); // Datos no válidos, debería activar el ModelState.IsValid false
        var controller = GetControllerInstance();
        controller.ModelState.AddModelError("Name", "Name is required"); // Agregar un error de modelo simulado

        // Act
        var result = await controller.EditGoal(invalidGoal);

        // Assert
        result.Should().BeOfType<ActionResult<GoalDTO>>();
        result.Result.Should().BeOfType<BadRequestObjectResult>();

        (result?.Result as BadRequestObjectResult)?.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task EditGoal_WhenServiceThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        GoalDTO sampleGoal = new GoalDTO
        {
            Id = 1,
            Name = "Goalmer goal",
            Finish_Date = new DateOnly(2025, 1, 23),
            UserSkillId = 3
        };

        _goalService
            .When(x => x.EditGoalAsync(Arg.Any<GoalDTO>()))
            .Throw(new Exception("Something went wrong in the service")); // Forzar que el servicio arroje una excepción
        var controller = GetControllerInstance();

        // Act
        var result = await controller.EditGoal(sampleGoal);

        // Assert
        result.Should().BeOfType<ActionResult<GoalDTO>>();
        result.Result.Should().BeOfType<ObjectResult>();

        (result?.Result as ObjectResult)?.StatusCode.Should().Be(500);
        (result?.Result as ObjectResult)?.Value.Should().Be("Internal server error: Something went wrong in the service");
    }

}
