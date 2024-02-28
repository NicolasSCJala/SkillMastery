using Moq;
using NSubstitute;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

using SkillMasteryAPI.Domain.Models;
using SkillMasteryAPI.Infrastructure.Repositories.Interfaces;
using SkillMasteryAPI.Application.DTOs.Goal;
using SkillMasteryAPI.Application.Services;
using SkillMasteryAPI.Application.CrossCutting;


namespace SkillMasteryAPI.Application.Tests.Services;

public class GoalServiceTests
{
    private readonly IGoalRepository _goalRepository;
    private readonly IMapper _mapper;

    private readonly Mock<IGoalRepository> _goalRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();

    public GoalServiceTests()
    {
        _goalRepository = Substitute.For<IGoalRepository>();
        _mapper = new Mapper();
    }

    private GoalService GetServiceInstance() => new(_goalRepository, _mapper);

    [Fact]
    public async Task GetAllGoalsAsync_ShouldReturnGoalDTOs()
    {
        // Arrange
        var goalService = GetServiceInstance();

        // Define some sample goals from the repository
        List<Goal> sampleGoals = new List<Goal>{
            new Goal
            {
                Id = 1,
                Name = "Sports goal",
                Finish_Date = new DateOnly(2025,1,23),
                UserSkillId = 1,
                CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
            },
            new Goal
            {
                 Id = 2,
                Name = "Rock Star",
                Finish_Date = new DateOnly(2025,1,23),
                UserSkillId = 2,
                CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
            },
            new Goal
            {
                Id = 3,
                Name = "Programmer goal",
                Finish_Date = new DateOnly(2025,1,23),
                UserSkillId = 3,
                CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
            }
   
          
        };


        _goalRepository.GetAllGoalsAsync().Returns(Task.FromResult<IEnumerable<Goal>>(sampleGoals));

        // Act
        var result = await goalService.GetAllGoalsAsync();

        // Assert
        result.Should().NotBeNull(); // Ensure the result is not null

        // Use BeEquivalentTo to compare elements and properties without requiring exact type matching
        result.Should().BeEquivalentTo(sampleGoals.Select(goal => _mapper.Map<GoalDTO>(goal)));

        // Check if the number of items in the result matches the number of items in the sampleGoals
        result.Should().HaveCount(sampleGoals.Count);

        // Ensure that each item in the result is of the expected type GoalDTO
        result.Should().ContainItemsAssignableTo<GoalDTO>();
    }

    [Fact]
    public async Task CreateGoalAsync_ShouldReturnGoalDTO_WhenCreateIsSuccessful()
    {
        // Arrange
        var createGoalDTO = new CreateGoalDTO
        {
            Name = "Sports goal",
            Finish_Date = new DateOnly(2025, 1, 23),
            UserSkillId = 1,
        };

        var goal = new Goal
        {
            Id = 2,
            Name = "Rock Star",
            Finish_Date = new DateOnly(2025, 1, 23),
            UserSkillId = 2,
            CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
        };

        var goalDTO = new GoalDTO
        {
            Id = 3,
            Name = "Programmer goal",
            Finish_Date = new DateOnly(2025, 1, 23),
            UserSkillId = 3
        };

        _mapperMock.Setup(m => m.Map<Goal>(It.IsAny<CreateGoalDTO>())).Returns(goal);
        _goalRepositoryMock.Setup(repo => repo.CreateGoalAsync(It.IsAny<Goal>())).ReturnsAsync(goal);
        _mapperMock.Setup(m => m.Map<GoalDTO>(It.IsAny<Goal>())).Returns(goalDTO);

        var service = new GoalService(_goalRepositoryMock.Object, _mapperMock.Object);

        // Act
        var result = await service.CreateGoalAsync(createGoalDTO);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(goalDTO, result);
        Assert.Equal(goalDTO.Name, result.Name);
        Assert.Equal(goalDTO.Finish_Date, result.Finish_Date);
        Assert.Equal(goalDTO.UserSkillId, result.UserSkillId);

        _goalRepositoryMock.Verify(repo => repo.CreateGoalAsync(It.IsAny<Goal>()), Times.Once);
        _mapperMock.Verify(m => m.Map<GoalDTO>(It.IsAny<Goal>()), Times.Once);
    }

    [Fact]
    public async Task DeleteGoalAsync_ShouldReturnDeletedGoalDTO()
    {
        // Arrange
        GoalService goalService = GetServiceInstance();

        // Define a sample goal from the repository
        Goal sampleGoal = new Goal
        {
            Id = 2,
            Name = "Rock Star",
            Finish_Date = new DateOnly(2025, 1, 23),
            UserSkillId = 2,
            CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
        };

        _goalRepository.GetGoalByIdAsync(1).Returns(Task.FromResult<Goal?>(sampleGoal));

        _goalRepository.DeleteGoalAsync(sampleGoal).Returns(Task.FromResult(sampleGoal));

        // Act
        var result = await goalService.DeleteGoalAsync(1);

        // Assert
        result.Should().NotBeNull();

        result.Should().BeEquivalentTo(_mapper.Map<GoalDTO>(sampleGoal));
    }

    [Fact]
    public async Task DeleteGoalAsync_ShouldThrowNotFoundException_WhenGoalNotFound()
    {
        // Arrange
        GoalService goalService = GetServiceInstance();

        _goalRepository.GetGoalByIdAsync(1).Returns(Task.FromResult<Goal?>(null));

        // Act
        Func<Task> act = async () => await goalService.DeleteGoalAsync(1);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Goal with id 1 not found");
    }

    [Fact]
    public async Task DeleteGoalAsync_ShoulThrowFoundException_WhenGoalIsUsedInClassroom()
    {
        // Arrange
        GoalService goalService = GetServiceInstance();

        Goal sampleGoal = new Goal
        {
            Id = 2,
            Name = "Rock Star",
            Finish_Date = new DateOnly(2025, 1, 23),
            UserSkillId = 2,
            CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
        };

      

        _goalRepository.GetGoalByIdAsync(1).Returns(Task.FromResult<Goal?>(sampleGoal));

        // Act
        Func<Task> act = async () => await goalService.DeleteGoalAsync(1);

        // Assert
        await act.Should().ThrowAsync<FoundException>().WithMessage("Goal with id 1 is used in a classroom");
    }

    [Fact]
    public async Task EditGoalAsync_ShouldReturnEditedGoalDTO()
    {
        //Arrange
        var goalService = GetServiceInstance();
        var initialGoal = new Goal
        {
            Id = 2,
            Name = "Initial Goal",
            Finish_Date = new DateOnly(2025, 1, 23),
            UserSkillId = 2,
      
        };

        _goalRepository.GetGoalByIdAsync(1).Returns(Task.FromResult<Goal?>(initialGoal));
        var updatedGoal = new Goal
        {
            Id = 2,
            Name = "Updated Goal",
            Finish_Date = new DateOnly(2025, 1, 23),
            UserSkillId = 2
        };
        _goalRepository.EditGoalAsync(Arg.Any<Goal>()).Returns(Task.FromResult(updatedGoal));

        //Act   
        var result = await goalService.EditGoalAsync(_mapper.Map<GoalDTO>(updatedGoal));

        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_mapper.Map<GoalDTO>(updatedGoal));

    }
}
