using System.Collections.Generic;
using System;

namespace UnityFinger.Test
{
    class ObserverTestSet<TObserver> where TObserver : IObserverFactory
    {
        TestInput input;
        TestTimer timer;
        TObserver observer;
        IEnumerator<Observation> enumerator;

        public TestInput Input { get { return input; } }

        public TestTimer Timer { get { return timer; } }

        public TObserver Observer { get { return observer; } }

        public IEnumerator<Observation> Enumerator { get { return enumerator; } }

        public void SetUp(Func<TObserver> createObserver)
        {
            input = new TestInput();
            timer = new TestTimer();
            observer = createObserver();
            enumerator = observer.GetObserver(input, timer);
        }
    }
}
