using UnityEngine;

namespace UnityFinger
{
    class DispatchedScreenInput : IScreenInput
    {
        readonly IScreenInput input;
        readonly IScreenInputDispatcher dispatcher;

        public DispatchedScreenInput(IScreenInput input, IScreenInputDispatcher dispatcher)
        {
            this.input = input;
            this.dispatcher = dispatcher;
        }

        public int FingerCount { get { return input.FingerCount; } }

        public Vector2 GetPosition()
        {
            var position = input.GetPosition();
            dispatcher.DispatchPosition(ref position);
            return position;
        }

        public Vector2 GetSecondPosition()
        {
            var position = input.GetSecondPosition();
            dispatcher.DispatchPosition(ref position);
            return position;
        }
    }
}