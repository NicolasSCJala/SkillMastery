namespace SkillMasteryAPI.Application.CrossCutting;

public class NotFoundException : ArgumentException
{
    public NotFoundException(string? message = "Resource was not found") : base(message) { }

    public static void ThrowIfNull<T>(T? param, string? message = null)
    {
        if(param is null)
        {
            throw new NotFoundException(message);
        }
    }
}
