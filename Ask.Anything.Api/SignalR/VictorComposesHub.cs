using Ask.Anything.Blazor.Shared.Extensions;
using Ask.Anything.Blazor.Shared.Models;
using Ask.Anything.Blazor.Shared.Models.Enums;
using Microsoft.AspNetCore.SignalR;
using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;

namespace Ask.Anything.Api.SignalR;

public class VictorComposesHub : Hub<IVictorComposesClient>
{
    private readonly ILogger<VictorComposesHub> _logger;
    private readonly IOpenAIService _openAiService;

    public VictorComposesHub(ILogger<VictorComposesHub> logger, IOpenAIService openAiService)
    {
        _logger = logger;
        _openAiService = openAiService;
    }

    public override Task OnConnectedAsync()
    {
        _logger.LogInformation(message: "User connected: {User}", args: Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        if (exception is null)
        {
            _logger.LogInformation(message: "User disconnected: {User}", args: Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        _logger.LogInformation(
            message: "User disconnected: {User} with error: {Error}",
            args: new object?[]
            {
                Context.ConnectionId,
                exception.Message
            }
        );
        return base.OnConnectedAsync();
    }

    public async Task BroadcastMessage(string user, string message)
    {
        await Clients.All.ReceiveBroadcast(user: user, message: message);
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
            }, OpenAI.ObjectModels.Models.Gpt_4, cancellationToken);
    }

    private static ChatMessage CreateChatMessage(Message message)
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