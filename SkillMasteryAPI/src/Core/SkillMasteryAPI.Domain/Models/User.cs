namespace SkillMasteryAPI.Domain.Models

{
    public class User: BaseModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ICollection<UserSkill> UserSkill { get; set; } = new List<UserSkill>();

    }
}
