using System.Collections.Generic;
using UnityEngine;

namespace UnityFinger
{
    public struct DragInfo
    {
        public readonly Vector2 prevPosition;
        public readonly Vector2 position;
        public readonly Vector2 delta;

        public DragInfo(Vector2 prevPosition, Vector2 position, Vector2 delta)
        {
            this.prevPosition = prevPosition;
            this.position = position;
            this.delta = delta;
        }
    }

    public interface IDragListener
    {
        void OnDragStart(DragInfo info);
        void OnDrag(DragInfo info);
        void OnDragEnd(DragInfo info);
    }
}

namespace UnityFinger.Observers
{
    public class DragObserver : IObserver
    {
        readonly IDragListener listener;

        readonly IFingerObserverConfig config;

        /// <summary>
        /// if IgnoreOtherObservers is true, drag events will be fired
        /// even if the callbacks like swipe are registerd.
        /// it means that drag starts immediately! swipe event will never be fired.
        /// </summary>
        readonly bool ignoreOtherObservers;

        public DragObserver(IFingerObserverConfig config, IDragListener listener, bool ignoreOtherObservers)
        {
            this.config = config;
            this.listener = listener;
            this.ignoreOtherObservers = ignoreOtherObservers;
        }

        #region IObserver implementation

        public int Priority { get { return 300; } }

        public IEnumerator<Result> GetObserver(IScreenInput fingerInput, ITimer timer)
        {
            // store the position the dragging start
            var origin = fingerInput.GetPosition();
            var prevPosition = origin;
            var currentPosition = origin;

            // use isInvoked to garantee the (start)(dragging*)(end) flow
            var isInvoked = false;

            while (fingerInput.FingerCount == 1) {
                if (fingerInput.FingerCount > 1) {
                    yield break;
                }

                if (ignoreOtherObservers || timer.ElapsedTime > config.DragDuration) {
                    prevPosition = currentPosition;
                    currentPosition = fingerInput.GetPosition();

                    var moveDelta = currentPosition - origin;
                    if (moveDelta.magnitude > config.DragDistance) {
                        if (!isInvoked) {
                            listener.OnDragStart(new DragInfo(prevPosition, currentPosition, moveDelta));
                        }
                        isInvoked = true;
                        break;
                    }
                }

                yield return Result.None;
            }

            yield return Result.InAction;

            if (fingerInput.FingerCount > 0) {
                prevPosition = currentPosition;
                currentPosition = fingerInput.GetPosition();
            }

            if (isInvoked) {
                while (fingerInput.FingerCount > 0) {
                    listener.OnDrag(new DragInfo(prevPosition, currentPosition, currentPosition - origin));
                    prevPosition = currentPosition;
                    currentPosition = fingerInput.GetPosition();
                    yield return Result.InAction;
                }

                listener.OnDragEnd(new DragInfo(prevPosition, currentPosition, currentPosition - origin));
                yield return Result.InAction;
            }
        }

        #endregion
    }
}

