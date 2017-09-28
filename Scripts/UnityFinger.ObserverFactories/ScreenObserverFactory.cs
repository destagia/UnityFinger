using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityFinger
{
    public interface IScreenListener
    {
        void OnScreen(Vector2 position);
    }
}

namespace UnityFinger.ObserverFactories
{
    public class ScreenObserverFactory : IObserverFactory
    {
        readonly IScreenListener listener;
        
        public ScreenObserverFactory(IScreenListener listener)
        {
            this.listener = listener;
        }

        #region IObserverFactory implementation

        public int Priority { get { return 0; } }

        public IEnumerator<Result> GetObserver(IScreenInput input, IReadOnlyTimer timer)
        {
            listener.OnScreen(input.GetPosition());
            yield break;
        }

        #endregion
    }
}

