using System.Collections.Generic;
using System;

namespace UnityFinger.Tests
{
    class ObserverTestSet<TObserver> where TObserver : IObserver
    {
        TestInput input;
        TestTimer timer;
        TObserver observer;
        IEnumerator<Result> enumerator;

        public TestInput Input { get { return input; } }

        public TestTimer Timer { get { return timer; } }

        public TObserver Observer { get { return observer; } }

        public IEnumerator<Result> Enumerator { get { return enumerator; } }

        public void SetUp(Func<TObserver> createObserver)
        {
            input = new TestInput();
            timer = new TestTimer();
            observer = createObserver();
            enumerator = observer.GetObserver(input, timer);
        }
    }
}
