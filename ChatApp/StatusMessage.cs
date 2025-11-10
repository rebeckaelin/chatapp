namespace ChatApp;

public class StatusMessage : BaseMessage
{
    public string Status { get; set; } = string.Empty;
    
    public override string FormatDisplay()
    {
        return $"[{Timestamp}] [System] {Status}";
    }
}