using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityFinger;
using UnityFinger.Observers;

namespace UnityFinger
{
	public class FingerSupervisor : MonoBehaviour, ITimer
	{
		private static FingerSupervisor instance;
		public static FingerSupervisor Instance
		{
			get {
				if (instance == null || instance.gameObject == null) {
					var fingerManagerObject = Object.FindObjectOfType<FingerSupervisor>();
					if (fingerManagerObject == null) {
						var gameObject = new GameObject("FingerSupervisor");
						instance = gameObject.AddComponent<FingerSupervisor>();
					}
				}
				return instance;
			}
		}

		private ScreenInputBase input;
		private List<IObserver> observers;

		private float onScreenStartTime;
		private bool isFirstOnScreen;

		/// <summary>
		/// Observing coroutines
		/// </summary>
		private List<IEnumerator<Result>> observerCoroutines = new List<IEnumerator<Result>>();

		/// <summary>
		/// If the enumerator return Result.InAction, it is being focus on
		/// </summary>
		private IEnumerator<Result> selectedCoroutine;

		public void AddObserver(IObserver observer)
		{
			observers.Add(observer);
			observers.Sort(new ObserverComparer());
		}

		public void RemoveObserver(IObserver observer)
		{
			observers.Remove(observer);
		}

		#region Unity Callbacks

		void Awake()
		{
			input = new EditorInput();
			// input = new MobileInput();
			observers = new List<IObserver>();
		}

		void Update()
		{
			input.Update();

			if (input.FingerCount > 0) {
				if (isFirstOnScreen) {
					onScreenStartTime = Time.time;
					foreach (var observer in observerCoroutines) {
						observer.Dispose();
					}
					observerCoroutines.Clear();
					foreach (var observer in observers) {
						observerCoroutines.Add(observer.GetObserver(input, this));
					}
					isFirstOnScreen = false;
				}
				OnEvent();
			} else {
				if (!OnEvent()) {
					foreach (var observerCoroutine in observerCoroutines) {
						observerCoroutine.Dispose();
					}
					selectedCoroutine = null;
				}
				isFirstOnScreen = true;
			}
		}

		void OnDestroy()
		{
			foreach (var observerCoroutine in observerCoroutines) {
				observerCoroutine.Dispose();
			}
		}

		#endregion

		private bool OnEvent()
		{
			if (selectedCoroutine != null) {
				return selectedCoroutine.MoveNext();
			}
			var isAnyContinuing = false;
			foreach (var observer in observerCoroutines) {
				if ((isAnyContinuing |= observer.MoveNext()) && observer.Current == Result.InAction) {
					selectedCoroutine = observer;
					return true;
				}
			}
			return isAnyContinuing;
		}

		#region ITimer implementation

		public float ElapsedTime { get { return Time.time - onScreenStartTime; } }

		#endregion

		private class ObserverComparer : IComparer<IObserver>
		{
			#region IComparer implementation
			public int Compare(IObserver x, IObserver y)
			{
				return x.Priority - y.Priority;
			}
			#endregion
		}
	}
}