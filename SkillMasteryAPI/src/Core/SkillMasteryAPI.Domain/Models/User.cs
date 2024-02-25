namespace SkillMasteryAPI.Domain.Models

{
    public class User: BaseModel
    {
        public required string FirstName { get; set; } = string.Empty;
        public required string LastName { get; set; } = string.Empty;
        public required string Email { get; set; } = string.Empty;
        public ICollection<UserSkill> UserSkill { get; set; } = new List<UserSkill>();

    }
}
