using Ask.Anything.Blazor.Shared.Models.Enums;

namespace Ask.Anything.Blazor.Shared.Extensions;

public static class PromptExtensions
{
    public static string GetPromptDescription(this PromptType promptType)
    {
        return promptType.GetTitle();
    }

    public static string GetPromptMessage(this PromptType promptType, string messageFromUser)
    {
        return promptType.GetSystemMessage() + messageFromUser;
    }

    public static string GetPromptMessage(this PromptType promptType)
    {
        return promptType.GetSystemMessage();
    }
}