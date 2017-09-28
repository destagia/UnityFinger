using System.Collections.Generic;

namespace UnityFinger
{
    public interface IObserverFactory
    {
        int Priority { get; }

        IEnumerator<Result> GetObserver(IScreenInput input, IReadOnlyTimer timer);
    }
}

