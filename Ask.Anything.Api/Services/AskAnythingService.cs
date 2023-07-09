using System.Runtime.CompilerServices;
using Ask.Anything.Blazor.Shared.Extensions;
using Ask.Anything.Blazor.Shared.Models;
using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;
using OpenAI.Tokenizer.GPT3;

namespace Ask.Anything.Api.Services;

public class AskAnythingService : IAskAnythingService
{
    private readonly IOpenAIService _openAiService;

    public AskAnythingService(IOpenAIService openAiService)
    {
        _openAiService = openAiService;
    }

    public async IAsyncEnumerable<ChatCompletionCreateResponse> CreateChatCompletionAsync(
        ChatRequest chatRequest,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        string chatText = chatRequest.Messages.Aggregate("", (current, message) => current + message.Text);
        var maxTokens = TokenizerGpt3.TokenCount(text: chatText);
        var result = _openAiService.ChatCompletion
            .CreateCompletionAsStream(new ChatCompletionCreateRequest()
            {
                Messages = chatRequest.Messages.Select(CreateChatMessage).ToList(),
                MaxTokens = maxTokens,
                Temperature = chatRequest.PromptType.GetTemperature(),
                PresencePenalty = chatRequest.PromptType.GetPresencePenalty(),
                FrequencyPenalty = chatRequest.PromptType.GetFrequencyPenalty()
            }, OpenAI.ObjectModels.Models.ChatGpt3_5Turbo, cancellationToken);

        await foreach (var completion in result.WithCancellation(cancellationToken))
        {
            if (completion.Successful)
            {
                yield return completion;
            }
            else
            {
                if (completion.Error == null)
                {
                    throw new Exception("Unknown Error");
                }
                else
                {
                    throw new Exception(completion.Error.Message);
                }
            }
        }
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