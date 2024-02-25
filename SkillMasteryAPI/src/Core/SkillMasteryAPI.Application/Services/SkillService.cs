using MapsterMapper;
using SkillMasteryAPI.Application.DTOs.Skill;
using SkillMasteryAPI.Application.Services.Interfaces;
using SkillMasteryAPI.Infrastructure.Repositories.Interfaces;
using SkillMasteryAPI.Domain.Models;
using SkillMasteryAPI.Application.CrossCutting;

namespace SkillMasteryAPI.Application.Services;

public class SkillService : ISkillService
{
    private readonly ISkillRepository _skillRepository;
    private readonly IMapper _mapper;

    public SkillService(ISkillRepository skillRepository, IMapper mapper)
    {
        _skillRepository = skillRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SkillDTO>> GetAllSkillsAsync()
    {
        var skills = await _skillRepository.GetAllSkillsAsync();
        return _mapper.Map<IEnumerable<SkillDTO>>(skills);
    }

    public async Task<SkillDTO> CreateSkillAsync(CreateSkillDTO createSkillDTO)
    {
        var skill = _mapper.Map<Skill>(createSkillDTO);
        var createdSkill = await _skillRepository.CreateSkillAsync(skill);
        return _mapper.Map<SkillDTO>(createdSkill);
    }

    async public Task<SkillDTO?> DeleteSkillAsync(int id)
    {
        var skill = await _skillRepository.GetSkillByIdAsync(id);
        NotFoundException.ThrowIfNull(skill, $"Skill with id {id} not found");


        var deletedSkill = await _skillRepository.DeleteSkillAsync(skill!);

        return _mapper.Map<SkillDTO>(deletedSkill);
    }

    public async Task<SkillDTO> EditSkillAsync(SkillDTO editSkillDTO)
    {
        var skill = _mapper.Map<Skill>(editSkillDTO);
        var editedSkill = await _skillRepository.EditSkillAsync(skill);
        return _mapper.Map<SkillDTO>(editedSkill);
    }
}
