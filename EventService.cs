using SseDemo.Dtos;
using System.Runtime.CompilerServices;

namespace SseDemo;

public class EventService
{
    public async IAsyncEnumerable<StockPriceEvent> GetStockPrice(int? lastId, [EnumeratorCancellation] CancellationToken token)
    {
        var id = 0;
        if (lastId is not null)
        {
            id = lastId.Value;
        }
        while (!token.IsCancellationRequested)
        {
            try
            {
                /*
                 * In order to this to work perfectly with browseres, the message format must be as the following:
                   The "data" keyword must be present, it represents the actual data payload.
                   The message must end with \n\n to indicate a message end
                 * The "event" keyword is optional and can be used to specify a custom event type.
                   If ommited, the client listens to all events using onmessage handler.
                 * The "id" keyword is optional and can be used to specify a unique identifier for the event.
                   If the connection is lost, the client can use this id passed in header as "Last-Event-ID" to resume from the last received message.
                 * The "retry" keyword is optional and can be used to specify the reconnection time in milliseconds if the connection is lost.
                   The browser will wait this milliseconds before attempting to reconnect.
                 * Comments can be added using the ":" prefix, they are ignored by the client and might be used to send
                   keep-alive signals to the client to prevent connection timeouts.
                */
                var symbols = new[] { "A", "B", "C", "D" };

                var rnd = new Random();
                var stock = new StockPriceEvent(
                    id.ToString(),
                    symbols[Random.Shared.Next(symbols.Length)],
                    Math.Round(100 + (decimal)rnd.NextDouble() * 10, 2),
                    DateTime.UtcNow
                    );

                yield return stock;
                await Task.Delay(1000, token);
                id++;
            }
            finally
            {
                //client closes connection
            }
        }
    }
}
