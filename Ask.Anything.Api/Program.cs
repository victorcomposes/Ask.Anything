using Ask.Anything.Api;
using Ask.Anything.Api.Routes;
using Ask.Anything.Api.SignalR;
using OpenAI.Extensions;

var builder = WebApplication.CreateBuilder(args);

const string CorsPolicy = nameof(CorsPolicy);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenAIService();
builder.Services.AddWebApi(CorsPolicy);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(CorsPolicy);
app.UseHttpsRedirection();
app.MapAuthRoutes();
app.MapHub<VictorComposesHub>("/matrixhub");

app.UseAuthorization();

app.MapControllers();

app.Logger.LogInformation("Starting WebAPI");
app.Run();