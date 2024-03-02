using NSubstitute;
using FluentAssertions;

using SkillMasteryAPI.Domain.Models;
using SkillMasteryAPI.Infrastructure.Data;
using SkillMasteryAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace SkillMasteryAPI.Infraestructure.Tests.Repositories;

public class GoalRepositoryTests : IDisposable
{
    private readonly SkillMasteryContext _context;

    public GoalRepositoryTests()
    {
        DbContextOptionsBuilder<SkillMasteryContext> dbContextOptions = new DbContextOptionsBuilder<SkillMasteryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());

        _context = new SkillMasteryContext(dbContextOptions.Options);
    }
    private GoalRepository GetRepositoryInstance() => new(_context);

    public void Dispose()
    {
        // Make sure that the in-memory database is deleted at the end of all tests.
        _context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetAllGoalsAsync_ShouldReturnGoals()
    {
        // Arrange
        var goals = new List<Goal>
        {
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
                Name = "Goalmer goal",
                Finish_Date = new DateOnly(2025,1,23),
                UserSkillId = 3,
                CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
            }
        };

        _context.Goal.AddRange(goals);
        await _context.SaveChangesAsync();

        var goalRepository = GetRepositoryInstance();
        // Act
        var result = await goalRepository.GetAllGoalsAsync();

        // Assert
        result.Should().BeEquivalentTo(goals);
        result.Should().HaveCount(goals.Count);
        result.Should().Equal(goals);
        result.First().Name.Should().Be(goals.First().Name);
        result.First().Finish_Date.Should().Be(goals.First().Finish_Date);
        result.First().UserSkillId.Should().Be(goals.First().UserSkillId);

    }

    [Fact]
    public async Task CreateGoalAsync_ShouldAddGoal_WhenCalled()
    {
        // Arrange

        var repository = new GoalRepository(_context);

        var newGoal = new Goal { };

        // Act
        var result = await repository.CreateGoalAsync(newGoal);

        // Assert
        Assert.NotNull(result);
        var goalInDb = await _context.Goal.FindAsync(result.Id);
        Assert.NotNull(goalInDb);

    }

    [Fact]
    public async Task DeleteGoalAsync_ShouldReturnDeletedGoal()
    {
        // Arrange
        var goal = new Goal
        {
            Id = 1,
            Name = "Sports goal",
            Finish_Date = new DateOnly(2025, 1, 23),
            UserSkillId = 1,
            CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
        };

        _context.Goal.Add(goal);
        await _context.SaveChangesAsync();

        var repository = new GoalRepository(_context);

        // Act
        var result = await repository.DeleteGoalAsync(goal);

        // Assert
        Assert.Equal(goal, result);
    }

    [Fact]
    public async Task GetGoalByIdAsync_WhenGoalExists_ReturnsGoal()
    {
        // Arrange
        var existingGoalId = 1;
        var existingGoal = new Goal
        {
            Id = 1,
            Name = "Sports goal",
            Finish_Date = new DateOnly(2025, 1, 23),
            UserSkillId = 1,
        };
        _context.Goal.Add(existingGoal);
        await _context.SaveChangesAsync();

        var goalRepository = GetRepositoryInstance();

        // Act
        var result = await goalRepository.GetGoalByIdAsync(existingGoalId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Goal>();
        result!.Id.Should().Be(existingGoalId);
        result.Name.Should().Be(existingGoal.Name);
        result.Finish_Date.Should().Be(existingGoal.Finish_Date);
        result.UserSkillId.Should().Be(existingGoal.UserSkillId);

    }

    [Fact]
    public async Task GetGoalByIdAsync_WhenGoalDoesNotExist_ReturnsNull()
    {
        // Arrange
        var nonExistingGoalId = 999;
        var goalRepository = GetRepositoryInstance();

        // Act
        var result = await goalRepository.GetGoalByIdAsync(nonExistingGoalId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task EditGoalAsync_WhenGoalExists_EditsAndReturnsEditedGoal()
    {
        // Arrange
        var existingGoalId = 1;
        var existingGoal = new Goal
        {
            Id = 1,
            Name = "Sports goal",
            Finish_Date = new DateOnly(2025, 1, 23),
            UserSkillId = 1,
        };
        _context.Goal.Add(existingGoal);
        await _context.SaveChangesAsync();

        var editedGoal = new Goal
        {
            Id = 1,
            Name = "Sports goal updated",
            Finish_Date = new DateOnly(2025, 1, 23),
            UserSkillId = 1,
        };
        GoalRepository? goalRepository = GetRepositoryInstance();

        // Act
        var result = await goalRepository.EditGoalAsync(editedGoal);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Goal>();
        result.Id.Should().Be(existingGoalId);
        result.Name.Should().Be(editedGoal.Name);
        result.Finish_Date.Should().Be(editedGoal.Finish_Date);

        // Additional Assertion: Ensure Goal is correctly edited in the database
        var editedGoalFromDb = await _context.Goal.FindAsync(existingGoalId);
        editedGoalFromDb.Should().NotBeNull();
        editedGoalFromDb!.Name.Should().Be(editedGoal.Name);
        editedGoalFromDb.Finish_Date.Should().Be(editedGoal.Finish_Date);
        editedGoalFromDb.UserSkillId.Should().Be(editedGoal.UserSkillId);

    }
}
