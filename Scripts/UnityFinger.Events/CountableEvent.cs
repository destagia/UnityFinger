using UnityEngine.Events;

namespace UnityFinger.Events
{
    abstract class CountableEvent<T> : UnityEvent<T>, ICountableEvent
    {
        public int ListenersCount { set; get; }

        new public void AddListener(UnityAction<T> callback)
        {
            ListenersCount++;
            base.AddListener(callback);
        }

        new public void RemoveListener(UnityAction<T> callback)
        {
            ListenersCount--;
            base.AddListener(callback);
        }
    }
}
