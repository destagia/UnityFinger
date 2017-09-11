using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace UnityFinger
{
	/// <summary>
	/// Use as the container of input informations at IObserver
	/// </summary>
	public interface IScreenInput
	{
		int FingerCount { get; }

		Vector2 GetPosition();

		Vector2 GetSecondPosition();
	}
	
}
