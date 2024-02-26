namespace SkillMasteryAPI.Application.CrossCutting;

public class FoundException : ArgumentException
{
    public FoundException(string? message = "Resource was found") : base(message) { }

    public static void ThrowIfNotNull<T>(T? param, string? message = null)
    {
        if(param is not null)
        {
            throw new FoundException(message);
        }
    }
}
