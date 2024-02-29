using Moq;
using NSubstitute;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

using SkillMasteryAPI.Domain.Models;
using SkillMasteryAPI.Application.Services
using SkillMasteryAPI.Infrastructure.Repositories.Interfaces;



namespace SkillMasteryAPI.Application.Tests.Services;

public class SkillServiceTests
{
    private readonly ISkillRepository _skillRepository;
    private readonly IMapper _mapper; //FALTA instalar

    private readonly Mock<ISkillRepository> _skillRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();

    public SkillServiceTests()
    {
        _skillRepository = Substitute.For<ISkillRepository>();
        _mapper = new Mapper();
    }

    private SkillService GetServiceInstance() => new(_skillRepository, _mapper);

    [Fact]
    public async Task GetAllSkillsAsync_ShouldReturnSkillDTOs()
    {
        // Arrange
        var skillService = GetServiceInstance();

        // Define some sample skills from the repository
        List<Skill> sampleSkills = new List<Skill>{
            new Skill
            {
                Id = 1,
                Name = "Bootcamp Developers 01",
                Description = "Skill covering concepts in software development.",
                CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
            },
            new Skill
            {
                Id = 2,
                Name = "Advanced Bootcamp Developers 01",
                Description = "Skill focused on advanced software design and development techniques.",
                CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
            },
            new Skill
            {
                Id = 3,
                Name = "Bootcamp Data Science and Analytics 01",
                Description = "Skill designed to teach the fundamentals of data analysis, machine learning, and statistical modeling.",
                CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
            },
            new Skill
            {
                Id = 4,
                Name = "Bootcamp Developers 02",
                Description = "Skill covering concepts in software development.",
                CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
            }
        };


        _skillRepository.GetAllSkillsAsync().Returns(Task.FromResult<IEnumerable<Skill>>(sampleSkills));

        // Act
        var result = await skillService.GetAllSkillsAsync();

        // Assert
        result.Should().NotBeNull(); // Ensure the result is not null

        // Use BeEquivalentTo to compare elements and properties without requiring exact type matching
        result.Should().BeEquivalentTo(sampleSkills.Select(skill => _mapper.Map<SkillDTO>(skill)));

        // Check if the number of items in the result matches the number of items in the sampleSkills
        result.Should().HaveCount(sampleSkills.Count);

        // Ensure that each item in the result is of the expected type SkillDTO
        result.Should().ContainItemsAssignableTo<SkillDTO>();
    }

    [Fact]
    public async Task CreateSkillAsync_ShouldReturnSkillDTO_WhenCreateIsSuccessful()
    {
        // Arrange
        var createSkillDTO = new CreateSkillDTO
        {
            Name = "Create Skill DTO Test",
            Description = "Create Skill DTO Test Description.",
        };

        var skill = new Skill
        {
            Id = 1,
            Name = "Skill Test",
            Description = "Skill Test Description.",
            CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
        };

        var skillDTO = new SkillDTO
        {
            Id = 2,
            Name = "Skill DTO Test",
            Description = "Skill DTO Test Description.",
        };

        _mapperMock.Setup(m => m.Map<Skill>(It.IsAny<CreateSkillDTO>())).Returns(skill);
        _skillRepositoryMock.Setup(repo => repo.CreateSkillAsync(It.IsAny<Skill>())).ReturnsAsync(skill);
        _mapperMock.Setup(m => m.Map<SkillDTO>(It.IsAny<Skill>())).Returns(skillDTO);

        var service = new SkillService(_skillRepositoryMock.Object, _classroomRepository, _mapperMock.Object);

        // Act
        var result = await service.CreateSkillAsync(createSkillDTO);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(skillDTO, result);
        Assert.Equal(skillDTO.Name, result.Name);
        Assert.Equal(skillDTO.Description, result.Description);
        _skillRepositoryMock.Verify(repo => repo.CreateSkillAsync(It.IsAny<Skill>()), Times.Once);
        _mapperMock.Verify(m => m.Map<SkillDTO>(It.IsAny<Skill>()), Times.Once);
    }

    [Fact]
    public async Task DeleteSkillAsync_ShouldReturnDeletedSkillDTO()
    {
        // Arrange
        SkillService skillService = GetServiceInstance();

        // Define a sample skill from the repository
        Skill sampleSkill = new Skill
        {
            Id = 1,
            Name = "Frontend Development",
            Description = "Skill to develop Web Applications focusing on HTML, CSS, JavaScript, and popular frameworks.",
            CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
        };

        _skillRepository.GetSkillByIdAsync(1).Returns(Task.FromResult<Skill?>(sampleSkill));
        _classroomRepository.GetClassroomsBySkillIdAsync(1).Returns(Task.FromResult<IEnumerable<Classroom>?>(null));

        _skillRepository.DeleteSkillAsync(sampleSkill).Returns(Task.FromResult(sampleSkill));

        // Act
        var result = await skillService.DeleteSkillAsync(1);

        // Assert
        result.Should().NotBeNull();

        result.Should().BeEquivalentTo(_mapper.Map<SkillDTO>(sampleSkill));
    }

    [Fact]
    public async Task DeleteSkillAsync_ShouldThrowNotFoundException_WhenSkillNotFound()
    {
        // Arrange
        SkillService skillService = GetServiceInstance();

        _skillRepository.GetSkillByIdAsync(1).Returns(Task.FromResult<Skill?>(null));

        // Act
        Func<Task> act = async () => await skillService.DeleteSkillAsync(1);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Skill with id 1 not found");
    }

    [Fact]
    public async Task DeleteSkillAsync_ShoulThrowFoundException_WhenSkillIsUsedInClassroom()
    {
        // Arrange
        SkillService skillService = GetServiceInstance();

        Skill sampleSkill = new Skill
        {
            Id = 1,
            Name = "Frontend Development",
            Description = "Skill to develop Web Applications focusing on HTML, CSS, JavaScript, and popular frameworks.",
            CreatedAt = new DateTime(2024, 1, 23, 0, 0, 0, DateTimeKind.Utc)
        };

        Classroom sampleClassroom = new Classroom
        {
            Id = 1,
            StartDate = new DateOnly(2024, 1, 23),
            SkillId = 1,
        };

        _skillRepository.GetSkillByIdAsync(1).Returns(Task.FromResult<Skill?>(sampleSkill));
        _classroomRepository.GetClassroomsBySkillIdAsync(1).Returns(Task.FromResult<IEnumerable<Classroom>?>(new List<Classroom> { sampleClassroom }));

        // Act
        Func<Task> act = async () => await skillService.DeleteSkillAsync(1);

        // Assert
        await act.Should().ThrowAsync<FoundException>().WithMessage("Skill with id 1 is used in a classroom");
    }

    [Fact]
    public async Task EditSkillAsync_ShouldReturnEditedSkillDTO()
    {
        //Arrange
        var skillService = GetServiceInstance();
        var initialSkill = new Skill
        {
            Id = 1,
            Name = "Initial Skill",
            Description = "Initial Skill Description"
        };

        _skillRepository.GetSkillByIdAsync(1).Returns(Task.FromResult<Skill?>(initialSkill));
        var updatedSkill = new Skill
        {
            Id = 1,
            Name = "Updated Skill",
            Description = "Updated Skill Description"
        };
        _skillRepository.EditSkillAsync(Arg.Any<Skill>()).Returns(Task.FromResult(updatedSkill));

        //Act   
        var result = await skillService.EditSkillAsync(_mapper.Map<SkillDTO>(updatedSkill));

        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_mapper.Map<SkillDTO>(updatedSkill));

    }
}
