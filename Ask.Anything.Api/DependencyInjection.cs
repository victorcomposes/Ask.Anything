using Ask.Anything.Api.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging.ApplicationInsights;

namespace Ask.Anything.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApi(
        this IServiceCollection services,
        string corsPolicy
    )
    {
        services.AddSingleton<SignalRHubFilter>();

        services.AddSignalR(options =>
        {
            options.AddFilter<SignalRHubFilter>();
            options.EnableDetailedErrors = true;
        });

        services.AddEndpointsApiExplorer();

        if (Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING") == null)
        {
            services.AddLogging(options => options.AddConsole());
        }
        else
        {
            services.AddApplicationInsightsTelemetry();
            services.AddLogging(
                options =>
                    options
                        .AddApplicationInsights()
                        .AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Information)
            );
        }

        var productionCorsUrls = new string[]
        {
            "https://delightful-forest-00a30e703.2.azurestaticapps.net",
        };

        var developmentCorsUrls = new string[] { "https://localhost:7293" };

        services.AddCors(
            options =>
                options.AddPolicy(
                    name: corsPolicy,
                    policy =>
                        policy
                            .WithOrigins(
                                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                                == "Development"
                                    ? developmentCorsUrls
                                    : productionCorsUrls
                            )
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                )
        );
        return services;
    }
}