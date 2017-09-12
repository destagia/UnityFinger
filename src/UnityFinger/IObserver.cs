using System.Collections.Generic;

namespace UnityFinger
{
    public enum Result
    {
        None,
        InAction
    }

    public interface ITimer
    {
        float ElapsedTime { get; }
    }

    public interface IObserver
    {
        int Priority { get; }

        IEnumerator<Result> GetObserver(IScreenInput fingerInput, ITimer timer);
    }
}

