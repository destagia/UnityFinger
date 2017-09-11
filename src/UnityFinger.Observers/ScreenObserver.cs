using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityFinger
{
    public interface IScreenListener
    {
        void OnScreen(Vector2 position);
    }
}

namespace UnityFinger.Observers
{
	public class ScreenObserver : IObserver
	{
		IScreenListener listener;

		public ScreenObserver(IScreenListener listener)
		{
			this.listener = listener;
		}

		#region IFingerObserver implementation

		public int Priority { get { return 0; } }

		public IEnumerator<Result> GetObserver(IScreenInput fingerInput, ITimer timer)
		{
			listener.OnScreen(fingerInput.GetPosition());
			yield break;
		}

		#endregion
	}
}

