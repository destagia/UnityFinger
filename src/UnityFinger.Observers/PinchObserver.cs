using System.Collections.Generic;
using UnityEngine;

namespace UnityFinger
{
    public struct PinchInfo
    {
        public readonly Vector2 startPosition1;
        public readonly Vector2 startPosition2;

        public readonly Vector2 prevPosition1;
        public readonly Vector2 prevPosition2;

        public readonly Vector2 position1;
        public readonly Vector2 position2;

        public PinchInfo(Vector2 startPosition1, Vector2 startPosition2, Vector2 position1, Vector2 position2, Vector2 prevPosition1, Vector2 prevPosition2)
        {
            this.position1 = position1;
            this.position2 = position2;
            this.startPosition1 = startPosition1;
            this.startPosition2 = startPosition2;
            this.prevPosition1 = prevPosition1;
            this.prevPosition2 = prevPosition2;
        }

        public float StartDistance {
            get { return (startPosition1 - startPosition2).magnitude; }
        }

        public float PrevDistance {
            get { return (prevPosition1 - prevPosition2).magnitude; }
        }

        public float Distance {
            get { return (position1 - position2).magnitude; }
        }
    }

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
        private const float PinchStartDistance = 0.02f;

        IPinchListener listener;

        public PinchObserver(IPinchListener listener)
        {
            this.listener = listener;
        }

        #region IObserver implementation

        public int Priority { get { return 100; } }

        public IEnumerator<Result> GetObserver(IScreenInput fingerInput, ITimer timer)
        {
            Vector2 startFirst = Vector2.zero;
            Vector2 startSecond = Vector2.zero;

            bool isFirstFrame = true;
            Vector2 firstStartFirst = Vector2.zero;
            Vector2 firstStartSecond = Vector2.zero;

            // Wait until two fingers are on screen in time
            while (fingerInput.FingerCount > 0) {
                if (fingerInput.FingerCount == 2) {

                    startFirst = fingerInput.GetPosition();
                    startSecond = fingerInput.GetSecondPosition();

                    if (isFirstFrame) {
                        isFirstFrame = false;
                        firstStartFirst = startFirst;
                        firstStartSecond = startSecond;
                    }

                    if ((startFirst - firstStartFirst).magnitude < PinchStartDistance || (startSecond - firstStartSecond).magnitude < PinchStartDistance) {
                        yield return Result.None;
                    } else {
                        listener.OnPinchStart(new PinchInfo(firstStartFirst, firstStartSecond, startFirst, startSecond, firstStartFirst, firstStartSecond));
                        yield return Result.InAction;
                        break;
                    }
                } else if (fingerInput.FingerCount > 2) {
                    yield break;
                } else {
                    yield return Result.None;
                }
            }

            Vector2 prevFirst = startFirst;
            Vector2 prevSecond = startSecond;

            var invokePinch = false;

            while (fingerInput.FingerCount >= 2) {
                var first = fingerInput.GetPosition();
                var second = fingerInput.GetSecondPosition();

                listener.OnPinch(new PinchInfo(startFirst, startSecond, first, second, prevFirst, prevSecond));

                prevFirst = first;
                prevSecond = second;

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

