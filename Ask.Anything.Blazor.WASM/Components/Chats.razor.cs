using Ask.Anything.Blazor.Shared.Extensions;
using Ask.Anything.Blazor.Shared.Models;
using Ask.Anything.Blazor.Shared.Models.Enums;
using Ask.Anything.Blazor.WASM.Models;
using Ask.Anything.Blazor.WASM.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;
using MudExtensions;

namespace Ask.Anything.Blazor.WASM.Components;

public class ChatsBase : ComponentBase, IDisposable
{
    [Inject] protected StateData StateData { get; set; } = default!;
    [Inject] protected NotifierService NotifierService { get; set; } = default!;
    [Inject] protected SignalRClient SignalR { get; set; } = default!;
    [Inject] protected ISnackbar Snackbar { get; set; } = default!;

    [CascadingParameter] protected MudTheme Theme { get; set; }
    [CascadingParameter] protected bool isDarkMode { get; set; }


    public MudTextFieldExtended<string> multilineReference;

    protected override async Task OnInitializedAsync()
    {
        NotifierService.Notify += OnNotify;
        NotifierService.CancelMessageStreamEvent += OnCancelMessageStream;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
        }
    }

    public async Task CustomPromptSelected(PromptType selectedPrompt)
    {
        StateData.NewMessageString = selectedPrompt.GetSystemMessage();
        StateData.CurrentPromptType = selectedPrompt;
        await multilineReference.FocusAsync();
    }

    public async Task ClearChat()
    {
        StateData.CurrentMessageThread.Clear();
        StateData.NewMessageString = "";
        StateData.CurrentPromptType = PromptType.Assistant;
        await multilineReference.FocusAsync();
    }

    protected async Task SendMessage()
    {
        if (string.IsNullOrWhiteSpace(StateData.NewMessageString) || !await CheckConnection())
        {
            return;
        }

        var newChatMessage = new Message(StateData.NewMessageString, Sender.USER);
        var newAssistantMessage = new Message(string.Empty, Sender.AI);


        StateData.CurrentMessageThread.Add(newChatMessage);
        StateData.CurrentMessageThread.Add(newAssistantMessage);
        StateData.NewMessageString = string.Empty;

        try
        {
            await SendMessage(newChatMessage, newAssistantMessage);
        }
        catch (Exception e)
        {
            Snackbar.Add(e.Message, Severity.Error);
            StateData.IsAwaitingResponseStream = false;
            StateData.IsAwaitingResponse = false;
            StateData.CancellationTokenSource.Dispose();
            StateData.CancellationTokenSource = new CancellationTokenSource();
            if (StateData.CurrentMessageThread.Any() && StateData.CurrentMessageThread.Last().Sender == Sender.AI)
            {
                var removeAIMessage = StateData.CurrentMessageThread.Last();
                StateData.CurrentMessageThread.Remove(removeAIMessage);
                var removeUserMessage = StateData.CurrentMessageThread.Last();
                StateData.CurrentMessageThread.Remove(removeUserMessage);
            }
        }
    }

    private async Task SendMessage(Message chatMessage, Message assistantMessage)
    {

        StateData.IsAwaitingResponseStream = true;
        StateData.IsAwaitingResponse = true;

        StateHasChanged();

        var resultStream = SignalR.CreateChatCompletionAsync(
            new ChatRequest()
            {
                Messages = StateData.CurrentMessageThread.ToList(),
                PromptType = PromptType.Assistant
            },
            StateData.CancellationTokenSource.Token);

        await foreach (var result in resultStream.WithCancellation(StateData.CancellationTokenSource.Token))
        {
            StateData.IsAwaitingResponseStream = false;
            assistantMessage.Text += result?.Choices[0].Message?.Content;

            StateHasChanged();
            _ = NotifierService.Update();
        }

        StateData.IsAwaitingResponseStream = false;
        StateData.IsAwaitingResponse = false;
        StateData.CancellationTokenSource.Dispose();
        StateData.CancellationTokenSource = new CancellationTokenSource();
    }

    private async Task<bool> CheckConnection()
    {
        if (SignalR.GetConnectionState() == SignalRClient.StatusHubConnectionState.Disconnected)
        {
            try
            {
                await SignalR.StartAsync(StateData.CancellationTokenSource.Token);
            }
            catch (HttpRequestException e)
            {
                Snackbar.Add("Unable to connect to Ask Anything", Severity.Error);
                return false;
            }
        }

        return true;
    }

    public void Dispose()
    {
        NotifierService.Notify -= OnNotify;
        NotifierService.CancelMessageStreamEvent -= OnCancelMessageStream;
    }

    protected async Task MessageTextFieldHandleEnterKey(KeyboardEventArgs args)
    {

        if (args is { Key: "Enter", ShiftKey: false })
        {
            await SendMessage();
        }
    }

    protected async Task CancelStreamingResponse()
    {
        StateData.CancellationTokenSource.Cancel();
        StateData.CancellationTokenSource.Dispose();

        var lastAssistantMessage = StateData.CurrentMessageThread.LastOrDefault(s => s.Sender == Sender.AI);
        if (lastAssistantMessage is not null && string.IsNullOrWhiteSpace(lastAssistantMessage?.Text))
        {
            StateData.CurrentMessageThread.Remove(lastAssistantMessage);
            StateData.ChatMessages.Remove(lastAssistantMessage);
        }

        StateData.IsAwaitingResponse = false;
        StateData.IsAwaitingResponseStream = false;
        StateData.CancellationTokenSource = new CancellationTokenSource();
    }

    private async Task OnNotify()
    {
        await InvokeAsync(StateHasChanged);
    }

    private async Task OnCancelMessageStream()
    {
        await InvokeAsync(CancelStreamingResponse);
    }
}