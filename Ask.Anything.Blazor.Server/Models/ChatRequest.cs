using Ask.Anything.Blazor.Server.Models.Enums;

namespace Ask.Anything.Blazor.Server.Models;

public class ChatRequest
{
    public IEnumerable<Message> Messages { get; set; }
    public PromptType PromptType { get; set; }
}