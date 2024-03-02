using NSubstitute;
using FluentAssertions;

using SkillMasteryAPI.Domain.Models;
using SkillMasteryAPI.Infrastructure.Data;
using SkillMasteryAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace SkillMasteryAPI.Infraestructure.Tests.Repositories;

public class UserSkillRepositoryTests : IDisposable
{
    private readonly SkillMasteryContext _context;

    public UserSkillRepositoryTests()
    {
        DbContextOptionsBuilder<SkillMasteryContext> dbContextOptions = new DbContextOptionsBuilder<SkillMasteryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());

        _context = new SkillMasteryContext(dbContextOptions.Options);
    }
    private UserSkillRepository GetRepositoryInstance() => new(_context);

    public void Dispose()
    {
        // Make sure that the in-memory database is deleted at the end of all tests.
        _context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetAllUserSkillsAsync_ShouldReturnUserSkills()
    {
        // Arrange
        var userskills = new List<UserSkill>
        {
             new UserSkill
            {
                Id = 1,
                Status = true,
                UserId = 1,
                SkillId = 1,
                CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
            },
            new UserSkill
            {
                 Id = 2,
                Status = false,
                UserId = 2,
                SkillId = 2,
                CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
            },
            new UserSkill
            {
                Id = 3,
                Status = true,
                UserId = 3,
                SkillId = 3,
                CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
            }
        };

        _context.UserSkill.AddRange(userskills);
        await _context.SaveChangesAsync();

        var userskillRepository = GetRepositoryInstance();
        // Act
        var result = await userskillRepository.GetAllUserSkillsAsync();

        // Assert
        result.Should().BeEquivalentTo(userskills);
        result.Should().HaveCount(userskills.Count);
        result.Should().Equal(userskills);
        result.First().Status.Should().Be(userskills.First().Status);
        result.First().UserId.Should().Be(userskills.First().UserId);
        result.First().SkillId.Should().Be(userskills.First().SkillId);

    }

    [Fact]
    public async Task CreateUserSkillAsync_ShouldAddUserSkill_WhenCalled()
    {
        // Arrange

        var repository = new UserSkillRepository(_context);

        var newUserSkill = new UserSkill { };

        // Act
        var result = await repository.CreateUserSkillAsync(newUserSkill);

        // Assert
        Assert.NotNull(result);
        var userskillInDb = await _context.UserSkill.FindAsync(result.Id);
        Assert.NotNull(userskillInDb);

    }

    [Fact]
    public async Task DeleteUserSkillAsync_ShouldReturnDeletedUserSkill()
    {
        // Arrange
        var userskill = new UserSkill
        {
            Id = 3,
            Status = true,
            UserId = 3,
            SkillId = 3,
            CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
        };

        _context.UserSkill.Add(userskill);
        await _context.SaveChangesAsync();

        var repository = new UserSkillRepository(_context);

        // Act
        var result = await repository.DeleteUserSkillAsync(userskill);

        // Assert
        Assert.Equal(userskill, result);
    }

    [Fact]
    public async Task GetUserSkillByIdAsync_WhenUserSkillExists_ReturnsUserSkill()
    {
        // Arrange
        var existingSkillId = 1;
        var existingUserSkill = new UserSkill
        {
            Id = 1,
            Status = true,
            UserId = 3,
            SkillId = 3,
        };
        _context.UserSkill.Add(existingUserSkill);
        await _context.SaveChangesAsync();

        var userskillRepository = GetRepositoryInstance();

        // Act
        var result = await userskillRepository.GetUserSkillByIdAsync(existingSkillId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<UserSkill>();
        result!.Id.Should().Be(existingSkillId);
        result.Status.Should().Be(existingUserSkill.Status);
        result.UserId.Should().Be(existingUserSkill.UserId);
        result.SkillId.Should().Be(existingUserSkill.SkillId);

    }

    [Fact]
    public async Task GetUserSkillByIdAsync_WhenUserSkillDoesNotExist_ReturnsNull()
    {
        // Arrange
        var nonExistingSkillId = 999;
        var userskillRepository = GetRepositoryInstance();

        // Act
        var result = await userskillRepository.GetUserSkillByIdAsync(nonExistingSkillId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task EditUserSkillAsync_WhenUserSkillExists_EditsAndReturnsEditedUserSkill()
    {
        // Arrange
        var existingSkillId = 1;
        var existingUserSkill = new UserSkill
        {
            Id = 1,
            Status = true,
            UserId = 3,
            SkillId = 1,
        };
        _context.UserSkill.Add(existingUserSkill);
        await _context.SaveChangesAsync();

        var editedUserSkill = new UserSkill
        {
            Id = 1,
            Status = true,
            UserId = 3,
            SkillId = 3,
        };
        UserSkillRepository? userskillRepository = GetRepositoryInstance();

        // Act
        var result = await userskillRepository.EditUserSkillAsync(editedUserSkill);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<UserSkill>();
        result.Id.Should().Be(existingSkillId);
        result.Status.Should().Be(editedUserSkill.Status);
        result.UserId.Should().Be(editedUserSkill.UserId);

        // Additional Assertion: Ensure UserSkill is correctly edited in the database
        var editedUserSkillFromDb = await _context.UserSkill.FindAsync(existingSkillId);
        editedUserSkillFromDb.Should().NotBeNull();
        editedUserSkillFromDb!.Status.Should().Be(editedUserSkill.Status);
        editedUserSkillFromDb.UserId.Should().Be(editedUserSkill.UserId);
        editedUserSkillFromDb.SkillId.Should().Be(editedUserSkill.SkillId);

    }
}
