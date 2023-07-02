using System.Diagnostics;
using System.Runtime.CompilerServices;
using Ask.Anything.Blazor.Shared.Models;
using Ask.Anything.Blazor.Shared.Models.Enums;
using Ask.Anything.Blazor.WASM.Models;
using Ask.Anything.Blazor.WASM.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;

namespace Ask.Anything.Blazor.WASM;

public class SignalRClient
{
    private readonly StateData _stateData;
    private readonly NotifierService _notifierService;
    private readonly HubConnection _connection;
    private readonly ILogger<SignalRClient> _logger;

    public SignalRClient(
        StateData stateData,
        IWebAssemblyHostEnvironment hostEnvironment,
        NotifierService notifierService,
        ILogger<SignalRClient> logger)
    {
        _stateData = stateData;
        _notifierService = notifierService;
        _logger = logger;
        var hubeBaseUrl = hostEnvironment.IsDevelopment()
            ? "https://localhost:7074"
            : "https://victorcomposes.azurewebsites.net";
        var hubUrl = $"{hubeBaseUrl}/matrixhub";
        _connection = new HubConnectionBuilder().WithUrl(hubUrl).WithAutomaticReconnect().Build();
        RegisterHandlers();
        _connection.Closed += async (exception) =>
        {
            if (exception != null)
            {
                _logger.LogInformation("Connection closed due to an error: {Exception}", exception);
            }
        };
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _connection.StartAsync(cancellationToken);
    }

    public enum StatusHubConnectionState
    {
        Disconnected,
        Connected,
        Connecting,
        Reconnecting
    }

    public StatusHubConnectionState GetConnectionState()
    {
        StatusHubConnectionState state;
        state = _connection.State switch
        {
            HubConnectionState.Disconnected => StatusHubConnectionState.Disconnected,
            HubConnectionState.Connected => StatusHubConnectionState.Connected,
            HubConnectionState.Connecting => StatusHubConnectionState.Connecting,
            HubConnectionState.Reconnecting => StatusHubConnectionState.Reconnecting,
            _ => throw new ArgumentOutOfRangeException()
        };
        return state;
    }

    public async Task BroadcastMessageAsync(string userName, string message)
    {
        await _connection.InvokeAsync("BroadcastMessage", userName, message);
    }

    public async IAsyncEnumerable<ChatCompletionCreateResponse?> CreateChatCompletionAsync(
        ChatRequest chatRequest,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        var completionResult = _connection.StreamAsync<ChatCompletionCreateResponse?>(
            "CreateChatCompletionAsync",
            chatRequest,
            cancellationToken
        );
        await foreach (var message in completionResult.WithCancellation(cancellationToken))
        {
            yield return message;
        }
    }

    //Methods that the client listens for
    private void RegisterHandlers()
    {
        _connection.On<string, string>(
            "ReceiveBroadcast",
            (user, message) =>
            {
                var encodedMsg = $"{user}: {message}";
                Console.WriteLine(encodedMsg);
            }
        );
        _connection.On<double>("ReceiveRateLimitedWarning", async (retryAfter) => { await _notifierService.RaiseRateLimited(retryAfter); });
    }
}