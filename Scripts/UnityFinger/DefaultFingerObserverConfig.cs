namespace UnityFinger
{
    public class DefaultFingerObserverConfig : IFingerObserverConfig
    {
        public virtual float DragDuration { get { return 0.15f; } }
        public virtual float DragDistance { get { return 60f; } }

        public virtual float FlickDistance { get { return 60f; } }

        public virtual float LongTapDuration { get { return 0.6f; } }
        public virtual float LongTapDistance { get { return 60f; } }

        public virtual float PinchStartDistance { get { return 30f; } }

        public virtual float TapDuration { get { return 0.15f; } }
        public virtual float TapDistance { get { return 30f; } }

        public virtual float TwoFingersTapDuration { get { return 0.05f; } }
        public virtual float TwoFingersTapStartDuration { get { return 0.25f; } }
        public virtual float TwoFingersTapReleaseDuration { get { return 0.3f; } }
    }
}
