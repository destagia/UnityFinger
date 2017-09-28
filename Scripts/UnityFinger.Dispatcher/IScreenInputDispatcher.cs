using UnityEngine;

namespace UnityFinger.Dispatcher
{
    public interface IScreenInputDispatcher
    {
        void DispatchPosition(ref Vector2 position);

        void DispatchDistance(ref float distance);
    }
}