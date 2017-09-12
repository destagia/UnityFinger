using UnityEngine;
using System.Collections.Generic;

namespace UnityFinger
{
    public struct FlickInfo
    {
        public readonly Vector2 origin;
        public readonly Vector2 direction;

        public FlickInfo(Vector2 origin, Vector2 direction)
        {
            this.origin = origin;
            this.direction = direction;
        }
    }

    public interface IFlickListener
    {
        void OnFlick(FlickInfo info);
    }
}

namespace UnityFinger.Observers
{
    public class FlickObserver : IObserver
    {
         readonly IFlickListener listener;

         readonly IFingerObserverConfig config;

        public FlickObserver(IFingerObserverConfig config, IFlickListener listener)
        {
            this.config = config;
            this.listener = listener;
        }

        #region IObserver implementation

        public int Priority { get { return 400; } }

        public IEnumerator<Result> GetObserver(IScreenInput fingerInput, ITimer timer)
        {
            var firstPosition = fingerInput.GetPosition();
            var secondPosition = firstPosition;

            while (fingerInput.FingerCount > 0) {
                if (fingerInput.FingerCount > 1) {
                    yield break;
                }
                secondPosition = fingerInput.GetPosition();
                yield return Result.None;
            }

            if (timer.ElapsedTime > 0.4f) {
                yield break;
            }

            var direction = secondPosition - firstPosition;
            if (direction.magnitude < config.FlickDistance) {
                yield break;
            }

            listener.OnFlick(new FlickInfo(firstPosition, direction));
            yield return Result.InAction;
        }

        #endregion
    }
}

