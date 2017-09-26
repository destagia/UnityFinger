namespace UnityFinger
{
    using UnityEngine;

    public struct DragInfo
    {
        readonly Vector2 origin;
        readonly Vector2 previous;
        readonly Vector2 current;

        public DragInfo(Vector2 origin, Vector2 previous, Vector2 current)
        {
            this.origin = origin;
            this.previous = previous;
            this.current = current;
        }

        public Vector2 Origin {
            get { return origin; }
        }

        public Vector2 Previous {
            get { return previous; }
        }

        public Vector2 Current {
            get { return current; }
        }
    }
}

namespace UnityFinger.Events
{
    class DragEvent : CountableEvent<DragInfo>
    {
    }
}
