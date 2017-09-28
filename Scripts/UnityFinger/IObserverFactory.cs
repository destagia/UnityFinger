using System.Collections.Generic;

namespace UnityFinger
{
    public interface IObserverFactory
    {
        int Priority { get; }

        IEnumerator<Observation> GetObserver(IScreenInput input, IReadOnlyTimer timer);
    }
}

