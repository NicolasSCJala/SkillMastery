using Moq;
using NSubstitute;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

using SkillMasteryAPI.Domain.Models;
using SkillMasteryAPI.Infrastructure.Repositories.Interfaces;
using SkillMasteryAPI.Application.DTOs.User;
using SkillMasteryAPI.Application.Services;
using SkillMasteryAPI.Application.CrossCutting;


namespace SkillMasteryAPI.Application.Tests.Services;

public class UserServiceTests
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();

    public UserServiceTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _mapper = new Mapper();
    }

    private UserService GetServiceInstance() => new(_userRepository, _mapper);

    [Fact]
    public async Task GetAllUsersAsync_ShouldReturnUserDTOs()
    {
        // Arrange
        var userService = GetServiceInstance();

        // Define some sample users from the repository
        List<User> sampleUsers = new List<User>{
            new User
            {
                Id = 1,
                FirstName = "random name",
                LastName =  "random last name",
                Email = "random email",
                CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                 Id = 2,
                FirstName = "random name",
                LastName =  "random last name",
                Email = "random email",
                CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = 3,
                FirstName = "random name",
                LastName =  "random last name",
                Email = "random email",
                CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
            }


        };


        _userRepository.GetAllUsersAsync().Returns(Task.FromResult<IEnumerable<User>>(sampleUsers));

        // Act
        var result = await userService.GetAllUsersAsync();

        // Assert
        result.Should().NotBeNull(); // Ensure the result is not null

        // Use BeEquivalentTo to compare elements and properties without requiring exact type matching
        result.Should().BeEquivalentTo(sampleUsers.Select(user => _mapper.Map<UserDTO>(user)));

        // Check if the number of items in the result matches the number of items in the sampleUsers
        result.Should().HaveCount(sampleUsers.Count);

        // Ensure that each item in the result is of the expected type UserDTO
        result.Should().ContainItemsAssignableTo<UserDTO>();
    }

    [Fact]
    public async Task CreateUserAsync_ShouldReturnUserDTO_WhenCreateIsSuccessful()
    {
        // Arrange
        var createUserDTO = new CreateUserDTO
        {
            FirstName = "random name",
            LastName = "random last name",
            Email = "random email",
        };

        var user = new User
        {
            Id = 2,
            FirstName = "random name",
            LastName = "random last name",
            Email = "random email",
            CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
        };

        var userDTO = new UserDTO
        {
            Id = 3,
            FirstName = "random name",
            LastName = "random last name",
            Email = "random email",
        };

        _mapperMock.Setup(m => m.Map<User>(It.IsAny<CreateUserDTO>())).Returns(user);
        _userRepositoryMock.Setup(repo => repo.CreateUserAsync(It.IsAny<User>())).ReturnsAsync(user);
        _mapperMock.Setup(m => m.Map<UserDTO>(It.IsAny<User>())).Returns(userDTO);

        var service = new UserService(_userRepositoryMock.Object, _mapperMock.Object);

        // Act
        var result = await service.CreateUserAsync(createUserDTO);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userDTO, result);
        Assert.Equal(userDTO.FirstName, result.FirstName);
        Assert.Equal(userDTO.LastName, result.LastName);
        Assert.Equal(userDTO.Email, result.Email);

        _userRepositoryMock.Verify(repo => repo.CreateUserAsync(It.IsAny<User>()), Times.Once);
        _mapperMock.Verify(m => m.Map<UserDTO>(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldReturnDeletedUserDTO()
    {
        // Arrange
        UserService userService = GetServiceInstance();

        // Define a sample user from the repository
        User sampleUser = new User
        {
            Id = 2,
            FirstName = "random name",
            LastName = "random last name",
            Email = "random email",
            CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
        };

        _userRepository.GetUserByIdAsync(1).Returns(Task.FromResult<User?>(sampleUser));

        _userRepository.DeleteUserAsync(sampleUser).Returns(Task.FromResult(sampleUser));

        // Act
        var result = await userService.DeleteUserAsync(1);

        // Assert
        result.Should().NotBeNull();

        result.Should().BeEquivalentTo(_mapper.Map<UserDTO>(sampleUser));
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldThrowNotFoundException_WhenUserNotFound()
    {
        // Arrange
        UserService userService = GetServiceInstance();

        _userRepository.GetUserByIdAsync(1).Returns(Task.FromResult<User?>(null));

        // Act
        Func<Task> act = async () => await userService.DeleteUserAsync(1);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage("User with id 1 not found");
    }

    
    [Fact]
    public async Task EditUserAsync_ShouldReturnEditedUserDTO()
    {
        //Arrange
        var userService = GetServiceInstance();
        var initialUser = new User
        {
            Id = 2,
            FirstName = "random name",
            LastName = "random last name",
            Email = "random email",

        };

        _userRepository.GetUserByIdAsync(1).Returns(Task.FromResult<User?>(initialUser));
        var updatedUser = new User
        {
            Id = 2,
            FirstName = "random name",
            LastName = "random last name",
            Email = "random email",
        };
        _userRepository.EditUserAsync(Arg.Any<User>()).Returns(Task.FromResult(updatedUser));

        //Act   
        var result = await userService.EditUserAsync(_mapper.Map<UserDTO>(updatedUser));

        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_mapper.Map<UserDTO>(updatedUser));

    }
}
