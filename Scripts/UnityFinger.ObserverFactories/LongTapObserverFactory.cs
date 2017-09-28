using System.Collections.Generic;
using UnityEngine;


namespace UnityFinger
{
    public interface ILongTapListener
    {
        void OnLongTap(Vector2 position);
    }
}

namespace UnityFinger.ObserverFactories
{
    public class LongTapObserverFactory : ObserverFactoryBase<ILongTapListener>
    {
        public LongTapObserverFactory(IFingerObserverConfig config, ILongTapListener listener)
            : base(config, listener)
        {
        }

        #region IObserverFactory implementation

        public override int Priority { get { return 200; } }

        public override IEnumerator<Result> GetObserver(IScreenInput input, IReadOnlyTimer timer)
        {
            var origin = input.GetPosition();

            while (input.FingerCount > 0) {
                if (input.FingerCount > 1) {
                    yield break;
                }

                var secondPosition = input.GetPosition();
                if ((secondPosition - origin).magnitude > Config.LongTapDistance) {
                    yield break;
                }

                if (timer.ElapsedTime > Config.LongTapDuration) {
                    break;
                } else {
                    yield return Result.None;
                }
            }

            if (input.FingerCount == 0) {
                yield break;
            }

            var currentPosition = input.GetPosition();

            Listener.OnLongTap(currentPosition);
            yield return Result.InAction;
        }

        #endregion
    }
}

