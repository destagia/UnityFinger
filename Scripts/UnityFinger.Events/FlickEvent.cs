namespace UnityFinger
{
    using UnityEngine;

    public struct FlickInfo
    {
        readonly Vector2 origin;
        readonly Vector2 direction;

        public FlickInfo(Vector2 origin, Vector2 direction)
        {
            this.origin = origin;
            this.direction = direction;
        }

        public Vector2 Origin {
            get { return origin; }
        }

        public Vector2 Direction {
            get { return direction; }
        }
    }
}

namespace UnityFinger.Events
{
    class FlickEvent : CountableEvent<FlickInfo>
    {
    }
}
