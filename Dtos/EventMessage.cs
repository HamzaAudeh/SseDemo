namespace SseDemo.Dtos;

public class EventMessage
{
    public string Id { get; set; }
    public string Body { get; set; }

    public EventMessage(string id, string body)
    {
        Id = id;
        Body = body;
    }
}
