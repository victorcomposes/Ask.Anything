using Ask.Anything.Blazor.Shared.Models.Enums;

namespace Ask.Anything.Blazor.Shared.Models;

public class ChatRequest
{
    public IEnumerable<Message> Messages { get; set; }
    public PromptType PromptType { get; set; }
}