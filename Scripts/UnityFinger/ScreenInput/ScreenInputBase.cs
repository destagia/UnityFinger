using UnityEngine;
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
    public abstract class ScreenInputBase : IScreenInput
    {
        EventSystem eventSystem;
        public EventSystem EventSystem {
            get {
                if (eventSystem == null || eventSystem.gameObject == null) {
                    eventSystem = UnityEngine.Object.FindObjectOfType<EventSystem>();
                    if (eventSystem == null) {
                        var eventSystemObj = new GameObject {
                            name = "EventSystem"
                        };
                        eventSystem = eventSystemObj.AddComponent<EventSystem>();
                        eventSystemObj.AddComponent<StandaloneInputModule>();
                    }
                }

                return eventSystem;
            }
        }

        /// <summary>
        /// Must update to change their states.
        /// </summary>
        public abstract void Update();

        #region ITimer implementation

        public abstract int FingerCount { get; }
        public abstract Vector2 GetPosition();
        public abstract Vector2 GetSecondPosition();

        #endregion
    }
}

