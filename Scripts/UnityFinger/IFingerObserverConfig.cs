namespace UnityFinger
{
    public interface IFingerObserverConfig
    {
        float DragDuration { get; }
        float DragDistance { get; }
        DragOptionFlag DragOptionFlag { get;  }

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
}
