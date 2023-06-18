using Ask.Anything.Blazor.Server.Extensions;
using Ask.Anything.Blazor.Server.Models;
using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;

namespace Ask.Anything.Blazor.Server.Services;

public class AskAnythingService : IAskAnythingService
{
    private readonly IOpenAIService _openAiService;

    public AskAnythingService(IOpenAIService openAiService)
    {
        _openAiService = openAiService;
    }

    public IAsyncEnumerable<ChatCompletionCreateResponse> CreateChatCompletionAsync(
        ChatRequest chatRequest,
        CancellationToken cancellationToken)
    {
        return _openAiService.ChatCompletion
            .CreateCompletionAsStream(new ChatCompletionCreateRequest()
            {
                Messages = chatRequest.Messages.Select(CreateChatMessage).ToList(),
                MaxTokens = chatRequest.PromptType.GetMaxTokens(),
                Temperature = chatRequest.PromptType.GetTemperature(),
                PresencePenalty = chatRequest.PromptType.GetPresencePenalty(),
                FrequencyPenalty = chatRequest.PromptType.GetFrequencyPenalty()
            }, OpenAI.ObjectModels.Models.ChatGpt3_5Turbo, cancellationToken);
    }

    private ChatMessage CreateChatMessage(Message message)
    {
        return message.Sender switch
        {
            Sender.USER => ChatMessage.FromUser(content: message.Text),
            Sender.AI => ChatMessage.FromAssistant(content: message.Text),
            Sender.SYSTEM => ChatMessage.FromSystem(content: message.Text),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}