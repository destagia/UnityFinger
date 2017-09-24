namespace UnityFinger
{
    public interface ITimer
    {
        void Start();

        float ElapsedTime { get; }
    }
}
