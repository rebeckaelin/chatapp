namespace ChatApp;

public abstract class BaseMessage
{
    public string Timestamp { get; set; } = DateTime.Now.ToString("HH:mm");

    public abstract string FormatDisplay();

    // public virtual string GetMessageType()
    // {
    //     return this.GetType().Name;
    // }
}