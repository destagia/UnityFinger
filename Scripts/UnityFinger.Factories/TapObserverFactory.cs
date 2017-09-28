using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityFinger
{
    public interface ITapListener
    {
        void OnTap(Vector2 position);
    }
}

namespace UnityFinger.Factories
{
    public class TapObserverFactory : ObserverFactoryBase<ITapListener>
    {
        public TapObserverFactory(IFingerObserverConfig config, ITapListener listener)
            : base(config, listener)
        {
        }

        #region IObserverFactory implementation

        public override int Priority { get { return 100; } }

        public override IEnumerator<Observation> GetObserver(IScreenInput input, IReadOnlyTimer timer)
        {
            var position = input.GetPosition();

            while (input.FingerCount > 0) {
                if (input.FingerCount > 1) {
                    yield break;
                }
                var secondPosition = input.GetPosition();
                if ((secondPosition - position).magnitude > Config.TapDistance) {
                    yield break;
                }
                position = secondPosition;
                yield return Observation.None;
            }

            if (timer.ElapsedTime > Config.TapDuration) {
                yield break;
            }

            Listener.OnTap(position);
            yield return Observation.Fired;
        }

        #endregion
    }
}

