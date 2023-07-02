namespace Ask.Anything.Blazor.Shared.Models;

public class Message
{
    public string Text { get; set; }
    public Sender Sender { get; set; }
    public DateTime TimeStamp { get; set; }

    public Message(string text, Sender sender)
    {
        Text = text;
        Sender = sender;
        TimeStamp = DateTime.Now;
    }

    public override string ToString()
    {
        return $"[{TimeStamp}] {Sender}: {Text}";
    }
}