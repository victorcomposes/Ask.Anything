using Microsoft.AspNetCore.SignalR;

namespace Ask.Anything.Api.SignalR;

public class SignalRHubFilter : IHubFilter
{
    private readonly ILogger<VictorComposesHub> _logger;

    public SignalRHubFilter(ILogger<VictorComposesHub> logger)
    {
        _logger = logger;
    }

    public async ValueTask<object> InvokeMethodAsync(
        HubInvocationContext invocationContext,
        Func<HubInvocationContext, ValueTask<object>> next
    )
    {
        _logger.LogInformation(
            "Calling hub method '{HubMethodName}'",
            invocationContext.HubMethodName
        );
        try
        {
            return await next(invocationContext);
        }
        // This doesn't work for IAsyncEnumerable
        // TODO: Catch exceptions from IAsyncEnumerable
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception calling '{HubMethodName}': {ex}",
                invocationContext.HubMethodName,
                ex
            );
            throw;
        }
    }
}