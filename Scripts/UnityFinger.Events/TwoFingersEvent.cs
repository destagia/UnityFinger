namespace UnityFinger
{
    using UnityEngine;

    public struct TwoFingersTapInfo
    {
        readonly Vector2 firstPosition;
        readonly Vector2 secondPosition;

        public TwoFingersTapInfo(Vector2 firstPosition, Vector2 secondPosition)
        {
            this.firstPosition = firstPosition;
            this.secondPosition = secondPosition;
        }

        public Vector2 First {
            get { return firstPosition; }
        }

        public Vector2 Second {
            get { return secondPosition; }
        }
    }
}

namespace UnityFinger.Events
{
    class TwoFingersEvent : CountableEvent<TwoFingersTapInfo>
    {
    }
}
