using NUnit.Framework;
using System.Collections.Generic;
using UnityFinger.Observers;
using System;
using UnityEngine;

namespace UnityFinger.Tests
{
    [TestFixture]
    class ScreenObserverTest : IScreenListener
    {
        TestInput input;
        TestTimer timer;

        ScreenObserver observer;
        IEnumerator<Result> enumerator;

        Action<Vector2> onScreen;

        [SetUp]
        public void SetUp()
        {
            input = new TestInput();
            timer = new TestTimer();

            onScreen = p => { };

            observer = new ScreenObserver(this);

            enumerator = observer.GetObserver(input, timer);
        }

        void IScreenListener.OnScreen(Vector2 position)
        {
            onScreen(position);
        }

        [Test]
        public void Works()
        {
            Vector2 position = Vector2.zero;
            onScreen = p => position = p;

            input.SetPosition(new Vector2(5, 5));

            Assert.AreEqual(new Vector2(5, 5), input.GetPosition());

            Assert.False(enumerator.MoveNext());
            Assert.AreEqual(Result.None, enumerator.Current);
            Assert.AreEqual(new Vector2(5, 5), position);
        }
    }
}
