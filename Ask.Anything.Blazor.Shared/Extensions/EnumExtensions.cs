using Ask.Anything.Blazor.Shared.Attributes;
using Ask.Anything.Blazor.Shared.Models;

namespace Ask.Anything.Blazor.Shared.Extensions;

public static class EnumExtensions
{
    public static string GetTitle<T>(this T enumerationValue) where T : Enum
    {
        var attribute = MyPromptAttributes(enumerationValue);

        return attribute?.Length > 0 ? attribute[0].Title : enumerationValue.ToString();
    }

    public static string GetId<T>(this T enumerationValue) where T : Enum
    {
        var attribute = MyPromptAttributes(enumerationValue);

        return attribute?.Length > 0 ? attribute[0].Id : enumerationValue.ToString();
    }

    public static int GetMaxTokens<T>(this T enumerationValue) where T : Enum
    {
        var attribute = MyPromptAttributes(enumerationValue);

        return attribute?.Length > 0 ? attribute[0].MaxTokens : throw new Exception("Max Token is required");
    }

    public static float GetTemperature<T>(this T enumerationValue) where T : Enum
    {
        var attribute = MyPromptAttributes(enumerationValue);

        return attribute?.Length > 0 ? attribute[0].Temperature : throw new Exception("Temperature is required");
    }

    public static float GetFrequencyPenalty<T>(this T enumerationValue) where T : Enum
    {
        var attribute = MyPromptAttributes(enumerationValue);

        return attribute?.Length > 0 ? attribute[0].FrequencyPenalty : throw new Exception("Frequency Penalty is required");
    }

    public static float GetPresencePenalty<T>(this T enumerationValue) where T : Enum
    {
        var attribute = MyPromptAttributes(enumerationValue);

        return attribute?.Length > 0 ? attribute[0].PresencePenalty : throw new Exception("Presence Penalty is required");
    }

    public static string GetSystemMessage<T>(this T enumerationValue) where T : Enum
    {
        var attribute = MyPromptAttributes(enumerationValue);

        return attribute?.Length > 0 ? attribute[0].SystemMessage : throw new Exception("System Message is required");
    }

    private static MyPromptAttribute[] MyPromptAttributes<T>(T enumerationValue) where T : Enum
    {
        var attribute = enumerationValue.GetType()
                .GetField(enumerationValue.ToString())!
                .GetCustomAttributes(typeof(MyPromptAttribute), false)
            as MyPromptAttribute[];
        return attribute ?? Array.Empty<MyPromptAttribute>();
    }

    public static string GetDescription(this Sender sender)
    {
        switch (sender)
        {
            case Sender.USER:
                return "user";
            case Sender.AI:
                return "assistant";
            case Sender.SYSTEM:
                return "system";
            default:
                throw new ArgumentOutOfRangeException(nameof(sender), sender, null);
        }
    }
}