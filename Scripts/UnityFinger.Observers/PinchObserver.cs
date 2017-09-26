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

namespace UnityFinger.Observers
{
    public class PinchObserver : IObserver
    {
        readonly IFingerObserverConfig config;

        readonly IPinchListener listener;

        public PinchObserver(IFingerObserverConfig config, IPinchListener listener)
        {
            this.config = config;
            this.listener = listener;
        }

        #region IObserver implementation

        public int Priority { get { return 100; } }

        public IEnumerator<Result> GetObserver(IScreenInput fingerInput, ITimer timer)
        {
            Vector2 firstCurrent = Vector2.zero;
            Vector2 secondCurrent = Vector2.zero;

            bool isFirstFrame = true;
            Vector2 firstOrigin = Vector2.zero;
            Vector2 secondOrigin = Vector2.zero;

            // Wait until two fingers are on screen in time
            while (fingerInput.FingerCount > 0) {

                if (fingerInput.FingerCount > 2) {
                    yield break;
                }

                if (fingerInput.FingerCount < 2) {
                    yield return Result.None;
                }

                firstCurrent = fingerInput.GetPosition();
                secondCurrent = fingerInput.GetSecondPosition();

                if (isFirstFrame) {
                    isFirstFrame = false;
                    firstOrigin = firstCurrent;
                    secondOrigin = secondCurrent;
                }

                var firstFingerMove = (firstCurrent - firstOrigin).magnitude;
                if (firstFingerMove < config.PinchStartDistance) {
                    yield return Result.None;
                }

                var secondFingerMove = (secondCurrent - secondOrigin).magnitude;
                if (secondFingerMove < config.PinchStartDistance) {
                    yield return Result.None;
                }

                var first = new DragInfo(firstOrigin, firstOrigin, firstCurrent);
                var second = new DragInfo(secondOrigin, secondOrigin, secondCurrent);

                listener.OnPinchStart(new PinchInfo(first, second));
                break;
            }

            yield return Result.InAction;

            Vector2 firstPrevious = firstCurrent;
            Vector2 secondPrevious = secondCurrent;

            var invokePinch = false;

            while (fingerInput.FingerCount >= 2) {
                firstCurrent = fingerInput.GetPosition();
                secondCurrent = fingerInput.GetSecondPosition();

                var first = new DragInfo(firstOrigin, firstPrevious, firstCurrent);
                var second = new DragInfo(secondOrigin, secondPrevious, secondCurrent);
                listener.OnPinch(new PinchInfo(first, second));

                firstPrevious = firstCurrent;
                secondPrevious = secondCurrent;

                invokePinch = true;
                yield return Result.InAction;
            }

            if (invokePinch) {
                listener.OnPinchEnd();
                yield return Result.InAction;
            }
        }

        #endregion
    }
}

