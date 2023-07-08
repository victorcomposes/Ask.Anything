using Ask.Anything.Blazor.Shared.Models;
using OpenAI.ObjectModels.ResponseModels;

namespace Ask.Anything.Api.Services;

public interface IAskAnythingService
{
    IAsyncEnumerable<ChatCompletionCreateResponse> CreateChatCompletionAsync(
        ChatRequest chatRequest,
        CancellationToken cancellationToken);
}