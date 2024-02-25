
namespace SkillMasteryAPI.Domain.Models
{
    public class UserSkill: BaseModel
    {
        public required bool Status { get; set; } = false;

        public int SkillId { get; set; }
        public  Skill? Skill { get; set; } 

        public int UserId { get; set; }
        public  User? User { get; set; }
        public ICollection<Goal> Goal { get; set; } = new List<Goal>();

    }
}