using UnityEngine;
using System.Collections.Generic;

namespace UnityFinger
{
    public class FingerObserverSupervisor : ITimer
    {
        readonly IScreenInput input;

        readonly List<IObserver> observers;

        /// <summary>
        /// Observing coroutines
        /// </summary>
        readonly List<IEnumerator<Result>> observerCoroutines;

        /// <summary>
        /// If the enumerator return Result.InAction, it is being focus on
        /// </summary>
        IEnumerator<Result> selectedCoroutine;

        public void AddObserver(IObserver observer)
        {
            observers.Add(observer);
            observers.Sort(new ObserverComparer());
        }

        public void RemoveObserver(IObserver observer)
        {
            observers.Remove(observer);
        }

        public FingerObserverSupervisor(IScreenInput input)
        {
            this.input = input;
            observers = new List<IObserver>();
            observerCoroutines = new List<IEnumerator<Result>>();
        }

        public void Update()
        {
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

        bool OnEvent()
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

        bool isFirstOnScreen;

        float onScreenStartTime;

        public float ElapsedTime { get { return Time.time - onScreenStartTime; } }

        #endregion

        class ObserverComparer : IComparer<IObserver>
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