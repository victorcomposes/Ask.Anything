using Ask.Anything.Blazor.Server.Models.Enums;

namespace Ask.Anything.Blazor.Server.Models;

public class ChatType
{
    public ChatType(PromptType systemPrompt)
    {
        SystemPrompt = systemPrompt;
    }

    public PromptType SystemPrompt { get; set; }
}