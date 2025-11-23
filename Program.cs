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

app.MapGet("/stocks", async (HttpContext context, EventService service) =>
{
    context.Response.Headers.Add("Content-Type", "text/event-stream");
    int? lastId = null;
    if (context.Request.Headers.TryGetValue("Last-Event-ID", out var val))
    {
        lastId = int.TryParse(val, out int parsed) ? parsed : null;
    }

    await foreach (var typedMessage in service.GetStockPrice(lastId, context.RequestAborted))
    {
        var json = JsonSerializer.Serialize(typedMessage);
        await context.Response.WriteAsync($"id: {typedMessage.Id}\n");
        await context.Response.WriteAsync($"data: {json}\n\n");
        await context.Response.Body.FlushAsync();
    }
})
.WithName("GetStocks");

app.MapGet("/stocks/specificEvent", async (HttpContext context, EventService service) =>
{
    context.Response.Headers.Add("Content-Type", "text/event-stream");
    int? lastId = null;
    if (context.Request.Headers.TryGetValue("Last-Event-ID", out var val))
    {
        lastId = int.TryParse(val, out int parsed) ? parsed : null;
        //you can use this to resume data sending from a specific place
    }

    await foreach (var typedMessage in service.GetStockPrice(lastId, context.RequestAborted))
    {
        var json = JsonSerializer.Serialize(typedMessage);
        await context.Response.WriteAsync($"id: {typedMessage.Id}\n");
        await context.Response.WriteAsync($"event: PriceChanged\n");
        await context.Response.WriteAsync($"data: {json}\n\n");
        await context.Response.Body.FlushAsync();
    }
})
.WithName("GetStocksWithSpecificEvent");


app.Run();
