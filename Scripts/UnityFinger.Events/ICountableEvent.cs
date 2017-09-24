namespace UnityFinger.Events
{
    interface ICountableEvent
    {
        int ListenersCount { get; }
    }
}
