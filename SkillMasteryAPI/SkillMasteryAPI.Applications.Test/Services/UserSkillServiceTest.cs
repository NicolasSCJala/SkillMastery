using Moq;
using NSubstitute;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

using SkillMasteryAPI.Domain.Models;
using SkillMasteryAPI.Infrastructure.Repositories.Interfaces;
using SkillMasteryAPI.Application.DTOs.UserSkill;
using SkillMasteryAPI.Application.Services;
using SkillMasteryAPI.Application.CrossCutting;


namespace SkillMasteryAPI.Application.Tests.Services;

public class UserSkillServiceTests
{
    private readonly IUserSkillRepository _userskillRepository;
    private readonly IMapper _mapper;

    private readonly Mock<IUserSkillRepository> _userskillRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();

    public UserSkillServiceTests()
    {
        _userskillRepository = Substitute.For<IUserSkillRepository>();
        _mapper = new Mapper();
    }

    private UserSkillService GetServiceInstance() => new(_userskillRepository, _mapper);

    [Fact]
    public async Task GetAllUserSkillsAsync_ShouldReturnUserSkillDTOs()
    {
        // Arrange
        var userskillService = GetServiceInstance();

        // Define some sample userskills from the repository
        List<UserSkill> sampleUserSkills = new List<UserSkill>{
            new UserSkill
            {
                Id = 1,
                Status = true,
                SkillId = 1,
                UserId = 1,
                CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
            },
            new UserSkill
            {
                Id = 2,
                Status = true,
                SkillId = 2,
                UserId = 2,
                CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
            },
            new UserSkill
            {
              Id = 3,
                Status = true,
                SkillId = 3,
                UserId = 3,
                CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
            },
            new UserSkill
            {
                 Id = 4,
                Status = true,
                SkillId = 4,
                UserId = 4,
                CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
            }
        };


        _userskillRepository.GetAllUserSkillsAsync().Returns(Task.FromResult<IEnumerable<UserSkill>>(sampleUserSkills));

        // Act
        var result = await userskillService.GetAllUserSkillsAsync();

        // Assert
        result.Should().NotBeNull(); // Ensure the result is not null

        // Use BeEquivalentTo to compare elements and properties without requiring exact type matching
        result.Should().BeEquivalentTo(sampleUserSkills.Select(userskill => _mapper.Map<UserSkillDTO>(userskill)));

        // Check if the number of items in the result matches the number of items in the sampleUserSkills
        result.Should().HaveCount(sampleUserSkills.Count);

        // Ensure that each item in the result is of the expected type UserSkillDTO
        result.Should().ContainItemsAssignableTo<UserSkillDTO>();
    }

    [Fact]
    public async Task CreateUserSkillAsync_ShouldReturnUserSkillDTO_WhenCreateIsSuccessful()
    {
        // Arrange
        var createUserSkillDTO = new CreateUserSkillDTO
        {
            Name = "Create UserSkill DTO Test",
            Description = "Create UserSkill DTO Test Description.",
        };

        var userskill = new UserSkill
        {
            Id = 1,
            Status = true,
            SkillId = 1,
            UserId = 1,
            CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)

        };

        var userskillDTO = new UserSkillDTO
        {
            Id = 1,
            Status = true,
            SkillId = 1,
            UserId = 1,
        };

        _mapperMock.Setup(m => m.Map<UserSkill>(It.IsAny<CreateUserSkillDTO>())).Returns(userskill);
        _userskillRepositoryMock.Setup(repo => repo.CreateUserSkillAsync(It.IsAny<UserSkill>())).ReturnsAsync(userskill);
        _mapperMock.Setup(m => m.Map<UserSkillDTO>(It.IsAny<UserSkill>())).Returns(userskillDTO);

        var service = new UserSkillService(_userskillRepositoryMock.Object, _mapperMock.Object);

        // Act
        var result = await service.CreateUserSkillAsync(createUserSkillDTO);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userskillDTO, result);
        Assert.Equal(userskillDTO.Status, result.Status);
        Assert.Equal(userskillDTO.SkillId, result.SkillId);
        Assert.Equal(userskillDTO.UserId, result.UserId);

        _userskillRepositoryMock.Verify(repo => repo.CreateUserSkillAsync(It.IsAny<UserSkill>()), Times.Once);

        _mapperMock.Verify(m => m.Map<UserSkillDTO>(It.IsAny<UserSkill>()), Times.Once);
    }

    [Fact]
    public async Task DeleteUserSkillAsync_ShouldReturnDeletedUserSkillDTO()
    {
        // Arrange
        UserSkillService userskillService = GetServiceInstance();

        // Define a sample userskill from the repository
        UserSkill sampleUserSkill = new UserSkill
        {
            Id = 1,
            Status = true,
            SkillId = 1,
            UserId = 1,
            CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
        };

        _userskillRepository.GetUserSkillByIdAsync(1).Returns(Task.FromResult<UserSkill?>(sampleUserSkill));

        _userskillRepository.DeleteUserSkillAsync(sampleUserSkill).Returns(Task.FromResult(sampleUserSkill));

        // Act
        var result = await userskillService.DeleteUserSkillAsync(1);

        // Assert
        result.Should().NotBeNull();

        result.Should().BeEquivalentTo(_mapper.Map<UserSkillDTO>(sampleUserSkill));
    }

    [Fact]
    public async Task DeleteUserSkillAsync_ShouldThrowNotFoundException_WhenUserSkillNotFound()
    {
        // Arrange
        UserSkillService userskillService = GetServiceInstance();

        _userskillRepository.GetUserSkillByIdAsync(1).Returns(Task.FromResult<UserSkill?>(null));

        // Act
        Func<Task> act = async () => await userskillService.DeleteUserSkillAsync(1);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage("UserSkill with id 1 not found");
    }

    [Fact]
    public async Task DeleteUserSkillAsync_ShoulThrowFoundException_WhenUserSkillIsUsedInClassroom()
    {
        // Arrange
        UserSkillService userskillService = GetServiceInstance();

        UserSkill sampleUserSkill = new UserSkill
        {
            Id = 1,
            Status = true,
            SkillId = 1,
            UserId = 1,
            CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
        };

       
        _userskillRepository.GetUserSkillByIdAsync(1).Returns(Task.FromResult<UserSkill?>(sampleUserSkill));

        // Act
        Func<Task> act = async () => await userskillService.DeleteUserSkillAsync(1);

        // Assert
        await act.Should().ThrowAsync<FoundException>().WithMessage("UserSkill with id 1 is used in a classroom");
    }

    [Fact]
    public async Task EditUserSkillAsync_ShouldReturnEditedUserSkillDTO()
    {
        //Arrange
        var userskillService = GetServiceInstance();
        var initialUserSkill = new UserSkill
        {
            Id = 1,
            Status = true,
            SkillId = 1,
            UserId = 1,
        };

        _userskillRepository.GetUserSkillByIdAsync(1).Returns(Task.FromResult<UserSkill?>(initialUserSkill));
        var updatedUserSkill = new UserSkill
        {
            Id = 1,
            Status = false,
            SkillId = 1,
            UserId = 1,
        };
        _userskillRepository.EditUserSkillAsync(Arg.Any<UserSkill>()).Returns(Task.FromResult(updatedUserSkill));

        //Act   
        var result = await userskillService.EditUserSkillAsync(_mapper.Map<UserSkillDTO>(updatedUserSkill));

        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_mapper.Map<UserSkillDTO>(updatedUserSkill));

    }
}
