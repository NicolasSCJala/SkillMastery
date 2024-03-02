using NSubstitute;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

using Moq;

using SkillMasteryAPI.Application.Services.Interfaces;
using SkillMasteryAPI.Application.DTOs.Skill;
using SkillMasteryAPI.Presentation.Controllers;

namespace SkillMasteryAPI.Presentation.Tests.Controllers;

public class SkillsControllerTests
{
    private readonly ISkillService _skillService;
    private readonly Mock<ISkillService> _mockSkillService;
    private readonly SkillsController _controller;

    public SkillsControllerTests()
    {
        _skillService = Substitute.For<ISkillService>();
        _mockSkillService = new Mock<ISkillService>();
        _controller = new SkillsController(_mockSkillService.Object);
    }
    private SkillsController GetControllerInstance() => new(_skillService);

    [Fact]
    public async Task GetAllSkills_ShouldReturnOk()
    {
        // Arrange
        _skillService.GetAllSkillsAsync().Returns(new List<SkillDTO>());
        var controller = GetControllerInstance();

        // Act
        var result = await controller.GetAllSkills();

        // Verify that the result is an ActionResult<IEnumerable<SkillDTO>>
        result.Should().BeOfType<ActionResult<IEnumerable<SkillDTO>>>();

        // Verify that the Result of the ActionResult is an OkObjectResult
        result.Result.Should().BeOfType<OkObjectResult>();

        // Verify the status code
        (result?.Result as OkObjectResult)?.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task CreateSkill_Returns201Created_WithValidInput()
    {
        // Arrange
        var createSkillDTO = new CreateSkillDTO { /* Populate required properties */ };
        var createdSkillDTO = new SkillDTO { /* Populate with expected result */ };
        _mockSkillService.Setup(service => service.CreateSkillAsync(createSkillDTO))
                           .ReturnsAsync(createdSkillDTO);

        // Act
        var result = await _controller.CreateSkill(createSkillDTO);

        // Assert
        var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(201, actionResult.StatusCode);
        Assert.Equal(createdSkillDTO, actionResult.Value);
    }

    [Fact]
    public async Task CreateSkill_Returns400BadRequest_WithInvalidModel()
    {
        // Arrange
        _controller.ModelState.AddModelError("Error", "Sample error");

        // Act
        var result = await _controller.CreateSkill(new CreateSkillDTO());

        // Assert
        var actionResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(400, actionResult.StatusCode);
    }

    [Fact]
    public async Task CreateSkill_HandlesException_WithInternalServerError()
    {
        // Arrange
        var createSkillDTO = new CreateSkillDTO { /* Populate required properties */ };
        _mockSkillService.Setup(service => service.CreateSkillAsync(createSkillDTO))
                           .ThrowsAsync(new System.Exception("Test exception"));

        // Act
        var result = await _controller.CreateSkill(createSkillDTO);

        // Assert
        var actionResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, actionResult.StatusCode);
        Assert.Contains("Internal server error", actionResult?.Value?.ToString());
    }

    [Fact]
    public async Task DeleteSkill_ShouldReturnOk()
    {
        // Arrange
        SkillDTO sampleSkill = new SkillDTO
        {
            Id = 1,
            Name = "Frontend Development",
            Description = "Skill to develop Web Applications focusing on HTML, CSS, JavaScript, and popular frameworks.",
        };

        _skillService.DeleteSkillAsync(1).Returns(Task.FromResult<SkillDTO?>(sampleSkill));
        var controller = GetControllerInstance();

        // Act
        var result = await controller.DeleteSkill(1);

        // Assert
        result.Should().BeOfType<ActionResult<SkillDTO>>();

        result.Result.Should().BeOfType<OkObjectResult>();

        (result?.Result as OkObjectResult)?.StatusCode.Should().Be(200);

        var okResult = result?.Result as OkObjectResult;
        var skillData = okResult?.Value as SkillDTO;
        skillData.Should().BeEquivalentTo(sampleSkill);
    }


    [Fact]
    public async Task EditSkill_ShouldReturnOk()
    {
        // Arrange
        SkillDTO sampleSkill = new SkillDTO
        {
            Id = 1,
            Name = "Edited Frontend Development",
            Description = "Skill to develop Web Applications focusing on HTML, CSS.",
        };

        _skillService.EditSkillAsync(sampleSkill).Returns(Task.FromResult<SkillDTO?>(sampleSkill));
        var controller = GetControllerInstance();

        // Act
        var result = await controller.EditSkill(sampleSkill);

        // Assert
        result.Should().BeOfType<ActionResult<SkillDTO>>();

        result.Result.Should().BeOfType<OkObjectResult>();

        (result?.Result as OkObjectResult)?.StatusCode.Should().Be(200);

        var okResult = result?.Result as OkObjectResult;
        var skillData = okResult?.Value as SkillDTO;
        skillData.Should().BeEquivalentTo(sampleSkill);
    }
    [Fact]
    public async Task EditSkill_WhenValidData_ReturnsOk()
    {
        //Arrange
        SkillDTO sampleSkill = new SkillDTO
        {
            Id = 1,
            Name = "Edited Frontend Development",
            Description = "Skill to develop Web Applications focusing on HTML, CSS.",
        };

        _skillService.EditSkillAsync(sampleSkill).Returns(Task.FromResult<SkillDTO?>(sampleSkill)!);
        var controller = GetControllerInstance();

        // Act
        var result = await controller.EditSkill(sampleSkill);

        // Assert
        result.Should().BeOfType<ActionResult<SkillDTO>>();
        result.Result.Should().BeOfType<OkObjectResult>();

        (result?.Result as OkObjectResult)?.StatusCode.Should().Be(200);

        var okResult = result?.Result as OkObjectResult;
        var skillData = okResult?.Value as SkillDTO;
        skillData.Should().BeEquivalentTo(sampleSkill);
    }

    [Fact]
    public async Task EditSkill_WhenInvalidData_ReturnsBadRequest()
    {
        // Arrange
        SkillDTO invalidSkill = new SkillDTO(); // Datos no válidos, debería activar el ModelState.IsValid false
        var controller = GetControllerInstance();
        controller.ModelState.AddModelError("Name", "Name is required"); // Agregar un error de modelo simulado

        // Act
        var result = await controller.EditSkill(invalidSkill);

        // Assert
        result.Should().BeOfType<ActionResult<SkillDTO>>();
        result.Result.Should().BeOfType<BadRequestObjectResult>();

        (result?.Result as BadRequestObjectResult)?.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task EditSkill_WhenServiceThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        SkillDTO sampleSkill = new SkillDTO
        {
            Id = 1,
            Name = "Edited Frontend Development",
            Description = "Skill to develop Web Applications focusing on HTML, CSS.",
        };

        _skillService
            .When(x => x.EditSkillAsync(Arg.Any<SkillDTO>()))
            .Throw(new Exception("Something went wrong in the service")); // Forzar que el servicio arroje una excepción
        var controller = GetControllerInstance();

        // Act
        var result = await controller.EditSkill(sampleSkill);

        // Assert
        result.Should().BeOfType<ActionResult<SkillDTO>>();
        result.Result.Should().BeOfType<ObjectResult>();

        (result?.Result as ObjectResult)?.StatusCode.Should().Be(500);
        (result?.Result as ObjectResult)?.Value.Should().Be("Internal server error: Something went wrong in the service");
    }

}
