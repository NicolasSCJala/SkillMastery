using SkillMasteryAPI.Application.DTOs.Skill;
using SkillMasteryAPI.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillMasteryAPI.Application.DTOs.UserSkill
{
    public class CreateUserSkillDTO
    {
        public bool Status { get; set; } = false;

        public int SkillId { get; set; }
        public SkillDTO? Skill { get; set; }

        public int UserId { get; set; }
        public UserDTO? User { get; set; }
    }
}
