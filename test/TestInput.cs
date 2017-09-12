using UnityEngine;

namespace UnityFinger.Tests
{
    class TestInput : IScreenInput
    {
        public int FingerCount { get; set; }

        Vector2 position;

        Vector2 secondPosition;

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        public void SetSecondPosition(Vector2 secondPosition)
        {
            this.secondPosition = secondPosition;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public Vector2 GetSecondPosition()
        {
            return secondPosition;
        }
    }
}
