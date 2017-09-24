using System.Collections.Generic;

namespace UnityFinger
{
    public interface IObserver
    {
        int Priority { get; }

        IEnumerator<Result> GetObserver(IScreenInput fingerInput, ITimer timer);
    }
}

