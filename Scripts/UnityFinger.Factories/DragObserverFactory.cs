using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityFinger
{
    public interface IDragListener
    {
        void OnDragStart(DragInfo info);
        void OnDrag(DragInfo info);
        void OnDragEnd(DragInfo info);
    }
}

namespace UnityFinger.Factories
{
    public class DragObserverFactory : ObserverFactoryBase<IDragListener>
    {
        /// <summary>
        /// if IgnoreOtherObservers is true, drag events will be fired
        /// even if the callbacks like swipe are registerd.
        /// it means that drag starts immediately! swipe event will never be fired.
        /// </summary>
        readonly bool ignoreOtherObservers;

        public DragObserverFactory(IFingerObserverConfig config, IDragListener listener, bool ignoreOtherObservers)
            : base(config, listener)
        {
            this.ignoreOtherObservers = ignoreOtherObservers;
        }

        #region IObserverFactory implementation

        public override int Priority { get { return 300; } }

        public override IEnumerator<Observation> GetObserver(IScreenInput input, IReadOnlyTimer timer)
        {
            // store the position the dragging start
            var origin = input.GetPosition();
            var prevPosition = origin;
            var currentPosition = origin;

            while (input.FingerCount == 1) {
                if (input.FingerCount > 1) {
                    yield break;
                }

                prevPosition = currentPosition;
                currentPosition = input.GetPosition();

                if (!ignoreOtherObservers && timer.ElapsedTime < Config.DragDuration) {
                    yield return Observation.None;
                    continue;
                }

                var moveDelta = currentPosition - origin;
                if (moveDelta.magnitude < Config.DragDistance) {
                    yield return Observation.None;
                    continue;
                }

                Listener.OnDragStart(new DragInfo(origin, prevPosition, currentPosition));
                yield return Observation.Fired;
                break;
            }

            while (input.FingerCount > 0) {
                prevPosition = currentPosition;
                currentPosition = input.GetPosition();

                Listener.OnDrag(new DragInfo(origin, prevPosition, currentPosition));
                yield return Observation.Fired;
            }

            prevPosition = currentPosition;
            currentPosition = input.GetPosition();

            Listener.OnDragEnd(new DragInfo(origin, prevPosition, currentPosition));
            yield return Observation.Fired;
        }

        #endregion
    }
}

