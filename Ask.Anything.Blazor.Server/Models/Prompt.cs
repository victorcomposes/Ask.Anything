using Ask.Anything.Blazor.Server.Extensions;
using Ask.Anything.Blazor.Server.Models.Enums;

namespace Ask.Anything.Blazor.Server.Models;

public class Prompt
{

    public PromptType Type { get; set; }
    public string Title { get; set; }
    public string Icon { get; set; }
    public bool Selected { get; set; }

    public static Prompt Create(PromptType type, string icon)
    {
        return new Prompt
        {
            Type = type,
            Title = type.GetPromptDescription(),
            Selected = false,
            Icon = icon,
        };
    }
}