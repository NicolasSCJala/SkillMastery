using NSubstitute;
using FluentAssertions;

using SkillMasteryAPI.Domain.Models;
using SkillMasteryAPI.Infrastructure.Data;
using SkillMasteryAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace SkillMasteryAPI.Infraestructure.Tests.Repositories;

public class UserRepositoryTests : IDisposable
{
    private readonly SkillMasteryContext _context;

    public UserRepositoryTests()
    {
        DbContextOptionsBuilder<SkillMasteryContext> dbContextOptions = new DbContextOptionsBuilder<SkillMasteryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());

        _context = new SkillMasteryContext(dbContextOptions.Options);
    }
    private UserRepository GetRepositoryInstance() => new(_context);

    public void Dispose()
    {
        // Make sure that the in-memory database is deleted at the end of all tests.
        _context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetAllUsersAsync_ShouldReturnUsers()
    {
        // Arrange
        var users = new List<User>
        {
             new User
            {
                Id = 1,
                FirstName = "name3",
                LastName = "last3",
                Email = "ramdom3@ramdom.com",
                CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = 2,
                FirstName = "name2",
                LastName = "last2",
                Email = "ramdom2@ramdom.com",
                CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = 3,
                FirstName = "name1",
                LastName = "last1",
                Email = "ramdom@ramdom.com",
                CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
            }
        };

        _context.User.AddRange(users);
        await _context.SaveChangesAsync();

        var userRepository = GetRepositoryInstance();
        // Act
        var result = await userRepository.GetAllUsersAsync();

        // Assert
        result.Should().BeEquivalentTo(users);
        result.Should().HaveCount(users.Count);
        result.Should().Equal(users);
        result.First().FirstName.Should().Be(users.First().FirstName);
        result.First().LastName.Should().Be(users.First().LastName);
        result.First().Email.Should().Be(users.First().Email);

    }

    [Fact]
    public async Task CreateUserAsync_ShouldAddUser_WhenCalled()
    {
        // Arrange

        var repository = new UserRepository(_context);

        var newUser = new User { };

        // Act
        var result = await repository.CreateUserAsync(newUser);

        // Assert
        Assert.NotNull(result);
        var userInDb = await _context.User.FindAsync(result.Id);
        Assert.NotNull(userInDb);

    }

    [Fact]
    public async Task DeleteUserAsync_ShouldReturnDeletedUser()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            FirstName = "name1",
            LastName = "last1",
            Email = "ramdom@ramdom.com",
            CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
        };

        _context.User.Add(user);
        await _context.SaveChangesAsync();

        var repository = new UserRepository(_context);

        // Act
        var result = await repository.DeleteUserAsync(user);

        // Assert
        Assert.Equal(user, result);
    }

    [Fact]
    public async Task GetUserByIdAsync_WhenUserExists_ReturnsUser()
    {
        // Arrange
        var existingUserId = 1;
        var existingUser = new User
        {
            Id = 1,
            FirstName = "Ramdom Nombre",
            LastName = "Random Last name",
            Email = "Random email",
        };
        _context.User.Add(existingUser);
        await _context.SaveChangesAsync();

        var userRepository = GetRepositoryInstance();

        // Act
        var result = await userRepository.GetUserByIdAsync(existingUserId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<User>();
        result!.Id.Should().Be(existingUserId);
        result.FirstName.Should().Be(existingUser.FirstName);
        result.LastName.Should().Be(existingUser.LastName);
        result.Email.Should().Be(existingUser.Email);

    }

    [Fact]
    public async Task GetUserByIdAsync_WhenUserDoesNotExist_ReturnsNull()
    {
        // Arrange
        var nonExistingUserId = 999;
        var userRepository = GetRepositoryInstance();

        // Act
        var result = await userRepository.GetUserByIdAsync(nonExistingUserId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task EditUserAsync_WhenUserExists_EditsAndReturnsEditedUser()
    {
        // Arrange
        var existingUserId = 1;
        var existingUser = new User
        {
            Id = 1,
            FirstName = "Ramdom Nombre",
            LastName = "Random Last name",
            Email = "Random email",
        };
        _context.User.Add(existingUser);
        await _context.SaveChangesAsync();

        var editedUser = new User
        {
            Id = 1,
            FirstName = "Ramdom Nombre",
            LastName = "Random Last name",
            Email = "Random email",
        };
        UserRepository? userRepository = GetRepositoryInstance();

        // Act
        var result = await userRepository.EditUserAsync(editedUser);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<User>();
        result.Id.Should().Be(existingUserId);
        result.FirstName.Should().Be(editedUser.FirstName);
        result.LastName.Should().Be(editedUser.LastName);

        // Additional Assertion: Ensure User is correctly edited in the database
        var editedUserFromDb = await _context.User.FindAsync(existingUserId);
        editedUserFromDb.Should().NotBeNull();
        editedUserFromDb!.FirstName.Should().Be(editedUser.FirstName);
        editedUserFromDb.LastName.Should().Be(editedUser.LastName);
        editedUserFromDb.Email.Should().Be(editedUser.Email);

    }
}
