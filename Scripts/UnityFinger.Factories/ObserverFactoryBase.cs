using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityFinger.Factories
{
    public abstract class ObserverFactoryBase<TListener> : IObserverFactory
    {
        public abstract int Priority { get; }

        public abstract IEnumerator<Observation> GetObserver(IScreenInput input, IReadOnlyTimer timer);

        readonly IFingerObserverConfig config;
        readonly TListener listener;

        protected ObserverFactoryBase(IFingerObserverConfig config, TListener listener)
        {
            this.config = config;
            this.listener = listener;
        }

        protected IFingerObserverConfig Config {
            get { return config; }
        }
        protected TListener Listener {
            get { return listener; }
        }
    }
}
