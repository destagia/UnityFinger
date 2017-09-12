using NUnit.Framework;
using System.Collections.Generic;
using UnityFinger.Observers;
using System;
using UnityEngine;

namespace UnityFinger.Tests
{
    [TestFixture(TestName = "TapObserver")]
    class TapObserverTest : ITapListener
    {
        TestInput input;
        TestTimer timer;

        TapObserver observer;
        IEnumerator<Result> enumerator;

        Vector2? tapPosition = null;

        [SetUp]
        public void SetUp()
        {
            input = new TestInput();
            timer = new TestTimer();

            tapPosition = null;

            observer = new TapObserver(new TestConfig(), this);

            enumerator = observer.GetObserver(input, timer);
        }

        void ITapListener.OnTap(Vector2 position)
        {
            tapPosition = position;
        }

        [Test]
        public void Works()
        {
            // A finger is on the screen
            input.FingerCount = 1;
            input.SetPosition(new Vector2(5, 5));

            Assert.True(enumerator.MoveNext());
            Assert.AreEqual(Result.None, enumerator.Current);

            // A finger was released from the screen
            input.FingerCount = 0;

            Assert.True(enumerator.MoveNext());
            Assert.AreEqual(Result.InAction, enumerator.Current);

            Assert.True(tapPosition.HasValue);
            Assert.AreEqual(new Vector2(5, 5), tapPosition.Value);
        }

        [Test]
        public void WorksIfFingerMoveLittle()
        {
            // A finger is on the screen
            input.FingerCount = 1;
            input.SetPosition(new Vector2(5, 5));

            Assert.True(enumerator.MoveNext());
            Assert.AreEqual(Result.None, enumerator.Current);

            // In the next frame, the finger has moved a little
            input.SetPosition(new Vector2(5.1f, 5.1f));

            Assert.True(enumerator.MoveNext());
            Assert.AreEqual(Result.None, enumerator.Current);

            // The finger was released from the screen
            input.FingerCount = 0;

            Assert.True(enumerator.MoveNext());
            Assert.AreEqual(Result.InAction, enumerator.Current);

            Assert.True(tapPosition.HasValue);
            Assert.AreEqual(new Vector2(5.1f, 5.1f), tapPosition.Value);
        }

        [Test]
        public void FailsIfFingerCountIsOver()
        {
            // Two finger is on the screen
            input.FingerCount = 2;
            input.SetPosition(new Vector2(5, 5));

            Assert.False(enumerator.MoveNext());
            Assert.AreEqual(Result.None, enumerator.Current);
            Assert.False(tapPosition.HasValue);
        }

        [Test]
        public void FailsIfFingerMoveMuch()
        {
            // A finger is on the screen
            input.FingerCount = 1;
            input.SetPosition(new Vector2(5, 5));

            Assert.True(enumerator.MoveNext());
            Assert.AreEqual(Result.None, enumerator.Current);

            // In the next frame, the finger has moved too much
            input.SetPosition(new Vector2(100, 100));

            Assert.False(enumerator.MoveNext());
            Assert.False(tapPosition.HasValue);
        }

        [Test]
        public void FailsIfTimeIsOver()
        {
            // A finger is on the screen
            input.FingerCount = 1;
            input.SetPosition(new Vector2(5, 5));

            Assert.True(enumerator.MoveNext());
            Assert.AreEqual(Result.None, enumerator.Current);

            // It takes too many times
            input.FingerCount = 0;
            timer.ElapsedTime = 2f;

            Assert.False(enumerator.MoveNext());
            Assert.False(tapPosition.HasValue);
        }
    }
}
