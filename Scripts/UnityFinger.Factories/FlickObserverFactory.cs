using UnityEngine;
using System.Collections.Generic;

namespace UnityFinger
{
    public interface IFlickListener
    {
        void OnFlick(FlickInfo info);
    }
}

namespace UnityFinger.Factories
{
    public class FlickObserverFactory : ObserverFactoryBase<IFlickListener>
    {
        public FlickObserverFactory(IFingerObserverConfig config, IFlickListener listener)
            : base(config, listener)
        {
        }

        #region IObserverFactory implementation

        public override int Priority { get { return 400; } }
        
        public override IEnumerator<Observation> GetObserver(IScreenInput input, IReadOnlyTimer timer)
        {
            var firstPosition = input.GetPosition();
            var secondPosition = firstPosition;

            while (input.FingerCount > 0) {
                if (input.FingerCount > 1) {
                    yield break;
                }
                secondPosition = input.GetPosition();
                yield return Observation.None;
            }

            if (timer.ElapsedTime > 0.4f) {
                yield break;
            }

            var direction = secondPosition - firstPosition;
            if (direction.magnitude < Config.FlickDistance) {
                yield break;
            }

            Listener.OnFlick(new FlickInfo(firstPosition, direction));
            yield return Observation.Fired;
        }

        #endregion
    }
}

