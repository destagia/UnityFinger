namespace UnityFinger.Dispatcher
{
    public class DispatchedFingerObserverConfig : IFingerObserverConfig
    {
        readonly IFingerObserverConfig config;
        readonly IScreenInputDispatcher dispatcher;

        public DispatchedFingerObserverConfig(IFingerObserverConfig config, IScreenInputDispatcher dispatcher)
        {
            this.config = config;
            this.dispatcher = dispatcher;
        }

        public float DragDuration {
            get { return config.DragDuration; }
        }

        public float DragDistance {
            get {
                var distance = config.DragDistance;
                dispatcher.DispatchDistance(ref distance);
                return distance;
            }
        }

        public DragOptionFlag DragOptionFlag {
            get { return config.DragOptionFlag; }
        }

        public float FlickDistance {
            get {
                var distance = config.FlickDistance;
                dispatcher.DispatchDistance(ref distance);
                return distance;
            }
        }

        public float LongTapDuration {
            get { return config.LongTapDuration; }
        }

        public float LongTapDistance {
            get {
                var distance = config.LongTapDistance;
                dispatcher.DispatchDistance(ref distance);
                return distance;
            }
        }

        public float PinchStartDistance {
            get {
                var distance = config.PinchStartDistance;
                dispatcher.DispatchDistance(ref distance);
                return distance;
            }
        }

        public float TapDuration {
            get { return config.TapDuration; }
        }

        public float TapDistance {
            get {
                var distance = config.TapDistance;
                dispatcher.DispatchDistance(ref distance);
                return distance;
            }
        }

        public float TwoFingersTapDuration {
            get { return config.TwoFingersTapDuration; }
        }

        public float TwoFingersTapStartDuration {
            get { return config.TwoFingersTapStartDuration; }
        }

        public float TwoFingersTapReleaseDuration {
            get { return config.TwoFingersTapReleaseDuration; }
        }
    }
}