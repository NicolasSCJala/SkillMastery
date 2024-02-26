using SkillMasteryAPI.Application.DTOs.UserSkill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillMasteryAPI.Application.DTOs.Goal
{
    public class CreateGoalDTO
    {
        public string Name { get; set; } = string.Empty;

        public  DateOnly Finish_Date { get; set; }

        public  int UserSkillId { get; set; }
        public  UserSkillDTO? UserSkill { get; set; }
    }
}
