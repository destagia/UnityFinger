using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityFinger
{
    public struct TwoFingersInfo
    {
        public readonly Vector2 firstPosition;
        public readonly Vector2 secondPosition;

        public TwoFingersInfo(Vector2 firstPosition, Vector2 secondPosition)
        {
            this.firstPosition = firstPosition;
            this.secondPosition = secondPosition;
        }
    }

    public interface ITwoFingersListener
    {
        void OnTwoFingersTap(TwoFingersInfo info);
    }
}

namespace UnityFinger.Observers
{
    public class TwoFingersTapObserver : IObserver
    {
        private const float StartDuration = 0.05f;
        private const float TwoFingersDuration = 0.25f;
        private const float ReleaseDuration = 0.3f;

        ITwoFingersListener listener;

        public TwoFingersTapObserver(ITwoFingersListener listener)
        {
            this.listener = listener;
        }

        #region IObserver implementation

        public int Priority { get { return 100; } }

        public IEnumerator<Result> GetObserver(IScreenInput fingerInput, ITimer timer)
        {
            var first = Vector2.zero;
            var second = Vector2.zero;

            // Wait until two fingers are on screen in time
            while (fingerInput.FingerCount > 0) {
                if (timer.ElapsedTime > StartDuration) {
                    yield break;
                }
                if (fingerInput.FingerCount == 2) {
                    yield return Result.None;
                    break;
                } else if (fingerInput.FingerCount > 2) {
                    yield break;
                }
                yield return Result.None;
            }

            var twofingerInvoke = false;

            while (fingerInput.FingerCount == 2) {
                twofingerInvoke = true;
                if (timer.ElapsedTime > TwoFingersDuration) {
                    yield break;
                }
                first = fingerInput.GetPosition();
                second = fingerInput.GetSecondPosition();
                yield return Result.None;
            }

            if (!twofingerInvoke) {
                yield break;
            }

            while (fingerInput.FingerCount > 0) {
                if (timer.ElapsedTime > ReleaseDuration) {
                    yield break;
                }
                yield return Result.None;
            }

            listener.OnTwoFingersTap(new TwoFingersInfo(first, second));
            yield return Result.InAction;
        }

        #endregion
    }
}

