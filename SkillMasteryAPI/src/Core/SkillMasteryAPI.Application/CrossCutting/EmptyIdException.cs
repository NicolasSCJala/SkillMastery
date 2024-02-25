namespace SkillMasteryAPI.Application.CrossCutting;

public class EmptyIdException : ArgumentException
{
    public EmptyIdException(string message) : base(message)
    {
    }

    public static void ThrowIfIdZero(
        int id, string? message = null, string? elementName = null)
    {
        if(id == 0)
        {
            throw new EmptyIdException(message ?? $"{elementName} Id cannot be empty");
        }
    }
}
