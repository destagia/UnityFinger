namespace UnityFinger
{
    using UnityEngine;

    public struct PinchInfo
    {
        readonly DragInfo first;
        readonly DragInfo second;

        public PinchInfo(DragInfo first, DragInfo second)
        {
            this.first = first;
            this.second = second;
        }

        public DragInfo First {
            get { return first; }
        }

        public DragInfo Second {
            get { return second; }
        }
    }
}

namespace UnityFinger.Events
{
    class PinchEvent : CountableEvent<PinchInfo>
    {
    }
}
