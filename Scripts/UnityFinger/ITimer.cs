namespace UnityFinger
{
    public interface IReadOnlyTimer
    {
        float ElapsedTime { get; }
    }

    public interface ITimer : IReadOnlyTimer
    {
        void Start();
    }
}
