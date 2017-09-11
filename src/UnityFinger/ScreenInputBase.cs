using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace UnityFinger
{
	public static class ScreenInput
	{
		public static bool IgnoreOverGameObject { get; set; }
	}
}

namespace UnityFinger
{
	/// <summary>
	/// There is the difference that ScreenInputBase has the function `Update` but
	/// IScreenInput not. Because ScreenInputBase is iterated at FingerManager
	/// but IScreenInput is used as the container of the input infomations.
	/// The Function Update must be visible only for supervisor such as FingerManager.
	/// </summary>
	abstract class ScreenInputBase : IScreenInput
	{
		private EventSystem eventSystem;
		protected EventSystem EventSystem {
			get {
				if (eventSystem == null || eventSystem.gameObject == null) {
					eventSystem = UnityEngine.Object.FindObjectOfType<EventSystem>();
					if (eventSystem == null) {
						var eventSystemObj = new GameObject();
						eventSystemObj.name = "EventSystem";
						eventSystem = eventSystemObj.AddComponent<EventSystem>();
						eventSystemObj.AddComponent<StandaloneInputModule>();
						eventSystemObj.AddComponent<TouchInputModule>();
					}
				}
				return eventSystem;
			}
		}

		public abstract void Update();

		#region ITimer implementation

		public abstract int FingerCount { get; }
		public abstract Vector2 GetPosition();
		public abstract Vector2 GetSecondPosition();

		#endregion
	}
}

