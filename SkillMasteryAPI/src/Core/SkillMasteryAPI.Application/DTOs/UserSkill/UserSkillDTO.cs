using SkillMasteryAPI.Application.DTOs.Dificulty;
using SkillMasteryAPI.Application.DTOs.Skill;
using SkillMasteryAPI.Application.DTOs.User;

namespace SkillMasteryAPI.Application.DTOs.UserSkill
{
    public class UserSkillDTO
    {
        public int Id { get; set; }

        public bool Status { get; set; } = false;

        public int SkillId { get; set; }
        public SkillDTO? Skill { get; set; }

        public int UserId { get; set; }
        public UserDTO? User { get; set; }

    }
}
