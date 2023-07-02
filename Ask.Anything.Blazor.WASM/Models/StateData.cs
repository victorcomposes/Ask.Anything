using Ask.Anything.Blazor.Shared.Models;
using Ask.Anything.Blazor.Shared.Models.Enums;

namespace Ask.Anything.Blazor.WASM.Models;

public class StateData
{
    public CancellationTokenSource CancellationTokenSource { get; set; } = new();
    public bool IsAwaitingResponse { get; set; }
    public bool IsAwaitingResponseStream { get; set; }
    public string NewMessageString { get; set; } = "";
    public List<Message> ChatMessages { get; set; } = new();
    public List<Message> CurrentMessageThread { get; set; } = new();
    public PromptType CurrentPromptType { get; set; } = new();
}