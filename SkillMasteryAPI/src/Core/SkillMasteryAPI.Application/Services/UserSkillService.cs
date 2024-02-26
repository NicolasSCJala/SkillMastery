using MapsterMapper;
using SkillMasteryAPI.Application.DTOs.UserSkill;
using SkillMasteryAPI.Application.Services.Interfaces;
using SkillMasteryAPI.Infrastructure.Repositories.Interfaces;
using SkillMasteryAPI.Domain.Models;
using SkillMasteryAPI.Application.CrossCutting;

namespace SkillMasteryAPI.Application.Services;

public class UserSkillService : IUserSkillService
{
    private readonly IUserSkillRepository _userskillRepository;
    private readonly IMapper _mapper;

    public UserSkillService(IUserSkillRepository userskillRepository, IMapper mapper)
    {
        _userskillRepository = userskillRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserSkillDTO>> GetAllUserSkillsAsync()
    {
        var userskills = await _userskillRepository.GetAllUserSkillsAsync();
        return _mapper.Map<IEnumerable<UserSkillDTO>>(userskills);
    }

    public async Task<UserSkillDTO> CreateUserSkillAsync(CreateUserSkillDTO createUserSkillDTO)
    {
        var userskill = _mapper.Map<UserSkill>(createUserSkillDTO);
        var createdUserSkill = await _userskillRepository.CreateUserSkillAsync(userskill);
        return _mapper.Map<UserSkillDTO>(createdUserSkill);
    }

    async public Task<UserSkillDTO?> DeleteUserSkillAsync(int id)
    {
        var userskill = await _userskillRepository.GetUserSkillByIdAsync(id);
        NotFoundException.ThrowIfNull(userskill, $"UserSkill with id {id} not found");


        var deletedUserSkill = await _userskillRepository.DeleteUserSkillAsync(userskill!);

        return _mapper.Map<UserSkillDTO>(deletedUserSkill);
    }

    public async Task<UserSkillDTO> EditUserSkillAsync(UserSkillDTO editUserSkillDTO)
    {
        var userskill = _mapper.Map<UserSkill>(editUserSkillDTO);
        var editedUserSkill = await _userskillRepository.EditUserSkillAsync(userskill);
        return _mapper.Map<UserSkillDTO>(editedUserSkill);
    }
}
