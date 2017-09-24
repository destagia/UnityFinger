using UnityEngine.Events;

namespace UnityFinger.Events
{
    class VoidEvent : UnityEvent, ICountableEvent
    {
        public int ListenersCount { set; get; }

        new public void AddListener(UnityAction callback)
        {
            ListenersCount++;
            base.AddListener(callback);
        }

        new public void RemoveListener(UnityAction callback)
        {
            ListenersCount--;
            base.AddListener(callback);
        }
    }
}
