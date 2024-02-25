namespace SkillMasteryAPI.Domain.Models
{
    public class Goal : BaseModel
    {
        public string Name { get; set; } = string.Empty;

        public DateOnly Finish_Date { get; set; }

        public int UserSkillId { get; set; }
        public UserSkill? UserSkill { get; set; }
    }
}