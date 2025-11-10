namespace ChatApp;

public class ChatMessage : BaseMessage
{
    public string UserName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public override string FormatDisplay()
    {
        return $"[{Timestamp}] {UserName}: {Message}";
    }
}