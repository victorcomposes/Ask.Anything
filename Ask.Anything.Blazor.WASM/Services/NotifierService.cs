namespace Ask.Anything.Blazor.WASM.Services;

public class NotifierService
{

    //https://learn.microsoft.com/en-us/aspnet/core/blazor/components/?view=aspnetcore-7.0#invoke-component-methods-externally-to-update-state
    public static async Task Update()
    {
        if (Notify != null)
        {
            await Notify.Invoke();
        }
    }

    public static async Task CancelMessageStream()
    {
        if (CancelMessageStreamEvent != null)
        {
            await CancelMessageStreamEvent.Invoke();
        }
    }

    public async Task RaiseRateLimited(double retryAfter)
    {
        if (OnRateLimited != null)
        {
            await OnRateLimited.Invoke(retryAfter);
        }
    }

    public static event Func<Task>? Notify;
    public static event Func<Task>? CancelMessageStreamEvent;
    public event Func<double, Task>? OnRateLimited;
}