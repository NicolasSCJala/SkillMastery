namespace SkillMasteryAPI.Domain.Models
{
    public class Skill : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int DificultyId { get; set; }
        public Dificulty? Dificulty { get; set; }
        public ICollection<UserSkill> UserSkill { get; set; } = new List<UserSkill>();

    }
}
