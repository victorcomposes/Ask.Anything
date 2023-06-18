using Ask.Anything.Blazor.Server.Models;
using OpenAI.ObjectModels.ResponseModels;

namespace Ask.Anything.Blazor.Server.Services;

public interface IAskAnythingService
{
    IAsyncEnumerable<ChatCompletionCreateResponse> CreateChatCompletionAsync(
        ChatRequest chatRequest,
        CancellationToken cancellationToken);
}