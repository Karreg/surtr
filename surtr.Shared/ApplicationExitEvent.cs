namespace surtr.Shared
{
    using Microsoft.Practices.Prism.PubSubEvents;

    /// <summary>
    /// Application exit event
    /// </summary>
    public class ApplicationExitEvent : PubSubEvent<string>
    {
    }
}
