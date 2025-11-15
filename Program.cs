using SseDemo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<EventService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
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

app.Run();
