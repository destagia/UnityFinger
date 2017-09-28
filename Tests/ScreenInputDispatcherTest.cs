using NUnit.Framework;
using UnityEngine;
using UnityFinger.Dispatcher;

namespace UnityFinger.Test
{
    class ScreenInputDispatcherTest
    {
        class DoubleDispatcher : IScreenInputDispatcher
        {
            public void DispatchDistance(ref float distance)
            {
                distance = distance * 2;
            }

            public void DispatchPosition(ref Vector2 position)
            {
                position = position * 2;
            }
        }

        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void Works()
        {
        }
    }
}
