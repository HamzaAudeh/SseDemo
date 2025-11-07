using System.Runtime.CompilerServices;

namespace SseDemo;

public class EventService
{
    public async IAsyncEnumerable<string> GetMessages([EnumeratorCancellation] CancellationToken token)
    {
        var counter = 0;
        while (!token.IsCancellationRequested)
        {
            var message = $"data: Message {counter++} at {DateTime.Now}\n\n";
            yield return message;

            await Task.Delay(1000, token);
        }
    }
}
