using System.Collections.Generic;
using UnityEngine;

namespace UnityFinger
{
    public interface IPinchListener
    {
        void OnPinchStart(PinchInfo info);
        void OnPinch(PinchInfo info);
        void OnPinchEnd();
    }
}

namespace UnityFinger.ObserverFactories
{
    public class PinchObserverFactory : ObserverFactoryBase<IPinchListener>
    {
        public PinchObserverFactory(IFingerObserverConfig config, IPinchListener listener)
            : base(config, listener)
        {
        }

        #region IObserverFactory implementation

        public override int Priority { get { return 100; } }

        public override IEnumerator<Observation> GetObserver(IScreenInput input, IReadOnlyTimer timer)
        {
            Vector2 firstCurrent = Vector2.zero;
            Vector2 secondCurrent = Vector2.zero;

            bool isFirstFrame = true;
            Vector2 firstOrigin = Vector2.zero;
            Vector2 secondOrigin = Vector2.zero;

            // Wait until two fingers are on screen in time
            while (input.FingerCount > 0) {

                if (input.FingerCount > 2) {
                    yield break;
                }

                if (input.FingerCount < 2) {
                    yield return Observation.None;
                }

                firstCurrent = input.GetPosition();
                secondCurrent = input.GetSecondPosition();

                if (isFirstFrame) {
                    isFirstFrame = false;
                    firstOrigin = firstCurrent;
                    secondOrigin = secondCurrent;
                }

                var firstFingerMove = (firstCurrent - firstOrigin).magnitude;
                if (firstFingerMove < Config.PinchStartDistance) {
                    yield return Observation.None;
                }

                var secondFingerMove = (secondCurrent - secondOrigin).magnitude;
                if (secondFingerMove < Config.PinchStartDistance) {
                    yield return Observation.None;
                }

                var first = new DragInfo(firstOrigin, firstOrigin, firstCurrent);
                var second = new DragInfo(secondOrigin, secondOrigin, secondCurrent);

                Listener.OnPinchStart(new PinchInfo(first, second));
                break;
            }

            yield return Observation.Fired;

            Vector2 firstPrevious = firstCurrent;
            Vector2 secondPrevious = secondCurrent;

            var invokePinch = false;

            while (input.FingerCount >= 2) {
                firstCurrent = input.GetPosition();
                secondCurrent = input.GetSecondPosition();

                var first = new DragInfo(firstOrigin, firstPrevious, firstCurrent);
                var second = new DragInfo(secondOrigin, secondPrevious, secondCurrent);
                Listener.OnPinch(new PinchInfo(first, second));

                firstPrevious = firstCurrent;
                secondPrevious = secondCurrent;

                invokePinch = true;
                yield return Observation.Fired;
            }

            if (invokePinch) {
                Listener.OnPinchEnd();
                yield return Observation.Fired;
            }
        }

        #endregion
    }
}

