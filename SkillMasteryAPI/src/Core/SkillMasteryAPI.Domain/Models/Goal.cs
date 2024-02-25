namespace SkillMasteryAPI.Domain.Models
{
    public class Goal : BaseModel
    {
        public required string Name { get; set; }

        public required DateOnly Finish_Date { get; set; }

        public required int UserSkillId { get; set; }
        public required UserSkill UserSkill { get; set; }

   





    }
}