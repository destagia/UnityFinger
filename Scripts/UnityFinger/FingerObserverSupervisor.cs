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
        readonly List<IEnumerator<Observation>> observers;

        /// <summary>
        /// If the enumerator return Result.InAction, it is being focus on
        /// </summary>
        IEnumerator<Observation> selectedObserver;

        public void AddObserver(IObserverFactory observerFactory)
        {
            observerFactories.Add(observerFactory);
            observerFactories.Sort(new ObserverFactoryComparer());
        }

        public void RemoveObserver(IObserverFactory observerFactory)
        {
            observerFactories.Remove(observerFactory);
        }

        public FingerObserverSupervisor(IScreenInput input, ITimer timer)
        {
            this.input = input;
            this.timer = timer;
            observerFactories = new List<IObserverFactory>();
            observers = new List<IEnumerator<Observation>>();
        }

        public void Update()
        {
            if (input.FingerCount == 0) {
                if (!OnEvent()) {
                    foreach (var observer in observers) {
                        observer.Dispose();
                    }

                    selectedObserver = null;
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

                foreach (var factory in observerFactories) {
                    observers.Add(factory.GetObserver(input, timer));
                }

                isFirstOnScreen = false;
            }

            OnEvent();
        }

        void OnDestroy()
        {
            foreach (var observer in observers) {
                observer.Dispose();
            }
        }

        bool OnEvent()
        {
            if (selectedObserver != null) {
                return selectedObserver.MoveNext();
            }

            selectedObserver = observers.Find(o => o.MoveNext() && o.Current == Observation.Fired);
            return selectedObserver != null;
        }

        class ObserverFactoryComparer : IComparer<IObserverFactory>
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