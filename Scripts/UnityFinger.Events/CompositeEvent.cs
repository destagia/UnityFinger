namespace UnityFinger.Events
{
    class CompositeEvent : ICountableEvent
    {
        readonly ICountableEvent[] events;

        public CompositeEvent(params ICountableEvent[] events)
        {
            this.events = events;
        }

        public int ListenersCount {
            get {
                var sum = 0;
                foreach (var e in events) {
                    sum += e.ListenersCount;
                }
                return sum;
            }
        }
    }
}
