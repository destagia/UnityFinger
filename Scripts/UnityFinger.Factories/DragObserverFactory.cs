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

    [Flags]
    public enum DragOptionFlag
    {
        /// <summary>
        /// Nothing special
        /// </summary>
        None = 0x000,

        /// <summary>
        /// Does't wait for finger moving
        /// </summary>
        Immediate = 0x001,

        /// <summary>
        /// if IgnoreOtherObservers is true, drag events will be fired
        /// even if the callbacks like swipe are registerd.
        /// it means that drag starts immediately! swipe event will never be fired.
        /// </summary>
        IgnoreOthers = 0x002,
    }
}

namespace UnityFinger.Factories
{
    public class DragObserverFactory : ObserverFactoryBase<IDragListener>
    {
        class DragOption
        {
            readonly DragOptionFlag flag;

            public DragOption(DragOptionFlag flag)
            {
                this.flag = flag;
            }

            public bool IgnoreOthers {
                get { return (flag & DragOptionFlag.IgnoreOthers) != 0; }
            }

            public bool Immediate {
                get { return (flag & DragOptionFlag.Immediate) != 0; }
            }
        }

        public DragObserverFactory(IFingerObserverConfig config, IDragListener listener)
            : base(config, listener)
        {
        }

        #region IObserverFactory implementation

        public override int Priority { get { return 300; } }

        public override IEnumerator<Observation> GetObserver(IScreenInput input, IReadOnlyTimer timer)
        {
            var option = new DragOption(Config.DragOptionFlag);

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

                if (!option.IgnoreOthers && timer.ElapsedTime < Config.DragDuration) {
                    yield return Observation.None;
                    continue;
                }

                var moveDelta = currentPosition - origin;
                if (!option.Immediate && moveDelta.magnitude < Config.DragDistance) {
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

