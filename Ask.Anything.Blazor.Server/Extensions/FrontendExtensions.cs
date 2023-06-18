﻿using Ask.Anything.Blazor.Server.Models.Enums;
using MudBlazor;

namespace Ask.Anything.Blazor.Server.Extensions;

public static class FrontendExtensions
{
    public static string GetPromptIcon(this PromptType promptType)
    {
        return promptType switch
        {
            PromptType.AdvancedRephraser => Icons.Material.Filled.GTranslate,
            PromptType.Poet => Icons.Material.Filled.MicExternalOn,
            PromptType.MotivationalSpeaker => Icons.Material.Filled.SpeakerGroup,
            PromptType.PetBehaviorist => Icons.Material.Filled.Pets,
            PromptType.Chef => Icons.Material.Filled.SetMeal,
            PromptType.FinancialAdvisor => Icons.Material.Filled.AttachMoney,
            PromptType.SelfHelpBook => Icons.Material.Filled.LiveHelp,
            PromptType.AphorismBook => Icons.Material.Filled.FormatQuote,
            PromptType.Mentor => Icons.Material.Filled.EscalatorWarning,
            PromptType.Accountant => Icons.Material.Filled.AccountBalance,
            PromptType.Stoic => Icons.Material.Filled.NordicWalking,
            PromptType.DatingCoach => Icons.Material.Filled.Campaign,
            PromptType.Researcher => Icons.Custom.Brands.Google,
            PromptType.Comedian => Icons.Material.Filled.EmojiEmotions,
            PromptType.PickUpLineGenerator => Icons.Material.Filled.Girl,
            PromptType.FootballSportsPunter => Icons.Material.Filled.SportsSoccer,
            PromptType.PromptGenerator => Icons.Material.Filled.GeneratingTokens,
            PromptType.QuoteGenerator => Icons.Material.Filled.FormatQuote,
            PromptType.MindMap => Icons.Material.Filled.Hub,
            PromptType.Essay => Icons.Material.Filled.Article,
            PromptType.CodeReviewer => Icons.Material.Filled.Code,
            PromptType.WealthManager => Icons.Material.Filled.AttachMoney,
            PromptType.SocraticPhilosopher => Icons.Material.Filled.Person2,
            PromptType.Assistant => Icons.Material.Filled.Public,
            PromptType.Etymologist => Icons.Material.Filled.Password,
            PromptType.LanguageLiteraryCritic => Icons.Material.Filled.Topic,
            PromptType.DanGpt => Icons.Material.Filled.Psychology,
            _ => throw new ArgumentException($"Invalid prompt type: {promptType}")
        };
    }
}