using SseDemo;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<EventService>();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("Allow", policy =>
        {
            policy.WithOrigins("*")
               .AllowAnyHeader()
               .AllowAnyMethod();
        });
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseCors("Allow");
}

app.UseHttpsRedirection();


app.MapGet("/summary", async (HttpContext context, EventService service) =>
{
    context.Response.Headers.Add("Content-Type", "text/event-stream");

    await foreach (var message in service.GetMessages(context.RequestAborted))
    {
        await context.Response.WriteAsync(message);
        await context.Response.Body.FlushAsync();
    }
})
.WithName("GetSummary");

app.MapGet("/typedSummary", async (HttpContext context, EventService service) =>
{
    context.Response.Headers.Add("Content-Type", "text/event-stream");

    await foreach (var typedMessage in service.GetTypedMessages(context.RequestAborted))
    {
        var json = JsonSerializer.Serialize(typedMessage);
        await context.Response.WriteAsync($"data: {json} at {DateTime.Now}\n\n");
        await context.Response.Body.FlushAsync();
    }
})
.WithName("GetTypedSummary");

app.Run();
