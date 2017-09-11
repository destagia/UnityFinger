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

namespace UnityFinger.Observers
{
	public class TapObserver : IObserver
	{
		private const float TapDuration = 0.15f;
		private const float TapDistance = 0.02f;

		ITapListener listener;

		public TapObserver(ITapListener listener)
		{
			this.listener = listener;
		}

		#region IFingerObserver implementation

		public int Priority { get { return 100; } }

		public IEnumerator<Result> GetObserver(IScreenInput fingerInput, ITimer timer)
		{
			var position = fingerInput.GetPosition();

			while (fingerInput.FingerCount > 0) {
				if (fingerInput.FingerCount > 1) {
					yield break;
				}
				var secondPosition = fingerInput.GetPosition();
				if ((secondPosition - position).magnitude > TapDistance) {
					yield break;
				}
				position = secondPosition;
				yield return Result.None;
			}

			if (timer.ElapsedTime > TapDuration) {
				yield break;
			}

			listener.OnTap(position);
			yield return Result.InAction;
		}

		#endregion
	}
}

