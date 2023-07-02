using Markdig;
using Markdig.Extensions.AutoLinks;

namespace Ask.Anything.Blazor.WASM.Services;

public class MarkdigPipelineService
{
    public MarkdownPipeline Pipeline { get; } = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .UsePipeTables()
        .UseAutoLinks(new AutoLinkOptions { OpenInNewWindow = true, UseHttpsForWWWLinks = true })
        .UseEmojiAndSmiley()
        .Build();
}