using System;

namespace UnityFinger.Test
{
    class TestConfig : IFingerObserverConfig
    {
        public float DragDuration {
            get { return 1.0f; }
        }
        public float DragDistance {
            get { return 1.0f; }
        }
        public DragOptionFlag DragOptionFlag {
            get { return DragOptionFlag.None; }
        }
        public float FlickDistance {
            get { return 1.0f; }
        }
        public float LongTapDuration {
            get { return 1.0f; }
        }
        public float LongTapDistance {
            get { return 1.0f; }
        }
        public float PinchStartDistance {
            get { return 1.0f; }
        }
        public float TapDuration {
            get { return 1.0f; }
        }
        public float TapDistance {
            get { return 1.0f; }
        }
        public float TwoFingersTapDuration {
            get { return 1.0f; }
        }
        public float TwoFingersTapStartDuration {
            get { return 1.0f; }
        }
        public float TwoFingersTapReleaseDuration {
            get { return 1.0f; }
        }
    }
}