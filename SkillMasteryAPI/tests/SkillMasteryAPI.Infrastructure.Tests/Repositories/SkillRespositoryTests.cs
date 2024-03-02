using NSubstitute;
using FluentAssertions;

using SkillMasteryAPI.Domain.Models;
using SkillMasteryAPI.Infrastructure.Data;
using SkillMasteryAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace SkillMasteryAPI.Infraestructure.Tests.Repositories;

public class SkillRepositoryTests : IDisposable
{
    private readonly SkillMasteryContext _context;

    public SkillRepositoryTests()
    {
        DbContextOptionsBuilder<SkillMasteryContext> dbContextOptions = new DbContextOptionsBuilder<SkillMasteryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());

        _context = new SkillMasteryContext(dbContextOptions.Options);
    }
    private SkillRepository GetRepositoryInstance() => new(_context);

    public void Dispose()
    {
        // Make sure that the in-memory database is deleted at the end of all tests.
        _context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetAllSkillsAsync_ShouldReturnSkills()
    {
        // Arrange
        var goals = new List<Skill>
        {
              new Skill
            {
                Id = 1,
                Name = "Skill Developers 01",
                Description = "Skill covering concepts in software development.",
                DificultyId = 1,
                CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
            },
            new Skill
            {
                Id = 2,
                Name = "Advanced Skill Developers 01",
                Description = "Skill focused on advanced software design and development techniques.",
                DificultyId = 2,
                CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
            },
            new Skill
            {
                Id = 3,
                Name = "Skill Data Science and Analytics 01",
                Description = "Skill designed to teach the fundamentals of data analysis, machine learning, and statistical modeling.",
                DificultyId = 3,
                CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
            },
            new Skill
            {
                Id = 4,
                Name = "Skill Developers 02",
                Description = "Skill covering concepts in software development.",
                DificultyId = 4,
                CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
            }
        };

        _context.Skill.AddRange(goals);
        await _context.SaveChangesAsync();

        var goalRepository = GetRepositoryInstance();
        // Act
        var result = await goalRepository.GetAllSkillsAsync();

        // Assert
        result.Should().BeEquivalentTo(goals);
        result.Should().HaveCount(goals.Count);
        result.Should().Equal(goals);
        result.First().Name.Should().Be(goals.First().Name);
        result.First().Description.Should().Be(goals.First().Description);
        result.First().DificultyId.Should().Be(goals.First().DificultyId);

    }

    [Fact]
    public async Task CreateSkillAsync_ShouldAddSkill_WhenCalled()
    {
        // Arrange

        var repository = new SkillRepository(_context);

        var newSkill = new Skill { };

        // Act
        var result = await repository.CreateSkillAsync(newSkill);

        // Assert
        Assert.NotNull(result);
        var goalInDb = await _context.Skill.FindAsync(result.Id);
        Assert.NotNull(goalInDb);

    }

    [Fact]
    public async Task DeleteSkillAsync_ShouldReturnDeletedSkill()
    {
        // Arrange
        var goal = new Skill
        {
            Id = 1,
            Name = "Sports goal",
            Description = "Skill covering concepts in software development.",
            DificultyId = 1,
            CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
        };

        _context.Skill.Add(goal);
        await _context.SaveChangesAsync();

        var repository = new SkillRepository(_context);

        // Act
        var result = await repository.DeleteSkillAsync(goal);

        // Assert
        Assert.Equal(goal, result);
    }

    [Fact]
    public async Task GetSkillByIdAsync_WhenSkillExists_ReturnsSkill()
    {
        // Arrange
        var existingSkillId = 1;
        var existingSkill = new Skill
        {
            Id = 1,
            Name = "Sports goal",
            Description = "Skill covering concepts in software development.",
            DificultyId = 1,
        };
        _context.Skill.Add(existingSkill);
        await _context.SaveChangesAsync();

        var goalRepository = GetRepositoryInstance();

        // Act
        var result = await goalRepository.GetSkillByIdAsync(existingSkillId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Skill>();
        result!.Id.Should().Be(existingSkillId);
        result.Name.Should().Be(existingSkill.Name);
        result.Description.Should().Be(existingSkill.Description);
        result.DificultyId.Should().Be(existingSkill.DificultyId);

    }

    [Fact]
    public async Task GetSkillByIdAsync_WhenSkillDoesNotExist_ReturnsNull()
    {
        // Arrange
        var nonExistingSkillId = 999;
        var goalRepository = GetRepositoryInstance();

        // Act
        var result = await goalRepository.GetSkillByIdAsync(nonExistingSkillId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task EditSkillAsync_WhenSkillExists_EditsAndReturnsEditedSkill()
    {
        // Arrange
        var existingSkillId = 1;
        var existingSkill = new Skill
        {
            Id = 1,
            Name = "Sports goal",
            Description = "Skill covering concepts in software development.",
            DificultyId = 1,
        };
        _context.Skill.Add(existingSkill);
        await _context.SaveChangesAsync();

        var editedSkill = new Skill
        {
            Id = 1,
            Name = "Sports goal updated",
            Description = "Skill covering concepts in software development.",
            DificultyId = 1,
        };
        SkillRepository? goalRepository = GetRepositoryInstance();

        // Act
        var result = await goalRepository.EditSkillAsync(editedSkill);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Skill>();
        result.Id.Should().Be(existingSkillId);
        result.Name.Should().Be(editedSkill.Name);
        result.Description.Should().Be(editedSkill.Description);

        // Additional Assertion: Ensure Skill is correctly edited in the database
        var editedSkillFromDb = await _context.Skill.FindAsync(existingSkillId);
        editedSkillFromDb.Should().NotBeNull();
        editedSkillFromDb!.Name.Should().Be(editedSkill.Name);
        editedSkillFromDb.Description.Should().Be(editedSkill.Description);
        editedSkillFromDb.DificultyId.Should().Be(editedSkill.DificultyId);

    }
}
