namespace AsteriskDotHMG.Common.Helpers;

public class BaseEvent : INotification
{
    public BaseEvent()
    {

    }

    public BaseEvent(string eventName)
    {
        EventName = eventName;
    }

    public string EventName { get; }
}
