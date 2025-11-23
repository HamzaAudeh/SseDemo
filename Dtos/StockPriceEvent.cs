namespace SseDemo.Dtos;

public record StockPriceEvent(
    string Id,
    string Symbol,
    decimal Price,
    DateTime Timestamp
);
