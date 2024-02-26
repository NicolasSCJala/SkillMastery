namespace SkillMasteryAPI.Domain.Models
{
    public class Dificulty: BaseModel

    {
        public required int Value { get; set; }
        public ICollection<Skill> Skill { get; set; } = new List<Skill>();


    }
}