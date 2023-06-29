namespace Ask.Anything.Blazor.Shared.Attributes;

public class MyPromptAttribute : Attribute
{
    public MyPromptAttribute(
        string title,
        float temperature,
        int maxTokens,
        float frequencyPenalty,
        float presencePenalty,
        string id,
        string systemMessage)
    {
        Title = title;
        Temperature = temperature;
        MaxTokens = maxTokens;
        FrequencyPenalty = frequencyPenalty;
        PresencePenalty = presencePenalty;
        Id = id;
        SystemMessage = systemMessage;
    }
    public string Title { get; set; }

    /// <summary>
    /// The number of text snippets to generate from the model in response to a single prompt.
    /// This is useful when you want to generate multiple suggestions or responses to a single input.
    /// </summary>
    public float Temperature { get; set; }

    /// <summary>
    /// The maximum number of tokens that the model should generate in response to a prompt.
    /// The model will stop generating tokens once this limit is reached.
    /// </summary>
    public int MaxTokens { get; set; }

    /// <summary>
    /// A value that discourages the model from generating the same token multiple times in the output.
    /// The higher the value, the less likely the model is to generate repeated tokens.
    /// </summary>
    public float FrequencyPenalty { get; set; }

    /// <summary>
    /// A value that discourages the model from generating tokens that were already present in the input text.
    /// The higher the value, the less likely the model is to generate repeated tokens.
    /// </summary>
    public float PresencePenalty { get; set; }
    public string Id { get; set; }
    public string SystemMessage { get; set; }
}