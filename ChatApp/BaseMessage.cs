namespace ChatApp;

public abstract class BaseMessage
{
    public string Timestamp { get; set; } = DateTime.Now.ToString("HH:mm");

    public abstract string FormatDisplay();
}