using NUnit.Framework;
using UnityFinger.Observers;
using UnityEngine;
using System;

namespace UnityFinger.Test
{
    class FingerEventManagerTest
    {
        TestInput input;
        TestTimer timer;

        FingerObserverSupervisor supervisor;
        FingerEventManager manager;

        Vector2? onScreenPosition;

        [SetUp]
        public void SetUp()
        {
            input = new TestInput();
            timer = new TestTimer();

            supervisor = new FingerObserverSupervisor(input, timer);
            manager = new FingerEventManager(supervisor, new DefaultFingerObserverConfig());

            onScreenPosition = null;
            manager.AddOnScreenListener(p => {
                onScreenPosition = p;
            });
        }

        [Test]
        public void Works()
        {
            Update();

            input.FingerCount = 1;
            input.SetPosition(new Vector2(5, 5));

            Update();

            Assert.IsTrue(onScreenPosition.HasValue);
            Assert.AreEqual(new Vector2(5, 5), onScreenPosition.Value);
        }

        void Update()
        {
            supervisor.Update();
        }
    }
}
