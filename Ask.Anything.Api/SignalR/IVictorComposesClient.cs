namespace Ask.Anything.Api.SignalR;

public interface IVictorComposesClient
{
    Task ReceiveBroadcast(string user, string message);
}