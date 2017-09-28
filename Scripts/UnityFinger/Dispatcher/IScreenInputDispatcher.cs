using System;
using UnityEngine;

namespace UnityFinger
{
    public interface IScreenInputDispatcher
    {
        void DispatchPosition(ref Vector2 position);

        void DispatchDistance(ref float distance);
    }
}