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

namespace UnityFinger.Factories
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

        public IEnumerator<Observation> GetObserver(IScreenInput input, IReadOnlyTimer timer)
        {
            listener.OnScreen(input.GetPosition());
            yield break;
        }

        #endregion
    }
}

