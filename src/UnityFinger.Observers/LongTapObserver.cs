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
        private readonly IFingerObserverConfig config;
        private readonly ILongTapListener listener;

        private readonly float longTapDistanceSqr;

        public LongTapObserver(IFingerObserverConfig config, ILongTapListener listener)
        {
            this.config = config;
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
                if ((secondPosition - origin).magnitude > config.LongTapDistance) {
                    yield break;
                }

                if (timer.ElapsedTime > config.LongTapDuration) {
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

