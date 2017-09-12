﻿using NUnit.Framework;
using UnityFinger.Observers;
using UnityEngine;

namespace UnityFinger.Tests
{
    [TestFixture(TestName = "TapObserver")]
    class TapObserverTest : ITapListener
    {
        ObserverTestSet<TapObserver> testSet;

        Vector2? tapPosition = null;

        [SetUp]
        public void SetUp()
        {
            tapPosition = null;

            testSet = new ObserverTestSet<TapObserver>();
            testSet.SetUp(() => new TapObserver(new TestConfig(), this));
        }

        void ITapListener.OnTap(Vector2 position)
        {
            tapPosition = position;
        }

        [Test]
        public void Works()
        {
            // A finger is on the screen
            testSet.Input.FingerCount = 1;
            testSet.Input.SetPosition(new Vector2(5, 5));

            Assert.True(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Result.None, testSet.Enumerator.Current);

            // A finger was released from the screen
            testSet.Input.FingerCount = 0;

            Assert.True(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Result.InAction, testSet.Enumerator.Current);

            Assert.True(tapPosition.HasValue);
            Assert.AreEqual(new Vector2(5, 5), tapPosition.Value);
        }

        [Test]
        public void WorksIfFingerMoveLittle()
        {
            // A finger is on the screen
            testSet.Input.FingerCount = 1;
            testSet.Input.SetPosition(new Vector2(5, 5));

            Assert.True(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Result.None, testSet.Enumerator.Current);

            // In the next frame, the finger has moved a little
            testSet.Input.SetPosition(new Vector2(5.1f, 5.1f));

            Assert.True(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Result.None, testSet.Enumerator.Current);

            // The finger was released from the screen
            testSet.Input.FingerCount = 0;

            Assert.True(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Result.InAction, testSet.Enumerator.Current);

            Assert.True(tapPosition.HasValue);
            Assert.AreEqual(new Vector2(5.1f, 5.1f), tapPosition.Value);
        }

        [Test]
        public void FailsIfFingerCountIsOver()
        {
            // Two finger is on the screen
            testSet.Input.FingerCount = 2;
            testSet.Input.SetPosition(new Vector2(5, 5));

            Assert.False(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Result.None, testSet.Enumerator.Current);
            Assert.False(tapPosition.HasValue);
        }

        [Test]
        public void FailsIfFingerMoveMuch()
        {
            // A finger is on the screen
            testSet.Input.FingerCount = 1;
            testSet.Input.SetPosition(new Vector2(5, 5));

            Assert.True(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Result.None, testSet.Enumerator.Current);

            // In the next frame, the finger has moved too much
            testSet.Input.SetPosition(new Vector2(100, 100));

            Assert.False(testSet.Enumerator.MoveNext());
            Assert.False(tapPosition.HasValue);
        }

        [Test]
        public void FailsIfTimeIsOver()
        {
            // A finger is on the screen
            testSet.Input.FingerCount = 1;
            testSet.Input.SetPosition(new Vector2(5, 5));

            Assert.True(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Result.None, testSet.Enumerator.Current);

            // It takes too many times
            testSet.Input.FingerCount = 0;
            testSet.Timer.ElapsedTime = 2f;

            Assert.False(testSet.Enumerator.MoveNext());
            Assert.False(tapPosition.HasValue);
        }
    }
}
