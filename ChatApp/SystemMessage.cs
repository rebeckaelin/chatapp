namespace ChatApp;

public class SystemMessage :BaseMessage
{
    public string Event { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;

    public override string FormatDisplay()
    {
        var action = Event == "join" ? "joined" : "left";
        return $"[{Timestamp}] [System] {Username} has {action} the chat";
    }
}