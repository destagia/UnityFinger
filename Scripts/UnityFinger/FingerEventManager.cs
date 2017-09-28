using UnityEngine;
using UnityEngine.Events;
using UnityFinger.Factories;
using UnityFinger.Events;

namespace UnityFinger
{
    public class FingerEventManager :
        IScreenListener,
        ITapListener,
        IDragListener,
        IFlickListener,
        ITwoFingersListener,
        ILongTapListener,
        IPinchListener
    {
        void IScreenListener.OnScreen(Vector2 position)
        {
            onScreen.Invoke(position);
        }

        void ITapListener.OnTap(Vector2 position)
        {
            onTap.Invoke(position);
        }

        void IDragListener.OnDragStart(DragInfo info)
        {
            onDragStart.Invoke(info);
        }

        void IDragListener.OnDrag(DragInfo info)
        {
            onDrag.Invoke(info);
        }

        void IDragListener.OnDragEnd(DragInfo info)
        {
            onDragEnd.Invoke(info);
        }

        void IFlickListener.OnFlick(FlickInfo info)
        {
            onFlick.Invoke(info);
        }

        void ITwoFingersListener.OnTwoFingersTap(TwoFingersTapInfo info)
        {
            onTwoFingersTap.Invoke(info);
        }

        void ILongTapListener.OnLongTap(Vector2 position)
        {
            onLongTap.Invoke(position);
        }

        void IPinchListener.OnPinch(PinchInfo info)
        {
            onPinch.Invoke(info);
        }

        void IPinchListener.OnPinchStart(PinchInfo info)
        {
            onPinchStart.Invoke(info);
        }

        void IPinchListener.OnPinchEnd()
        {
            onPinchEnd.Invoke();
        }

        readonly FingerObserverSupervisor supervisor;

        readonly ScreenObserverFactory screenObserver;
        readonly TapObserverFactory tapObserver;
        readonly FlickObserverFactory flickObserver;
        readonly DragObserverFactory dragObserver;
        readonly DragObserverFactory ignoreOthersDragObserver;
        readonly TwoFingersTapObserverFactory twoFingersTapObserver;
        readonly LongTapObserverFactory longTapObserver;
        readonly PinchObserverFactory pinchObserver;

        readonly PositionEvent onScreen;
        readonly PositionEvent onTap;
        readonly FlickEvent onFlick;
        readonly DragEvent onDragStart;
        readonly DragEvent onDrag;
        readonly DragEvent onDragEnd;
        readonly CompositeEvent dragEvents;
        readonly CompositeEvent ignoreOthersDragEvents;
        readonly TwoFingersEvent onTwoFingersTap;
        readonly PositionEvent onLongTap;
        readonly PinchEvent onPinchStart;
        readonly PinchEvent onPinch;
        readonly VoidEvent onPinchEnd;
        readonly CompositeEvent pinchEvents;

        public FingerEventManager(FingerObserverSupervisor supervisor, IFingerObserverConfig config)
        {
            this.supervisor = supervisor;

            onScreen = new PositionEvent();
            onTap = new PositionEvent();
            onFlick = new FlickEvent();
            onDragStart = new DragEvent();
            onDrag = new DragEvent();
            onDragEnd = new DragEvent();
            dragEvents = new CompositeEvent(onDragStart, onDrag, onDragEnd);
            ignoreOthersDragEvents = new CompositeEvent(onDragStart, onDrag, onDragEnd);
            onTwoFingersTap = new TwoFingersEvent();
            onLongTap = new PositionEvent();
            onPinchStart = new PinchEvent();
            onPinch = new PinchEvent();
            onPinchEnd = new VoidEvent();
            pinchEvents = new CompositeEvent(onPinchStart, onPinch, onPinchEnd);

            screenObserver = new ScreenObserverFactory(this);
            tapObserver = new TapObserverFactory(config, this);
            flickObserver = new FlickObserverFactory(config, this);
            dragObserver = new DragObserverFactory(config, this, false);
            ignoreOthersDragObserver = new DragObserverFactory(config, this, true);
            twoFingersTapObserver = new TwoFingersTapObserverFactory(config, this);
            longTapObserver = new LongTapObserverFactory(config, this);
            pinchObserver = new PinchObserverFactory(config, this);
        }

        void RegisterObserver(ICountableEvent countableEvent, IObserverFactory target)
        {
            if (countableEvent.ListenersCount == 0) {
                supervisor.AddObserver(target);
            }
        }

        void UnregisterObserver(ICountableEvent countableEvent, IObserverFactory target)
        {
            if (countableEvent.ListenersCount == 0) {
                supervisor.RemoveObserver(target);
            }
        }

        public void AddOnScreenListener(UnityAction<Vector2> action)
        {
            RegisterObserver(onScreen, screenObserver);
            onScreen.AddListener(action);
        }

        public void RemoveOnScreenListener(UnityAction<Vector2> action)
        {
            onScreen.RemoveListener(action);
            UnregisterObserver(onScreen, screenObserver);
        }

        public void AddOnDragStartListener(UnityAction<DragInfo> action)
        {
            RegisterObserver(dragEvents, dragObserver);
            onDragStart.AddListener(action);
        }

        public void RemoveOnDragStartListener(UnityAction<DragInfo> action)
        {
            onDragStart.RemoveListener(action);
            UnregisterObserver(dragEvents, dragObserver);
        }

        public void AddOnDragListener(UnityAction<DragInfo> action)
        {
            RegisterObserver(dragEvents, dragObserver);
            onDrag.AddListener(action);
        }

        public void RemoveOnDragListener(UnityAction<DragInfo> action)
        {
            onDrag.RemoveListener(action);
            UnregisterObserver(dragEvents, dragObserver);
        }

        public void AddOnIgnoreOthersDragListener(UnityAction<DragInfo> action)
        {
            RegisterObserver(ignoreOthersDragEvents, ignoreOthersDragObserver);
            onDrag.AddListener(action);
        }

        public void RemoveOnIgnoreOthersDragListener(UnityAction<DragInfo> action)
        {
            onDrag.RemoveListener(action);
            UnregisterObserver(ignoreOthersDragEvents, ignoreOthersDragObserver);
        }

        public void AddOnDragEndListener(UnityAction<DragInfo> action)
        {
            RegisterObserver(dragEvents, dragObserver);
            onDragEnd.AddListener(action);
        }

        public void RemoveOnDragEndListener(UnityAction<DragInfo> action)
        {
            onDragEnd.RemoveListener(action);
            UnregisterObserver(dragEvents, dragObserver);
        }

        public void AddOnTapListener(UnityAction<Vector2> action)
        {
            RegisterObserver(onTap, tapObserver);
            onTap.AddListener(action);
        }

        public void RemoveOnTapListener(UnityAction<Vector2> action)
        {
            onTap.RemoveListener(action);
            UnregisterObserver(onTap, tapObserver);
        }

        public void AddOnFlickListener(UnityAction<FlickInfo> action)
        {
            RegisterObserver(onFlick, flickObserver);
            onFlick.AddListener(action);
        }

        public void RemoveOnFlickListener(UnityAction<FlickInfo> action)
        {
            onFlick.RemoveListener(action);
            UnregisterObserver(onFlick, flickObserver);
        }

        public void AddOnTwoFingerTapListener(UnityAction<TwoFingersTapInfo> action)
        {
            RegisterObserver(onTwoFingersTap, twoFingersTapObserver);
            onTwoFingersTap.AddListener(action);
        }

        public void RemoveOnTwoFingerTapListener(UnityAction<TwoFingersTapInfo> action)
        {
            onTwoFingersTap.RemoveListener(action);
            UnregisterObserver(onTwoFingersTap, twoFingersTapObserver);
        }

        public void AddOnLongTapListener(UnityAction<Vector2> action)
        {
            RegisterObserver(onLongTap, longTapObserver);
            onLongTap.AddListener(action);
        }

        public void RemoveOnLongTapListener(UnityAction<Vector2> action)
        {
            onLongTap.RemoveListener(action);
            UnregisterObserver(onLongTap, longTapObserver);
        }

        public void AddOnPinchListener(UnityAction<PinchInfo> action)
        {
            RegisterObserver(pinchEvents, pinchObserver);
            onPinch.AddListener(action);
        }

        public void RemoveOnPinchListener(UnityAction<PinchInfo> action)
        {
            onPinch.RemoveListener(action);
            UnregisterObserver(pinchEvents, pinchObserver);
        }

        public void AddOnPinchStartListener(UnityAction<PinchInfo> action)
        {
            RegisterObserver(pinchEvents, pinchObserver);
            onPinchStart.AddListener(action);
        }

        public void RemoveOnPinchStartListener(UnityAction<PinchInfo> action)
        {
            onPinchStart.RemoveListener(action);
            UnregisterObserver(pinchEvents, pinchObserver);
        }

        public void AddOnPinchEndListener(UnityAction action)
        {
            RegisterObserver(pinchEvents, pinchObserver);
            onPinchEnd.AddListener(action);
        }

        public void RemoveOnPinchEndListener(UnityAction action)
        {
            onPinchEnd.RemoveListener(action);
            UnregisterObserver(pinchEvents, pinchObserver);
        }
    }
}