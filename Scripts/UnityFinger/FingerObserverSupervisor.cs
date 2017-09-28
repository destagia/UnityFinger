using System.Linq;
using System.Collections.Generic;

namespace UnityFinger
{
    public class FingerObserverSupervisor
    {
        readonly IScreenInput input;

        bool isFirstOnScreen;

        readonly ITimer timer;

        readonly List<IObserverFactory> observerFactories;

        /// <summary>
        /// Observing coroutines
        /// </summary>
        readonly List<IEnumerator<Result>> observers;

        /// <summary>
        /// If the enumerator return Result.InAction, it is being focus on
        /// </summary>
        IEnumerator<Result> selectedCoroutine;

        public void AddObserver(IObserverFactory observer)
        {
            observerFactories.Add(observer);
            observerFactories.Sort(new ObserverComparer());
        }

        public void RemoveObserver(IObserverFactory observer)
        {
            observerFactories.Remove(observer);
        }

        public FingerObserverSupervisor(IScreenInput input, ITimer timer)
        {
            this.input = input;
            this.timer = timer;
            observerFactories = new List<IObserverFactory>();
            observers = new List<IEnumerator<Result>>();
        }

        public void Update()
        {
            if (input.FingerCount == 0) {
                if (!OnEvent()) {
                    foreach (var observerCoroutine in observers) {
                        observerCoroutine.Dispose();
                    }

                    selectedCoroutine = null;
                }

                isFirstOnScreen = true;
                return;
            }

            if (isFirstOnScreen) {
                timer.Start();

                foreach (var observer in observers) {
                    observer.Dispose();
                }
                observers.Clear();

                foreach (var observer in observerFactories) {
                    observers.Add(observer.GetObserver(input, timer));
                }

                isFirstOnScreen = false;
            }

            OnEvent();
        }

        void OnDestroy()
        {
            foreach (var observerCoroutine in observers) {
                observerCoroutine.Dispose();
            }
        }

        bool OnEvent()
        {
            if (selectedCoroutine != null) {
                return selectedCoroutine.MoveNext();
            }

            selectedCoroutine = observers.Find(o => o.MoveNext() && o.Current == Result.InAction);
            return selectedCoroutine != null;
        }

        class ObserverComparer : IComparer<IObserverFactory>
        {
            #region IComparer implementation

            public int Compare(IObserverFactory x, IObserverFactory y)
            {
                return x.Priority - y.Priority;
            }

            #endregion
        }
    }
}