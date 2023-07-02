using Ask.Anything.Blazor.Shared.Models.Enums;

namespace Ask.Anything.Blazor.Shared.Models;

public class ChatType
{
    public ChatType(PromptType systemPrompt)
    {
        SystemPrompt = systemPrompt;
    }

    public PromptType SystemPrompt { get; set; }
}