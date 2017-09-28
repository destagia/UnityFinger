using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityFinger
{
    public interface ITwoFingersListener
    {
        void OnTwoFingersTap(TwoFingersTapInfo info);
    }
}

namespace UnityFinger.ObserverFactories
{
    public class TwoFingersTapObserverFactory : ObserverFactoryBase<ITwoFingersListener>
    {
        public TwoFingersTapObserverFactory(IFingerObserverConfig config, ITwoFingersListener listener)
            : base(config, listener)
        {
        }

        #region IObserverFactory implementation

        public override int Priority { get { return 100; } }

        public override IEnumerator<Observation> GetObserver(IScreenInput input, IReadOnlyTimer timer)
        {
            var first = Vector2.zero;
            var second = Vector2.zero;

            // Wait until two fingers are on screen in time
            while (input.FingerCount > 0) {
                if (timer.ElapsedTime > Config.TwoFingersTapStartDuration) {
                    yield break;
                }
                if (input.FingerCount == 2) {
                    yield return Observation.None;
                    break;
                } else if (input.FingerCount > 2) {
                    yield break;
                }
                yield return Observation.None;
            }

            var twofingerInvoke = false;

            while (input.FingerCount == 2) {
                twofingerInvoke = true;
                if (timer.ElapsedTime > Config.TwoFingersTapDuration) {
                    yield break;
                }
                first = input.GetPosition();
                second = input.GetSecondPosition();
                yield return Observation.None;
            }

            if (!twofingerInvoke) {
                yield break;
            }

            while (input.FingerCount > 0) {
                if (timer.ElapsedTime > Config.TwoFingersTapReleaseDuration) {
                    yield break;
                }
                yield return Observation.None;
            }

            Listener.OnTwoFingersTap(new TwoFingersTapInfo(first, second));
            yield return Observation.Fired;
        }

        #endregion
    }
}

