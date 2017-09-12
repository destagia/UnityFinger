using System.Collections.Generic;
using UnityEngine;


namespace UnityFinger
{
    public interface ILongTapListener
    {
        void OnLongTap(Vector2 position);
    }
}

namespace UnityFinger.Observers
{
    public class LongTapObserver : IObserver
    {
        private const float LongTapDuration = 0.6f;
        private const float LongTapDistance = 0.05f;
        private const float LongTapDistanceSqr = LongTapDistance * LongTapDistance;

        ILongTapListener listener;

        public LongTapObserver(ILongTapListener listener)
        {
            this.listener = listener;
        }

        #region IObserver implementation

        public int Priority { get { return 200; } }

        public IEnumerator<Result> GetObserver(IScreenInput fingerInput, ITimer timer)
        {
            var origin = fingerInput.GetPosition();

            while (fingerInput.FingerCount > 0) {
                if (fingerInput.FingerCount > 1) {
                    yield break;
                }

                var secondPosition = fingerInput.GetPosition();
                if ((secondPosition - origin).magnitude > LongTapDistance) {
                    yield break;
                }

                if (timer.ElapsedTime > LongTapDuration) {
                    break;
                } else {
                    yield return Result.None;
                }
            }

            if (fingerInput.FingerCount == 0) {
                yield break;
            }

            var currentPosition = fingerInput.GetPosition();

            listener.OnLongTap(currentPosition);
            yield return Result.InAction;
        }

        #endregion
    }
}

