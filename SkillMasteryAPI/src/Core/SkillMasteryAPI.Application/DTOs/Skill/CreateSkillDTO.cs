using SkillMasteryAPI.Application.DTOs.Dificulty;


namespace SkillMasteryAPI.Application.DTOs.Skill
{
    public class CreateSkillDTO
    {

        public  string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DificultyId { get; set; }
        public DificultyDTO? Dificulty { get; set; }
    }
}
