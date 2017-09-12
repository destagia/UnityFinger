using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityFinger
{
    public interface IFingerObserverConfig
    {
        float DragDuration { get; }
        float DragDistance { get; }

        float FlickDistance { get; }

        float LongTapDuration { get; }
        float LongTapDistance { get; }

        float PinchStartDistance { get; }

        float TapDuration { get; }
        float TapDistance { get; }

        float TwoFingersTapDuration { get; }
        float TwoFingersTapStartDuration { get; }
        float TwoFingersTapReleaseDuration { get; }
    }

    public class DefaultFingerObserverConfig : IFingerObserverConfig
    {
        public float DragDuration { get { return 0.15f; } }
        public float DragDistance { get { return 0.05f; } }

        public float FlickDistance { get { return 0.05f; } }

        public float LongTapDuration { get { return 0.6f; } }
        public float LongTapDistance { get { return 0.05f; } }

        public float PinchStartDistance { get { return 0.02f; } }

        public float TapDuration { get { return 0.15f; } }
        public float TapDistance { get { return 0.02f; } }

        public float TwoFingersTapDuration { get { return 0.05f; } }
        public float TwoFingersTapStartDuration { get { return 0.25f; } }
        public float TwoFingersTapReleaseDuration { get { return 0.3f; } }
    }
}
